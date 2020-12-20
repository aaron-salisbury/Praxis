using System;
using System.Collections.Generic;
using static Praxis.Engine.Tiers.Application.Types;

namespace Praxis.Engine.Tiers.Application.BoardRepresentation.Pieces
{
    internal class King : Piece
    {
        internal bool IsChecked { get; set; }

        internal bool IsCheckMated
        {
            get
            {
                if (!IsChecked)
                {
                    return false;
                }
                else
                {
                    return false; //TODO: Add operation to figure this out.
                }
            } 
        }

        internal King(Player player, Files startingFile, Ranks startingRank) : base(player, startingFile, startingRank)
        {
            PieceType = PieceTypes.King;
            Figurine = Player.Color == Colors.White ? "♔" : "♚";
        }

        internal override List<Move> GetValidMoves()
        {
            List<Move> validMoves = new List<Move>();
            int currentFileIndex = (int)CurrentFile - 1;
            int currentRankIndex = (int)CurrentRank - 1;
            int leftFileIndex = Player.Engine.WhitePlayer.MovesNext ? currentFileIndex - 1 : currentFileIndex + 1;
            int rightFileIndex = Player.Engine.WhitePlayer.MovesNext ? currentFileIndex + 1 : currentFileIndex - 1;

            #region Forward & Diagonal
            int forwardRankIndex = Player.Engine.WhitePlayer.MovesNext ? currentRankIndex + 1 : currentRankIndex - 1;

            if (forwardRankIndex < 8 && forwardRankIndex >= 0)
            {
                // Check forward
                if (Player.Engine.Board.Squares[currentFileIndex, forwardRankIndex] == null || Player.Engine.Board.Squares[currentFileIndex, forwardRankIndex].Player.Color != Player.Color)
                {
                    validMoves.Add(new Move()
                    {
                        Piece = this,
                        FromFile = CurrentFile.Value,
                        FromRank = CurrentRank.Value,
                        ToFile = CurrentFile.Value,
                        ToRank = (Ranks)Enum.ToObject(typeof(Ranks), forwardRankIndex + 1),
                        AttackedPiece = Player.Engine.Board.Squares[currentFileIndex, forwardRankIndex]
                    });
                }

                // Check diagonal forward left.
                if (leftFileIndex >= 0 && leftFileIndex < 8 && 
                        (Player.Engine.Board.Squares[leftFileIndex, forwardRankIndex] == null || 
                        Player.Engine.Board.Squares[leftFileIndex, forwardRankIndex].Player.Color != Player.Color))
                {
                    validMoves.Add(new Move()
                    {
                        Piece = this,
                        FromFile = CurrentFile.Value,
                        FromRank = CurrentRank.Value,
                        ToFile = (Files)Enum.ToObject(typeof(Files), leftFileIndex + 1),
                        ToRank = (Ranks)Enum.ToObject(typeof(Ranks), forwardRankIndex + 1),
                        AttackedPiece = Player.Engine.Board.Squares[leftFileIndex, forwardRankIndex]
                    });
                }

                // Check diagonal forward right.
                if (rightFileIndex >= 0 && rightFileIndex < 8 &&
                        (Player.Engine.Board.Squares[rightFileIndex, forwardRankIndex] == null ||
                        Player.Engine.Board.Squares[rightFileIndex, forwardRankIndex].Player.Color != Player.Color))
                {
                    validMoves.Add(new Move()
                    {
                        Piece = this,
                        FromFile = CurrentFile.Value,
                        FromRank = CurrentRank.Value,
                        ToFile = (Files)Enum.ToObject(typeof(Files), rightFileIndex + 1),
                        ToRank = (Ranks)Enum.ToObject(typeof(Ranks), forwardRankIndex + 1),
                        AttackedPiece = Player.Engine.Board.Squares[rightFileIndex, forwardRankIndex]
                    });
                }
            }
            #endregion

            #region Backward & Diagonal
            int bacwardRankIndex = Player.Engine.WhitePlayer.MovesNext ? currentRankIndex - 1 : currentRankIndex + 1;

            if (bacwardRankIndex < 8 && bacwardRankIndex >= 0)
            {
                // Check bacward.
                if (Player.Engine.Board.Squares[currentFileIndex, bacwardRankIndex] == null || Player.Engine.Board.Squares[currentFileIndex, bacwardRankIndex].Player.Color != Player.Color)
                {
                    validMoves.Add(new Move()
                    {
                        Piece = this,
                        FromFile = CurrentFile.Value,
                        FromRank = CurrentRank.Value,
                        ToFile = CurrentFile.Value,
                        ToRank = (Ranks)Enum.ToObject(typeof(Ranks), bacwardRankIndex + 1),
                        AttackedPiece = Player.Engine.Board.Squares[currentFileIndex, bacwardRankIndex]
                    });
                }

                // Check diagonal bacward left.
                if (leftFileIndex >= 0 && leftFileIndex < 8 &&
                        (Player.Engine.Board.Squares[leftFileIndex, bacwardRankIndex] == null ||
                        Player.Engine.Board.Squares[leftFileIndex, bacwardRankIndex].Player.Color != Player.Color))
                {
                    validMoves.Add(new Move()
                    {
                        Piece = this,
                        FromFile = CurrentFile.Value,
                        FromRank = CurrentRank.Value,
                        ToFile = (Files)Enum.ToObject(typeof(Files), leftFileIndex + 1),
                        ToRank = (Ranks)Enum.ToObject(typeof(Ranks), bacwardRankIndex + 1),
                        AttackedPiece = Player.Engine.Board.Squares[leftFileIndex, bacwardRankIndex]
                    });
                }

                // Check diagonal bacward right.
                if (rightFileIndex >= 0 && rightFileIndex < 8 &&
                        (Player.Engine.Board.Squares[rightFileIndex, bacwardRankIndex] == null ||
                        Player.Engine.Board.Squares[rightFileIndex, bacwardRankIndex].Player.Color != Player.Color))
                {
                    validMoves.Add(new Move()
                    {
                        Piece = this,
                        FromFile = CurrentFile.Value,
                        FromRank = CurrentRank.Value,
                        ToFile = (Files)Enum.ToObject(typeof(Files), rightFileIndex + 1),
                        ToRank = (Ranks)Enum.ToObject(typeof(Ranks), bacwardRankIndex + 1),
                        AttackedPiece = Player.Engine.Board.Squares[rightFileIndex, bacwardRankIndex]
                    });
                }
            }
            #endregion

            #region Left
            if (leftFileIndex >= 0 && leftFileIndex < 8 && 
                    (Player.Engine.Board.Squares[leftFileIndex, currentRankIndex] == null || Player.Engine.Board.Squares[leftFileIndex, currentRankIndex].Player.Color != Player.Color))
            {
                validMoves.Add(new Move()
                {
                    Piece = this,
                    FromFile = CurrentFile.Value,
                    FromRank = CurrentRank.Value,
                    ToFile = (Files)Enum.ToObject(typeof(Files), leftFileIndex + 1),
                    ToRank = CurrentRank.Value,
                    AttackedPiece = Player.Engine.Board.Squares[leftFileIndex, currentRankIndex]
                });
            }
            #endregion

            #region Right
            if (rightFileIndex >= 0 && rightFileIndex < 8 && 
                    (Player.Engine.Board.Squares[rightFileIndex, currentRankIndex] == null || Player.Engine.Board.Squares[rightFileIndex, currentRankIndex].Player.Color != Player.Color))
            {
                validMoves.Add(new Move()
                {
                    Piece = this,
                    FromFile = CurrentFile.Value,
                    FromRank = CurrentRank.Value,
                    ToFile = (Files)Enum.ToObject(typeof(Files), rightFileIndex + 1),
                    ToRank = CurrentRank.Value,
                    AttackedPiece = Player.Engine.Board.Squares[rightFileIndex, currentRankIndex]
                });
            }
            #endregion

            // TODO: Cannot castle if any square in between could be attacked.
            #region Castling
            // Check castle queenside.
            int queensideOneFileIndex = currentFileIndex - 1;
            int queensideTwoFileIndex = currentFileIndex - 2;
            int queensideThreeFileIndex = currentFileIndex - 3;

            if (Player.CanCastleQueenside &&
                Player.Engine.Board.Squares[queensideOneFileIndex, currentRankIndex] == null &&
                Player.Engine.Board.Squares[queensideTwoFileIndex, currentRankIndex] == null &&
                Player.Engine.Board.Squares[queensideThreeFileIndex, currentRankIndex] == null)
            {
                validMoves.Add(new Move()
                {
                    Piece = this,
                    FromFile = CurrentFile.Value,
                    FromRank = CurrentRank.Value,
                    ToFile = (Files)Enum.ToObject(typeof(Files), queensideTwoFileIndex + 1),
                    ToRank = CurrentRank.Value
                });
            }

            // Check castle kingside.
            int kingsideOneFileIndex = currentFileIndex + 1;
            int kingsideTwoFileIndex = currentFileIndex + 2;

            if (Player.CanCastleKingside &&
                Player.Engine.Board.Squares[kingsideOneFileIndex, currentRankIndex] == null &&
                Player.Engine.Board.Squares[kingsideTwoFileIndex, currentRankIndex] == null)
            {
                validMoves.Add(new Move()
                {
                    Piece = this,
                    FromFile = CurrentFile.Value,
                    FromRank = CurrentRank.Value,
                    ToFile = (Files)Enum.ToObject(typeof(Files), kingsideTwoFileIndex + 1),
                    ToRank = CurrentRank.Value
                });
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
            return new King(Player, CurrentFile.Value, CurrentRank.Value);
        }
    }
}