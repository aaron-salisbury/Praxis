using Praxis.Business.BoardRepresentation.Pieces;
using System;
using System.Collections.Generic;
using System.Linq;
using static Praxis.Base.Types;

namespace Praxis.Business.BoardRepresentation
{
    public class Board
    {
        public Piece[,] Squares { get; set; }
        internal Engine Engine { get; set; }
        internal int NumberOfPieces
        {
            get
            {
                int numPieces = 0;

                for (int i = 0; i < 8; i++)
                {
                    for (int j = 0; j < 8; j++)
                    {
                        if (Squares[i, j] != null)
                        {
                            numPieces++;
                        }
                    }
                }

                return numPieces;
            }
        }

        internal Board(Engine engine)
        {
            Squares = new Piece[8, 8];
            Engine = engine;
        }

        internal void MovePiece(Move move)
        {
            // Any new move will null out a previous En Passant. A pawn may reinitiate it.
            Engine.EnPassantFile = null;
            Engine.EnPassantRank = null;

            bool isWhitesTurn = Engine.WhitePlayer.MovesNext;
            int firstFileIndex = (int)move.FromFile - 1;
            int firstRankIndex = (int)move.FromRank - 1;
            int secondFileIndex = (int)move.ToFile - 1;
            int secondRankIndex = (int)move.ToRank - 1;
            bool captureMade = Squares[secondFileIndex, secondRankIndex] != null;
            PieceTypes movingPieceType = Squares[firstFileIndex, firstRankIndex].PieceType;

            if (move.Piece == null)
            {
                move.Piece = Squares[firstFileIndex, firstRankIndex];
            }

            if (!captureMade && movingPieceType != PieceTypes.Pawn)
            {
                Engine.HalfmoveCounter++;
            }
            else
            {
                Engine.HalfmoveCounter = 0;
            }

            switch (movingPieceType)
            {
                case PieceTypes.Pawn:
                    #region Handle Pawn Movement
                    if (move.PromotionPieceType != null)
                    {
                        switch (move.PromotionPieceType)
                        {
                            case PieceTypes.Queen:
                                Squares[firstFileIndex, firstRankIndex] = new Queen(Squares[firstFileIndex, firstRankIndex].Player, Squares[firstFileIndex, firstRankIndex].CurrentFile.Value, Squares[firstFileIndex, firstRankIndex].CurrentRank.Value);
                                break;
                            case PieceTypes.Rook:
                                Squares[firstFileIndex, firstRankIndex] = new Rook(Squares[firstFileIndex, firstRankIndex].Player, Squares[firstFileIndex, firstRankIndex].CurrentFile.Value, Squares[firstFileIndex, firstRankIndex].CurrentRank.Value);
                                break;
                            case PieceTypes.Bishop:
                                Squares[firstFileIndex, firstRankIndex] = new Bishop(Squares[firstFileIndex, firstRankIndex].Player, Squares[firstFileIndex, firstRankIndex].CurrentFile.Value, Squares[firstFileIndex, firstRankIndex].CurrentRank.Value);
                                break;
                            case PieceTypes.Knight:
                                Squares[firstFileIndex, firstRankIndex] = new Knight(Squares[firstFileIndex, firstRankIndex].Player, Squares[firstFileIndex, firstRankIndex].CurrentFile.Value, Squares[firstFileIndex, firstRankIndex].CurrentRank.Value);
                                break;
                        }
                    }

                    if (!captureMade && Engine.EnPassantFile != null && Engine.EnPassantRank != null && move.ToFile == Engine.EnPassantFile && move.ToRank == Engine.EnPassantRank)
                    {
                        captureMade = true;
                    }

                    if (EnPassantOccurred(isWhitesTurn, move.FromFile, move.ToFile, move.FromRank, move.ToRank))
                    {
                        Engine.EnPassantFile = move.ToFile;
                        Engine.EnPassantRank = move.ToRank - 1;
                    }
                    #endregion
                    break;
                case PieceTypes.Rook:
                    #region Handle Rook Movement
                    if (isWhitesTurn && move.FromRank == Ranks.one)
                    {
                        if (Engine.WhitePlayer.CanCastleKingside && move.FromFile == Files.h)
                        {
                            Engine.WhitePlayer.CanCastleKingside = false;
                        }
                        else if (Engine.WhitePlayer.CanCastleQueenside && move.FromFile == Files.a)
                        {
                            Engine.WhitePlayer.CanCastleQueenside = false;
                        }
                    }
                    else if (!isWhitesTurn && move.FromRank == Ranks.eight)
                    {
                        if (Engine.BlackPlayer.CanCastleKingside && move.FromFile == Files.h)
                        {
                            Engine.BlackPlayer.CanCastleKingside = false;
                        }
                        else if (Engine.BlackPlayer.CanCastleQueenside && move.FromFile == Files.a)
                        {
                            Engine.BlackPlayer.CanCastleQueenside = false;
                        }
                    }
                    #endregion
                    break;
                case PieceTypes.King:
                    #region Handle King Movement
                    if (isWhitesTurn)
                    {
                        if (move.FromFile == Files.e && move.FromRank == Ranks.one && move.ToRank == Ranks.one)
                        {
                            // Move Rook if necessary.
                            if (move.ToFile == Files.g)
                            {
                                MakePhysicalMove(new Move()
                                {
                                    Piece = Squares[(int)Files.h - 1, (int)Ranks.one - 1],
                                    FromFile = Files.h,
                                    FromRank = Ranks.one,
                                    ToFile = Files.f,
                                    ToRank = Ranks.one,
                                    IsKingsideCastle = true
                                });

                                move.IsKingsideCastle = true;
                            }
                            else if (move.ToFile == Files.c)
                            {
                                MakePhysicalMove(new Move()
                                {
                                    Piece = Squares[(int)Files.a - 1, (int)Ranks.one - 1],
                                    FromFile = Files.a,
                                    FromRank = Ranks.one,
                                    ToFile = Files.d,
                                    ToRank = Ranks.one,
                                    IsQueensideCastle = true
                                });

                                move.IsQueensideCastle = true;
                            }
                        }

                        Engine.WhitePlayer.CanCastleKingside = false;
                        Engine.WhitePlayer.CanCastleQueenside = false;
                    }
                    else
                    {
                        if (move.FromFile == Files.e && move.FromRank == Ranks.eight && move.ToRank == Ranks.eight)
                        {
                            // Move Rook if necessary.
                            if (move.ToFile == Files.g)
                            {
                                MakePhysicalMove(new Move()
                                {
                                    Piece = Squares[(int)Files.h - 1, (int)Ranks.eight - 1],
                                    FromFile = Files.h,
                                    FromRank = Ranks.eight,
                                    ToFile = Files.f,
                                    ToRank = Ranks.eight,
                                    IsKingsideCastle = true
                                });

                                move.IsKingsideCastle = true;
                            }
                            else if (move.ToFile == Files.c)
                            {
                                MakePhysicalMove(new Move()
                                {
                                    Piece = Squares[(int)Files.a - 1, (int)Ranks.eight - 1],
                                    FromFile = Files.a,
                                    FromRank = Ranks.eight,
                                    ToFile = Files.d,
                                    ToRank = Ranks.eight,
                                    IsQueensideCastle = true
                                });

                                move.IsQueensideCastle = true;
                            }
                        }

                        Engine.BlackPlayer.CanCastleKingside = false;
                        Engine.BlackPlayer.CanCastleQueenside = false;
                    }
                    #endregion
                    break;
            }

            if (captureMade)
            {
                Squares[secondFileIndex, secondRankIndex].CurrentFile = null;
                Squares[secondFileIndex, secondRankIndex].CurrentRank = null;
            }

            MakePhysicalMove(move);
        }

