using Praxis.Engine.Application.BoardRepresentation.Pieces;
using System;
using System.Collections.Generic;
using System.Linq;
using static Praxis.Engine.Application.Types;

namespace Praxis.Engine.Application.BoardRepresentation
{
    internal class Move
    {
        internal Piece Piece { get; set; }
        internal Files FromFile { get; set; }
        internal Ranks FromRank { get; set; }
        internal Files ToFile { get; set; }
        internal Ranks ToRank { get; set; }
        internal PieceTypes? PromotionPieceType { get; set; }
        internal bool IsKingsideCastle { get; set; }
        internal bool IsQueensideCastle { get; set; }
        internal Piece AttackedPiece { get; set; }

        internal static Dictionary<string, PieceTypes> PieceTypesByLetters = new Dictionary<string, PieceTypes>()
        {
            { "k", PieceTypes.King },
            { "q", PieceTypes.Queen },
            { "r", PieceTypes.Rook },
            { "b", PieceTypes.Bishop },
            { "n", PieceTypes.Knight },
            { "p", PieceTypes.Pawn }
        };

        internal string ConvertToAlgebraic()
        {
            string algebraicMove = string.Format("{0}{1}{2}{3}", FromFile.ToString(), (int)FromRank, ToFile.ToString(), (int)ToRank);

            if (PromotionPieceType != null)
            {
                algebraicMove += PieceTypesByLetters.Where(kvp => kvp.Value == PromotionPieceType).Select(kvp => kvp.Key).FirstOrDefault();
            }

            return algebraicMove;
        }

        internal void AddToRunningSAN()
        {
            // If its a Rook and part of a castle, don't update SAN. Its implied with the King's move.
            if ((!IsKingsideCastle && !IsQueensideCastle) || Piece.PieceType != PieceTypes.Rook)
            {
                string san = string.Empty;

                if (!IsKingsideCastle && !IsQueensideCastle)
                {
                    if (Piece.PieceType != PieceTypes.Pawn)
                    {
                        string movingPiece = PieceTypesByLetters.Where(kvp => kvp.Value == Piece.PieceType).Select(kvp => kvp.Key).FirstOrDefault().ToUpper();
                        san = movingPiece;
                    }

                    if (Piece.Player.Engine.Board.Squares[(int)ToFile - 1, (int)ToRank - 1] != null)
                    {
                        if (Piece.PieceType == PieceTypes.Pawn)
                        {
                            san += FromFile.ToString();
                        }

                        san += "x";
                    }

                    string destination = string.Format("{0}{1}", ToFile.ToString(), (int)ToRank);
                    san += destination;

                    if (PromotionPieceType != null)
                    {
                        string promotionPiece = PieceTypesByLetters.Where(kvp => kvp.Value == PromotionPieceType).Select(kvp => kvp.Key).FirstOrDefault().ToUpper();
                        san += promotionPiece;
                    }
                }
                else
                {
                    // FIDE uses the digit zero while PGN requires, and the ECO uses, the uppercase letter O.
                    san = "O-O";

                    if (IsQueensideCastle)
                    {
                        san += "-O";
                    }
                }

                // Add space after previous move.
                if (Piece.Player.Engine.FullmoveCounter != 1 || Piece.Player.Color == Colors.Black)
                {
                    Piece.Player.Engine.SAN += " ";
                }

                // Add turn number.
                if (Piece.Player.Color == Colors.White)
                {
                    Piece.Player.Engine.SAN += Piece.Player.Engine.FullmoveCounter + ".";
                }

                // Add this player's move for the turn.
                Piece.Player.Engine.SAN += san;
            }
        }

        internal static Move ConvertAlgebraicToMove(string moveNotation, Engine engine)
        {
            if (moveNotation.StartsWith("O"))
            {
                return GetCastlingMove(moveNotation, engine);
            }

            moveNotation = moveNotation.Replace("x", string.Empty);

            Files firstFile = (Files)Enum.Parse(typeof(Files), moveNotation[0].ToString(), true);
            Ranks firstRank = (Ranks)Convert.ToInt32(moveNotation[1].ToString());
            Files secondFile = (Files)Enum.Parse(typeof(Files), moveNotation[2].ToString(), true);
            Ranks secondRank = (Ranks)Convert.ToInt32(moveNotation[3].ToString());
            PieceTypes? promotionType = null;

            if (moveNotation.Length == 5)
            {
                promotionType = PieceTypesByLetters[moveNotation[moveNotation.Length - 1].ToString().ToLower()];
            }

            return new Move()
            {
                Piece = engine.Board.Squares[(int)firstFile - 1, (int)firstRank - 1],
                FromFile = firstFile,
                FromRank = firstRank,
                ToFile = secondFile,
                ToRank = secondRank,
                PromotionPieceType = promotionType
            };
        }

        internal static Move GetCastlingMove(string moveNotation, Engine engine)
        {
            Files fromFile = Files.e;
            Ranks fromRank = engine.WhitePlayer.MovesNext ? Ranks.one : Ranks.eight;

            return new Move()
            {
                Piece = engine.Board.Squares[(int)fromFile - 1, (int)fromRank - 1],
                FromFile = fromFile,
                FromRank = fromRank,
                ToFile = moveNotation.Equals("O-O") ? Files.f : Files.d,
                ToRank = engine.WhitePlayer.MovesNext ? Ranks.one : Ranks.eight,
                IsKingsideCastle = moveNotation.Equals("O-O"),
                IsQueensideCastle = moveNotation.Equals("O-O-O")
            };
        }
    }
}
