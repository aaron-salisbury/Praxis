using Praxis.Engine.Tiers.Application;
using Praxis.Engine.Tiers.Application.BoardRepresentation;
using Praxis.Engine.Tiers.Application.BoardRepresentation.Pieces;
using Praxis.Engine.Tiers.Application.Evaluation;
using Praxis.Engine.Tiers.Data;
using Praxis.Engine.Tiers.Presentation;
using Serilog;
using System;
using System.Collections.Generic;
using static Praxis.Engine.Tiers.Application.Types;

namespace Praxis.Engine
{
    public class Engine
    {
        internal const string NAME = "Praxis";
        internal const string AUTHOR = "Aaron Salisbury";
        private const string FEN_START_POSITIONS = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1";
        private const int NUM_MEN_ENDGAME_ANALYSIS = 7; // SyzygyTablebaseProber

        internal static ILogger Logger { get; set; }

        /// <summary>
        /// Is the engine currently active and communicating with it's interface.
        /// </summary>
        public bool IsActive { get; set; }
        /// <summary>
        /// Types of chess interface protocols.
        /// </summary>
        public enum Protocols { UCI, CECP }
        /// <summary>
        /// Forsyth–Edwards Notation that describes the current board position.
        /// </summary>
        public string FEN
        {
            get
            {
                if (Board == null)
                {
                    return string.Empty;
                }
                else
                {
                    string piecePlacement = string.Empty;
                    #region Piece Placement
                    for (int rankIndex = 7; rankIndex >= 0; rankIndex--)
                    {
                        int emptySquareCounter = 0;

                        for (int fileIndex = 0; fileIndex <= 7; fileIndex++)
                        {
                            Piece currentPiece = Board.Squares[fileIndex, rankIndex];

                            if (currentPiece == null)
                            {
                                emptySquareCounter++;
                            }
                            else
                            {
                                if (emptySquareCounter > 0)
                                {
                                    piecePlacement = piecePlacement + emptySquareCounter.ToString();
                                    emptySquareCounter = 0;
                                }

                                #region Piece Switch
                                switch (currentPiece.PieceType)
                                {
                                    case PieceTypes.Rook:
                                        piecePlacement += (currentPiece.Player.Color == Colors.White) ? "R" : "r";
                                        break;
                                    case PieceTypes.Knight:
                                        piecePlacement += (currentPiece.Player.Color == Colors.White) ? "N" : "n";
                                        break;
                                    case PieceTypes.Bishop:
                                        piecePlacement += (currentPiece.Player.Color == Colors.White) ? "B" : "b";
                                        break;
                                    case PieceTypes.Queen:
                                        piecePlacement += (currentPiece.Player.Color == Colors.White) ? "Q" : "q";
                                        break;
                                    case PieceTypes.King:
                                        piecePlacement += (currentPiece.Player.Color == Colors.White) ? "K" : "k";
                                        break;
                                    case PieceTypes.Pawn:
                                        piecePlacement += (currentPiece.Player.Color == Colors.White) ? "P" : "p";
                                        break;
                                }
                                #endregion
                            }

                            if (fileIndex == 7)
                            {
                                if (emptySquareCounter > 0)
                                {
                                    piecePlacement = piecePlacement + emptySquareCounter.ToString();
                                }

                                if (rankIndex != 0)
                                {
                                    piecePlacement = piecePlacement + "/";
                                }
                            }
                        }
                    }
                    #endregion

                    string activeColor = (WhitePlayer.MovesNext) ? "w" : "b";

                    string castling = string.Empty;
                    #region Castling
                    if (WhitePlayer.CanCastleKingside)
                    {
                        castling = castling + "K";
                    }
                    if (WhitePlayer.CanCastleQueenside)
                    {
                        castling = castling + "Q";
                    }
                    if (BlackPlayer.CanCastleKingside)
                    {
                        castling = castling + "k";
                    }
                    if (BlackPlayer.CanCastleQueenside)
                    {
                        castling = castling + "q";
                    }
                    if (castling.Equals(string.Empty))
                    {
                        castling = "-";
                    }
                    #endregion

                    string enPassant = "-";
                    if (EnPassantFile != null && EnPassantRank != null)
                    {
                        enPassant = EnPassantFile.ToString() + (int)EnPassantRank;
                    }

                    return string.Format("{0} {1} {2} {3} {4} {5}", piecePlacement, activeColor, castling, enPassant, HalfmoveCounter.ToString(), FullmoveCounter.ToString());
                }
            }
        }
        /// <summary>
        /// Standard Algebraic Notation that describes the current game.
        /// </summary>
        public string SAN { get; set; }
        /// <summary>
        /// Internal board representation.
        /// </summary>
        public Board Board { get; set; }
        /// <summary>
        /// The color of Player whose turn is next.
        /// </summary>
        public Colors TurnColor
        {
            get
            {
                if (WhitePlayer.MovesNext)
                {
                    return Colors.White;
                }
                else
                {
                    return Colors.Black;
                }
            }
        }

