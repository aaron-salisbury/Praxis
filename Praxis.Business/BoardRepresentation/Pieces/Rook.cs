using System;
using System.Collections.Generic;
using static Praxis.Base.Types;

namespace Praxis.Business.BoardRepresentation.Pieces
{
    internal class Rook : Piece
    {
        internal Rook(Player player, Files startingFile, Ranks startingRank) : base(player, startingFile, startingRank)
        {
            PieceType = PieceTypes.Rook;
            IsSliding = true;
            Figurine = Player.Color == Colors.White ? "♖" : "♜";
        }

        internal override List<Move> GetValidMoves()
        {
            return GetRookLikeValidMoves(this);
        }

        internal override ICollection<Move> GetLegalMoves()
        {
            return RemoveIllegalMoves(GetValidMoves());
        }

        internal override Piece ClonePiece()
        {
            return new Rook(Player, CurrentFile.Value, CurrentRank.Value);
        }

        internal static List<Move> GetRookLikeValidMoves(Piece piece)
        {
            List<Move> validMoves = new List<Move>();
            int currentFileIndex = (int)piece.CurrentFile - 1;
            int currentRankIndex = (int)piece.CurrentRank - 1;

            #region Loop Forward Along Rank
            for (int i = currentFileIndex + 1; i < 8; i++)
            {
                if (piece.Player.Engine.Board.Squares[i, currentRankIndex] == null)
                {
                    validMoves.Add(new Move()
                    {
                        Piece = piece,
                        FromFile = piece.CurrentFile.Value,
                        FromRank = piece.CurrentRank.Value,
                        ToFile = (Files)Enum.ToObject(typeof(Files), i + 1),
                        ToRank = piece.CurrentRank.Value
                    });
                }
                else if (piece.Player.Engine.Board.Squares[i, currentRankIndex].Player.Color != piece.Player.Color)
                {
                    validMoves.Add(new Move()
                    {
                        Piece = piece,
                        FromFile = piece.CurrentFile.Value,
                        FromRank = piece.CurrentRank.Value,
                        ToFile = (Files)Enum.ToObject(typeof(Files), i + 1),
                        ToRank = piece.CurrentRank.Value,
                        AttackedPiece = piece.Player.Engine.Board.Squares[i, currentRankIndex]
                    });

                    break;
                }
                else
                {
                    break;
                }
            }
            #endregion

            #region Loop Backward Along Rank
            for (int i = currentFileIndex - 1; i >= 0; i--)
            {
                if (piece.Player.Engine.Board.Squares[i, currentRankIndex] == null)
                {
                    validMoves.Add(new Move()
                    {
                        Piece = piece,
                        FromFile = piece.CurrentFile.Value,
                        FromRank = piece.CurrentRank.Value,
                        ToFile = (Files)Enum.ToObject(typeof(Files), i + 1),
                        ToRank = piece.CurrentRank.Value
                    });
                }
                else if (piece.Player.Engine.Board.Squares[i, currentRankIndex].Player.Color != piece.Player.Color)
                {
                    validMoves.Add(new Move()
                    {
                        Piece = piece,
                        FromFile = piece.CurrentFile.Value,
                        FromRank = piece.CurrentRank.Value,
                        ToFile = (Files)Enum.ToObject(typeof(Files), i + 1),
                        ToRank = piece.CurrentRank.Value,
                        AttackedPiece = piece.Player.Engine.Board.Squares[i, currentRankIndex]
                    });

                    break;
                }
                else
                {
                    break;
                }
            }
            #endregion

            #region Loop Up Along File
            for (int i = currentRankIndex + 1; i < 8; i++)
            {
                if (piece.Player.Engine.Board.Squares[currentFileIndex, i] == null)
                {
                    validMoves.Add(new Move()
                    {
                        Piece = piece,
                        FromFile = piece.CurrentFile.Value,
                        FromRank = piece.CurrentRank.Value,
                        ToFile = piece.CurrentFile.Value,
                        ToRank = (Ranks)Enum.ToObject(typeof(Ranks), i + 1)
                    });
                }
                else if (piece.Player.Engine.Board.Squares[currentFileIndex, i].Player.Color != piece.Player.Color)
                {
                    validMoves.Add(new Move()
                    {
                        Piece = piece,
                        FromFile = piece.CurrentFile.Value,
                        FromRank = piece.CurrentRank.Value,
                        ToFile = piece.CurrentFile.Value,
                        ToRank = (Ranks)Enum.ToObject(typeof(Ranks), i + 1),
                        AttackedPiece = piece.Player.Engine.Board.Squares[currentFileIndex, i]
                    });

                    break;
                }
                else
                {
                    break;
                }
            }
            #endregion

            #region Loop Down Along File
            for (int i = currentRankIndex - 1; i >= 0; i--)
            {
                if (piece.Player.Engine.Board.Squares[currentFileIndex, i] == null)
                {
                    validMoves.Add(new Move()
                    {
                        Piece = piece,
                        FromFile = piece.CurrentFile.Value,
                        FromRank = piece.CurrentRank.Value,
                        ToFile = piece.CurrentFile.Value,
                        ToRank = (Ranks)Enum.ToObject(typeof(Ranks), i + 1)
                    });
                }
                else if (piece.Player.Engine.Board.Squares[currentFileIndex, i].Player.Color != piece.Player.Color)
                {
                    validMoves.Add(new Move()
                    {
                        Piece = piece,
                        FromFile = piece.CurrentFile.Value,
                        FromRank = piece.CurrentRank.Value,
                        ToFile = piece.CurrentFile.Value,
                        ToRank = (Ranks)Enum.ToObject(typeof(Ranks), i + 1),
                        AttackedPiece = piece.Player.Engine.Board.Squares[currentFileIndex, i]
                    });

                    break;
                }
                else
                {
                    break;
                }
            }
            #endregion

            return validMoves;
        }
    }
}