        private void MakePhysicalMove(Move move)
        {
            move.AddToRunningSAN();

            Squares[(int)move.FromFile - 1, (int)move.FromRank - 1].CurrentFile = move.ToFile;
            Squares[(int)move.FromFile - 1, (int)move.FromRank - 1].CurrentRank = move.ToRank;
            Squares[(int)move.ToFile - 1, (int)move.ToRank - 1] = Squares[(int)move.FromFile - 1, (int)move.FromRank - 1].ClonePiece();
            Squares[(int)move.FromFile - 1, (int)move.FromRank - 1] = null;

            if (Engine.BlackPlayer.MovesNext)
            {
                Engine.FullmoveCounter++;
            }

            Engine.SurveyGameStage();

            //SurveyKingThreatOfCapture(move);

            Engine.WhitePlayer.MovesNext = !Engine.WhitePlayer.MovesNext;
            Engine.BlackPlayer.MovesNext = !Engine.BlackPlayer.MovesNext;
        }

        private void SurveyKingThreatOfCapture(Move move)
        {
            Player player = Engine.WhitePlayer.MovesNext ? Engine.WhitePlayer : Engine.BlackPlayer;
            King opposingKing = (Engine.WhitePlayer.MovesNext ? Engine.BlackPlayer : Engine.WhitePlayer).King;

            // Check if the piece that just moved attacks the king.
            List<Move> moves = move.Piece.GetLegalMoves().ToList();

            // Check sliding pieces for a newly opened attack on the king.
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (Engine.Board.Squares[i, j] != null && Engine.Board.Squares[i, j].IsSliding && Engine.Board.Squares[i, j].Player == player)
                    {
                        moves.AddRange(Engine.Board.Squares[i, j].GetLegalMoves());
                    }
                }
            }

