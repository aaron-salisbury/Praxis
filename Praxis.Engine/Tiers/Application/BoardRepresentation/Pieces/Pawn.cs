using System.Collections.Generic;
using static Praxis.Engine.Tiers.Application.Types;

namespace Praxis.Engine.Tiers.Application.BoardRepresentation.Pieces
{
    internal class Pawn : Piece
    {
        internal Pawn(Player player, Files startingFile, Ranks startingRank) : base(player, startingFile, startingRank)
        {
            PieceType = PieceTypes.Pawn;
            Figurine = Player.Color == Colors.White ? "♙" : "♟";
        }

        internal override List<Move> GetValidMoves()
        {
            List<Move> validMoves = new List<Move>();

            #region If White
            if (Player.Color == Colors.White)
            {
                if (CurrentRank < Ranks.eight)
                {
                    // Move forward one.
                    if (Player.Engine.Board.Squares[(int)CurrentFile - 1, (int)CurrentRank] == null)
                    {
                        Move move = new Move()
                        {
                            Piece = this,
                            FromFile = CurrentFile.Value,
                            FromRank = CurrentRank.Value,
                            ToFile = CurrentFile.Value,
                            ToRank = CurrentRank.Value + 1
                        };

                        if(move.ToRank == Ranks.eight)
                        {
                            move.PromotionPieceType = PieceTypes.Queen; //TODO: For now assuming promotion to queen.
                        }

                        validMoves.Add(move);
                    }

                    // Attack left.
                    if ((int)CurrentFile - 1 >= 1 && 
                        Player.Engine.Board.Squares[(int)CurrentFile - 2, (int)CurrentRank] != null &&
                        Player.Engine.Board.Squares[(int)CurrentFile - 2, (int)CurrentRank].Player.Color != Player.Color)
                    {
                        Move move = new Move()
                        {
                            Piece = this,
                            FromFile = CurrentFile.Value,
                            FromRank = CurrentRank.Value,
                            ToFile = CurrentFile.Value - 1,
                            ToRank = CurrentRank.Value + 1,
                            AttackedPiece = Player.Engine.Board.Squares[(int)CurrentFile - 2, (int)CurrentRank]
                        };

                        if (move.ToRank == Ranks.eight)
                        {
                            move.PromotionPieceType = PieceTypes.Queen; //TODO: For now assuming promotion to queen.
                        }

                        validMoves.Add(move);
                    }

                    // Attack right.
                    if ((int)CurrentFile + 1 <= 8 &&
                        Player.Engine.Board.Squares[(int)CurrentFile, (int)CurrentRank] != null &&
                        Player.Engine.Board.Squares[(int)CurrentFile, (int)CurrentRank].Player.Color != Player.Color)
                    {
                        Move move = new Move()
                        {
                            Piece = this,
                            FromFile = CurrentFile.Value,
                            FromRank = CurrentRank.Value,
                            ToFile = CurrentFile.Value + 1,
                            ToRank = CurrentRank.Value + 1,
                            AttackedPiece = Player.Engine.Board.Squares[(int)CurrentFile, (int)CurrentRank]
                        };

                        if (move.ToRank == Ranks.eight)
                        {
                            move.PromotionPieceType = PieceTypes.Queen; //TODO: For now assuming promotion to queen.
                        }

                        validMoves.Add(move);
                    }
                }

                // Move forward two.
                if (CurrentRank == Ranks.two && 
                    Player.Engine.Board.Squares[(int)CurrentFile - 1, (int)CurrentRank] == null && 
                    Player.Engine.Board.Squares[(int)CurrentFile - 1, (int)CurrentRank + 1] == null)
                {
                    validMoves.Add(new Move()
                    {
                        Piece = this,
                        FromFile = CurrentFile.Value,
                        FromRank = CurrentRank.Value,
                        ToFile = CurrentFile.Value,
                        ToRank = CurrentRank.Value + 2
                    });
                }

                // En passant.
                if (Player.Engine.EnPassantFile != null && 
                    Player.Engine.EnPassantRank != null && 
                    CurrentRank + 1 == Player.Engine.EnPassantRank && 
                    (CurrentFile - 1 == Player.Engine.EnPassantFile || CurrentFile + 1 == Player.Engine.EnPassantFile))
                {
                    validMoves.Add(new Move()
                    {
                        Piece = this,
                        FromFile = CurrentFile.Value,
                        FromRank = CurrentRank.Value,
                        ToFile = Player.Engine.EnPassantFile.Value,
                        ToRank = Player.Engine.EnPassantRank.Value
                    });
                }
            }
            #endregion

            #region If Black
            else
            {
                if (CurrentRank > Ranks.one)
                {
                    // Move forward one.
                    if (Player.Engine.Board.Squares[(int)CurrentFile - 1, (int)CurrentRank - 2] == null)
                    {
                        Move move = new Move()
                        {
                            Piece = this,
                            FromFile = CurrentFile.Value,
                            FromRank = CurrentRank.Value,
                            ToFile = CurrentFile.Value,
                            ToRank = CurrentRank.Value - 1
                        };

                        if (move.ToRank == Ranks.one)
                        {
                            move.PromotionPieceType = PieceTypes.Queen; //TODO: For now assuming promotion to queen.
                        }

                        validMoves.Add(move);
                    }

                    // Attack right.
                    if ((int)CurrentFile - 1 >= 1 &&
                        Player.Engine.Board.Squares[(int)CurrentFile - 2, (int)CurrentRank - 2] != null &&
                        Player.Engine.Board.Squares[(int)CurrentFile - 2, (int)CurrentRank - 2].Player.Color != Player.Color)
                    {
                        Move move = new Move()
                        {
                            Piece = this,
                            FromFile = CurrentFile.Value,
                            FromRank = CurrentRank.Value,
                            ToFile = CurrentFile.Value - 1,
                            ToRank = CurrentRank.Value - 1,
                            AttackedPiece = Player.Engine.Board.Squares[(int)CurrentFile - 2, (int)CurrentRank - 2]
                        };

                        if (move.ToRank == Ranks.one)
                        {
                            move.PromotionPieceType = PieceTypes.Queen; //TODO: For now assuming promotion to queen.
                        }

                        validMoves.Add(move);
                    }

                    // Attack left.
                    if ((int)CurrentFile + 1 <= 8 &&
                        Player.Engine.Board.Squares[(int)CurrentFile, (int)CurrentRank - 2] != null &&
                        Player.Engine.Board.Squares[(int)CurrentFile, (int)CurrentRank - 2].Player.Color != Player.Color)
                    {
                        Move move = new Move()
                        {
                            Piece = this,
                            FromFile = CurrentFile.Value,
                            FromRank = CurrentRank.Value,
                            ToFile = CurrentFile.Value + 1,
                            ToRank = CurrentRank.Value - 1,
                            AttackedPiece = Player.Engine.Board.Squares[(int)CurrentFile, (int)CurrentRank - 2]
                        };

                        if (move.ToRank == Ranks.one)
                        {
                            move.PromotionPieceType = PieceTypes.Queen; //TODO: For now assuming promotion to queen.
                        }

                        validMoves.Add(move);
                    }
                }

                // Move forward two.
                if (CurrentRank == Ranks.seven && 
                    Player.Engine.Board.Squares[(int)CurrentFile - 1, (int)CurrentRank - 2] == null && 
                    Player.Engine.Board.Squares[(int)CurrentFile - 1, (int)CurrentRank - 3] == null)
                {
                    validMoves.Add(new Move()
                    {
                        Piece = this,
                        FromFile = CurrentFile.Value,
                        FromRank = CurrentRank.Value,
                        ToFile = CurrentFile.Value,
                        ToRank = CurrentRank.Value - 2
                    });
                }

                // En passant.
                if (Player.Engine.EnPassantFile != null &&
                    Player.Engine.EnPassantRank != null &&
                    CurrentRank - 1 == Player.Engine.EnPassantRank &&
                    (CurrentFile - 1 == Player.Engine.EnPassantFile || CurrentFile + 1 == Player.Engine.EnPassantFile))
                {
                    validMoves.Add(new Move()
                    {
                        Piece = this,
                        FromFile = CurrentFile.Value,
                        FromRank = CurrentRank.Value,
                        ToFile = Player.Engine.EnPassantFile.Value,
                        ToRank = Player.Engine.EnPassantRank.Value
                    });
                }
            }
            #endregion

            return validMoves;
        }

        internal override ICollection<Move> GetLegalMoves()
        {
            return RemoveIllegalMoves(GetValidMoves());
        }

        internal override Piece ClonePiece()
        {
            return new Pawn(Player, CurrentFile.Value, CurrentRank.Value);
        }
    }
}