        private Stages _stage;
        internal Stages Stage
        {
            get { return _stage; }
            set
            {
                _stage = value;

                if (_stage == Stages.Opening)
                {
                    MoveEvaluator = new OpeningMoveEvaluator(this);
                }
                else if (_stage == Stages.Middle)
                {
                    MoveEvaluator = new MiddleGameMoveEvaluator(this);
                }
                else
                {
                    MoveEvaluator = new EndGameMoveEvaluator(this);
                }

            }
        }

        internal Player WhitePlayer { get; set; }
        internal Player BlackPlayer { get; set; }
        internal Files? EnPassantFile { get; set; }
        internal Ranks? EnPassantRank { get; set; }
        internal int HalfmoveCounter { get; set; }
        internal int FullmoveCounter { get; set; }
        internal Protocols CommunicationType { get; set; }
        internal CommunicationProtocol CommunicationProtocol { get; set; }
        internal Random RandomGenerator { get; set; }
        internal MoveEvaluator MoveEvaluator { get; set; }
        internal List<Opening> Openings { get; set; }

        /// <summary>
        /// Create chess engine for move generation and internal board representation.
        /// </summary>
        /// <param name="presentation">Type of communication protocol the interface uses.</param>
        public Engine(Protocols presentation)
        {
            Logger = new LoggerConfiguration()
                .WriteTo.Console()
                .CreateLogger();

            CommunicationType = presentation;
            IsActive = true;
            RandomGenerator = new Random();

            if (CommunicationType == Protocols.UCI)
            {
                CommunicationProtocol = new UCI(this);
            }
            else if (CommunicationType == Protocols.CECP)
            {
                CommunicationProtocol = new CECP(this);
            }
        }

        /// <summary>
        /// Send input from the interface to the engine for processing.
        /// </summary>
        /// <param name="input">Command or data from interface.</param>
        /// <returns>Engine processing results.</returns>
        public List<string> ProcessInputFromInterface(string input)
        {
            return CommunicationProtocol.ProcessInput(input);
        }

        internal void CostlySetup()
        {
            // Conduct time consuming setup activities here and only call when the interface is expecting to wait.

            if (Openings == null)
            {
                Openings = Access.GetOpenings();
            }
        }

        internal void LoadGame(string fenRecord = null)
        {
            InitializeEngine();

            if (string.IsNullOrEmpty(fenRecord))
            {
                fenRecord = FEN_START_POSITIONS;
            }

            CreateGameFromFEN(fenRecord);

            SurveyGameStage();
        }

        internal void SurveyGameStage()
        {
            if (Stage != Stages.End)
            {
                if (Board.NumberOfPieces <= NUM_MEN_ENDGAME_ANALYSIS)
                {
                    Stage = Stages.End;
                }
                else if (Opening.PotentialOpeningsExist(this))
                {
                    Stage = Stages.Opening;
                }
                else
                {
                    Stage = Stages.Middle;
                }
            }
        }

        internal Engine CloneEngine()
        {
            Engine cloneEngine = new Engine(CommunicationType)
            {
                Openings = Openings
            };

            cloneEngine.LoadGame(FEN);

            return cloneEngine;
        }