            opposingKing.IsChecked = moves.Any(m => m.ToFile == opposingKing.CurrentFile && m.ToRank == opposingKing.CurrentRank);
        }

        private bool EnPassantOccurred(bool isWhitesTurn, Files firstFile, Files secondFile, Ranks firstRank, Ranks secondRank)
        {
            int secondFileIndex = (int)secondFile - 1;

            return Math.Abs((int)secondRank - (int)firstRank) == 2 && //Moved 2 spaces.
                    ((isWhitesTurn &&
                    (
                        (secondFileIndex - 1 >= 0 &&
                        Squares[secondFileIndex - 1, (int)Ranks.four] != null &&
                        Squares[secondFileIndex - 1, (int)Ranks.four].Player.Color == Colors.Black &&
                        Squares[secondFileIndex - 1, (int)Ranks.four].PieceType == PieceTypes.Pawn) || //Opposite pawn to the left.
                            (secondFileIndex + 1 <= 7 &&
                            Squares[secondFileIndex + 1, (int)Ranks.four] != null &&
                            Squares[secondFileIndex + 1, (int)Ranks.four].Player.Color == Colors.Black &&
                            Squares[secondFileIndex + 1, (int)Ranks.four].PieceType == PieceTypes.Pawn))) || //Opposite pawn to the right.
                    (!isWhitesTurn &&
                    (
                        (secondFileIndex - 1 >= 0 &&
                        Squares[secondFileIndex - 1, (int)Ranks.five] != null &&
                        Squares[secondFileIndex - 1, (int)Ranks.five].Player.Color == Colors.White &&
                        Squares[secondFileIndex - 1, (int)Ranks.five].PieceType == PieceTypes.Pawn) ||
                            (secondFileIndex + 1 <= 7 &&
                            Squares[secondFileIndex + 1, (int)Ranks.five] != null &&
                            Squares[secondFileIndex + 1, (int)Ranks.five].Player.Color == Colors.White &&
                            Squares[secondFileIndex + 1, (int)Ranks.five].PieceType == PieceTypes.Pawn))));
        }
    }
}
