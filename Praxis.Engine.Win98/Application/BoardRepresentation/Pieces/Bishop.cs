using System;
using System.Collections.Generic;
using static Praxis.Engine.Win98.Application.Types;

namespace Praxis.Engine.Win98.Application.BoardRepresentation.Pieces
{
    internal class Bishop : Piece
    {
        internal Bishop(Player player, Files startingFile, Ranks startingRank) : base(player, startingFile, startingRank)
        {
            PieceType = PieceTypes.Bishop;
            IsSliding = true;
            Figurine = Player.Color == Colors.White ? "♗" : "♝";
        }

        internal override List<Move> GetValidMoves()
        {
            return GetBishopLikeValidMoves(this);
        }

        internal override ICollection<Move> GetLegalMoves()
        {
            return RemoveIllegalMoves(GetValidMoves());
        }

        internal override Piece ClonePiece()
        {
            return new Bishop(Player, CurrentFile.Value, CurrentRank.Value);
        }

        internal static List<Move> GetBishopLikeValidMoves(Piece piece)
        {
            List<Move> validMoves = new List<Move>();
            int currentFileIndex = (int)piece.CurrentFile - 1;
            int currentRankIndex = (int)piece.CurrentRank - 1;

            #region Diagonal Top Left
            int leftFileIndex = piece.Player.Color == Colors.White ? currentFileIndex - 1 : currentFileIndex + 1;
            int upRankIndex = piece.Player.Color == Colors.White ? currentRankIndex + 1 : currentRankIndex - 1;

            while (leftFileIndex >= 0 && leftFileIndex < 8 && upRankIndex >= 0 && upRankIndex < 8)
            {
                if (piece.Player.Engine.Board.Squares[leftFileIndex, upRankIndex] == null)
                {
                    validMoves.Add(new Move()
                    {
                        Piece = piece,
                        FromFile = piece.CurrentFile.Value,
                        FromRank = piece.CurrentRank.Value,
                        ToFile = (Files)Enum.ToObject(typeof(Files), leftFileIndex + 1),
                        ToRank = (Ranks)Enum.ToObject(typeof(Ranks), upRankIndex + 1)
                    });

                    leftFileIndex = piece.Player.Color == Colors.White ? leftFileIndex - 1 : leftFileIndex + 1;
                    upRankIndex = piece.Player.Color == Colors.White ? upRankIndex + 1 : upRankIndex - 1;
                }
                else
                {
                    if (piece.Player.Engine.Board.Squares[leftFileIndex, upRankIndex].Player.Color != piece.Player.Color)
                    {
                        validMoves.Add(new Move()
                        {
                            Piece = piece,
                            FromFile = piece.CurrentFile.Value,
                            FromRank = piece.CurrentRank.Value,
                            ToFile = (Files)Enum.ToObject(typeof(Files), leftFileIndex + 1),
                            ToRank = (Ranks)Enum.ToObject(typeof(Ranks), upRankIndex + 1),
                            AttackedPiece = piece.Player.Engine.Board.Squares[leftFileIndex, upRankIndex]
                        });
                    }

                    break;
                }
            }
            #endregion

            #region Diagonal Top Right
            int rightFileIndex = piece.Player.Color == Colors.White ? currentFileIndex + 1 : currentFileIndex - 1;
            upRankIndex = piece.Player.Color == Colors.White ? currentRankIndex + 1 : currentRankIndex - 1;

            while (rightFileIndex >= 0 && rightFileIndex < 8 && upRankIndex >= 0 && upRankIndex < 8)
            {
                if (piece.Player.Engine.Board.Squares[rightFileIndex, upRankIndex] == null)
                {
                    validMoves.Add(new Move()
                    {
                        Piece = piece,
                        FromFile = piece.CurrentFile.Value,
                        FromRank = piece.CurrentRank.Value,
                        ToFile = (Files)Enum.ToObject(typeof(Files), rightFileIndex + 1),
                        ToRank = (Ranks)Enum.ToObject(typeof(Ranks), upRankIndex + 1)
                    });

                    rightFileIndex = piece.Player.Color == Colors.White ? rightFileIndex + 1 : rightFileIndex - 1;
                    upRankIndex = piece.Player.Color == Colors.White ? upRankIndex + 1 : upRankIndex - 1;
                }
                else
                {
                    if (piece.Player.Engine.Board.Squares[rightFileIndex, upRankIndex].Player.Color != piece.Player.Color)
                    {
                        validMoves.Add(new Move()
                        {
                            Piece = piece,
                            FromFile = piece.CurrentFile.Value,
                            FromRank = piece.CurrentRank.Value,
                            ToFile = (Files)Enum.ToObject(typeof(Files), rightFileIndex + 1),
                            ToRank = (Ranks)Enum.ToObject(typeof(Ranks), upRankIndex + 1),
                            AttackedPiece = piece.Player.Engine.Board.Squares[rightFileIndex, upRankIndex]
                        });
                    }

                    break;
                }
            }
            #endregion

            #region Diagonal Bottom Left
            leftFileIndex = piece.Player.Color == Colors.White ? currentFileIndex - 1 : currentFileIndex + 1;
            int downRankIndex = piece.Player.Color == Colors.White ? currentRankIndex - 1 : currentRankIndex + 1;

            while (leftFileIndex >= 0 && leftFileIndex < 8 && downRankIndex >= 0 && downRankIndex < 8)
            {
                if (piece.Player.Engine.Board.Squares[leftFileIndex, downRankIndex] == null)
                {
                    validMoves.Add(new Move()
                    {
                        Piece = piece,
                        FromFile = piece.CurrentFile.Value,
                        FromRank = piece.CurrentRank.Value,
                        ToFile = (Files)Enum.ToObject(typeof(Files), leftFileIndex + 1),
                        ToRank = (Ranks)Enum.ToObject(typeof(Ranks), downRankIndex + 1)
                    });

                    leftFileIndex = piece.Player.Color == Colors.White ? leftFileIndex - 1 : leftFileIndex + 1;
                    downRankIndex = piece.Player.Color == Colors.White ? downRankIndex - 1 : downRankIndex + 1;
                }
                else
                {
                    if (piece.Player.Engine.Board.Squares[leftFileIndex, downRankIndex].Player.Color != piece.Player.Color)
                    {
                        validMoves.Add(new Move()
                        {
                            Piece = piece,
                            FromFile = piece.CurrentFile.Value,
                            FromRank = piece.CurrentRank.Value,
                            ToFile = (Files)Enum.ToObject(typeof(Files), leftFileIndex + 1),
                            ToRank = (Ranks)Enum.ToObject(typeof(Ranks), downRankIndex + 1),
                            AttackedPiece = piece.Player.Engine.Board.Squares[leftFileIndex, downRankIndex]
                        });
                    }

                    break;
                }
            }
            #endregion

            #region Diagonal Bottom Right
            rightFileIndex = piece.Player.Color == Colors.White ? currentFileIndex + 1 : currentFileIndex - 1;
            downRankIndex = piece.Player.Color == Colors.White ? currentRankIndex - 1 : currentRankIndex + 1;

            while (rightFileIndex >= 0 && rightFileIndex < 8 && downRankIndex >= 0 && downRankIndex < 8)
            {
                if (piece.Player.Engine.Board.Squares[rightFileIndex, downRankIndex] == null)
                {
                    validMoves.Add(new Move()
                    {
                        Piece = piece,
                        FromFile = piece.CurrentFile.Value,
                        FromRank = piece.CurrentRank.Value,
                        ToFile = (Files)Enum.ToObject(typeof(Files), rightFileIndex + 1),
                        ToRank = (Ranks)Enum.ToObject(typeof(Ranks), downRankIndex + 1)
                    });

                    rightFileIndex = piece.Player.Color == Colors.White ? rightFileIndex + 1 : rightFileIndex - 1;
                    downRankIndex = piece.Player.Color == Colors.White ? downRankIndex - 1 : downRankIndex + 1;
                }
                else
                {
                    if (piece.Player.Engine.Board.Squares[rightFileIndex, downRankIndex].Player.Color != piece.Player.Color)
                    {
                        validMoves.Add(new Move()
                        {
                            Piece = piece,
                            FromFile = piece.CurrentFile.Value,
                            FromRank = piece.CurrentRank.Value,
                            ToFile = (Files)Enum.ToObject(typeof(Files), rightFileIndex + 1),
                            ToRank = (Ranks)Enum.ToObject(typeof(Ranks), downRankIndex + 1),
                            AttackedPiece = piece.Player.Engine.Board.Squares[rightFileIndex, downRankIndex]
                        });
                    }

                    break;
                }
            }
            #endregion

            return validMoves;
        }
    }
}