        private void CreateGameFromFEN(string fenRecord)
        {
            if (Access.FENRecordIsValid(fenRecord))
            {
                string[] fenFields = fenRecord.Split(default(string[]), StringSplitOptions.RemoveEmptyEntries);

                Files file = Files.a;
                Ranks rank = Ranks.eight;

                // Piece Placement
                foreach (char placementChar in fenFields[0])
                {
                    if (placementChar.Equals('/'))
                    {
                        rank--;
                        file = Files.a;
                    }
                    else if (char.IsNumber(placementChar))
                    {
                        int numEmptySquares = Convert.ToInt32(placementChar.ToString());
                        file += numEmptySquares;
                    }
                    else if (char.IsLetter(placementChar))
                    {
                        #region Piece Switch
                        switch (placementChar)
                        {
                            case 'r':
                                new Rook(BlackPlayer, file, rank);
                                break;
                            case 'n':
                                new Knight(BlackPlayer, file, rank);
                                break;
                            case 'b':
                                new Bishop(BlackPlayer, file, rank);
                                break;
                            case 'q':
                                new Queen(BlackPlayer, file, rank);
                                break;
                            case 'k':
                                new King(BlackPlayer, file, rank);
                                break;
                            case 'p':
                                new Pawn(BlackPlayer, file, rank);
                                break;
                            case 'R':
                                new Rook(WhitePlayer, file, rank);
                                break;
                            case 'N':
                                new Knight(WhitePlayer, file, rank);
                                break;
                            case 'B':
                                new Bishop(WhitePlayer, file, rank);
                                break;
                            case 'Q':
                                new Queen(WhitePlayer, file, rank);
                                break;
                            case 'K':
                                new King(WhitePlayer, file, rank);
                                break;
                            case 'P':
                                new Pawn(WhitePlayer, file, rank);
                                break;
                        }
                        #endregion

                        file++;
                    }
                }

                // Active Color
                if (fenFields[1].Equals("w", StringComparison.OrdinalIgnoreCase))
                {
                    WhitePlayer.MovesNext = true;
                    BlackPlayer.MovesNext = false;
                }
                else if (fenFields[1].ToString().Equals("b", StringComparison.OrdinalIgnoreCase))
                {
                    BlackPlayer.MovesNext = true;
                    WhitePlayer.MovesNext = false;
                }

                // Castling Availability
                WhitePlayer.CanCastleKingside = fenFields[2].Contains("K");
                WhitePlayer.CanCastleQueenside = fenFields[2].Contains("Q");
                BlackPlayer.CanCastleKingside = fenFields[2].Contains("k");
                BlackPlayer.CanCastleQueenside = fenFields[2].Contains("q");

                // En Passant Target
                if (fenFields[3].Equals("-"))
                {
                    EnPassantFile = null;
                    EnPassantRank = null;
                }
                else
                {
                    EnPassantFile = (Files)Enum.Parse(typeof(Files), fenFields[3][0].ToString(), true);
                    EnPassantRank = (Ranks)Convert.ToInt32(fenFields[3][1]);
                }

                // Halfmove Clock
                HalfmoveCounter = Convert.ToInt32(fenFields[4]);

                // Fullmove Number
                FullmoveCounter = Convert.ToInt32(fenFields[5]);
            }
            else
            {
                Logger.Error("Cannot create game from FEN.");
            }
        }

        private void InitializeEngine()
        {
            if (Openings == null)
            {
                Openings = Access.GetOpenings();
            }

            Styles whiteStyle = WhitePlayer != null ? WhitePlayer.Style : Player.GetRandomStyle(RandomGenerator);
            Styles blackStyle = BlackPlayer != null ? BlackPlayer.Style : Player.GetRandomStyle(RandomGenerator);

            WhitePlayer = new Player(Colors.White, this, whiteStyle);
            BlackPlayer = new Player(Colors.Black, this, blackStyle);
            Board = new Board(this);
            SAN = string.Empty;
            Stage = Stages.Opening;
            EnPassantFile = null;
            EnPassantRank = null;
            HalfmoveCounter = 0;
            FullmoveCounter = 1;
        }
    }
}
