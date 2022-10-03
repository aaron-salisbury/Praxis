using System;
using System.Collections.Generic;
using static Praxis.Engine.Win98.Application.Types;

namespace Praxis.Engine.Win98.Application.BoardRepresentation.Pieces
{
    internal class Knight : Piece
    {
        internal Knight(Player player, Files startingFile, Ranks startingRank) : base(player, startingFile, startingRank)
        {
            PieceType = PieceTypes.Knight;
            Figurine = Player.Color == Colors.White ? "♘" : "♞";
        }

        internal override List<Move> GetValidMoves()
        {
            List<Move> validMoves = new List<Move>();
            int currentFileIndex = (int)CurrentFile - 1;
            int currentRankIndex = (int)CurrentRank - 1;

            int horizontalTopLeftFileIndex = Player.Color == Colors.White ? currentFileIndex - 2 : currentFileIndex + 2;
            int horizontalTopLeftRankIndex = Player.Color == Colors.White ? currentRankIndex + 1 : currentRankIndex - 1;
            ProcessPotentialKnightMove(horizontalTopLeftFileIndex, horizontalTopLeftRankIndex, validMoves);

            int verticalTopLeftFileIndex = Player.Color == Colors.White ? currentFileIndex - 1 : currentFileIndex + 1;
            int verticalTopLeftRankIndex = Player.Color == Colors.White ? currentRankIndex + 2 : currentRankIndex - 2;
            ProcessPotentialKnightMove(verticalTopLeftFileIndex, verticalTopLeftRankIndex, validMoves);

            int verticalTopRightFileIndex = Player.Color == Colors.White ? currentFileIndex + 1 : currentFileIndex - 1;
            int verticalTopRightRankIndex = Player.Color == Colors.White ? currentRankIndex + 2 : currentRankIndex - 2;
            ProcessPotentialKnightMove(verticalTopRightFileIndex, verticalTopRightRankIndex, validMoves);

            int horizontalTopRightFileIndex = Player.Color == Colors.White ? currentFileIndex + 2 : currentFileIndex - 2;
            int horizontalTopRightRankIndex = Player.Color == Colors.White ? currentRankIndex + 1 : currentRankIndex - 1;
            ProcessPotentialKnightMove(horizontalTopRightFileIndex, horizontalTopRightRankIndex, validMoves);

            int horizontalBottomRightFileIndex = Player.Color == Colors.White ? currentFileIndex + 2 : currentFileIndex - 2;
            int horizontalBottomRightRankIndex = Player.Color == Colors.White ? currentRankIndex - 1 : currentRankIndex + 1;
            ProcessPotentialKnightMove(horizontalBottomRightFileIndex, horizontalBottomRightRankIndex, validMoves);

            int verticalBottomRightFileIndex = Player.Color == Colors.White ? currentFileIndex + 1 : currentFileIndex - 1;
            int verticalBottomRightRankIndex = Player.Color == Colors.White ? currentRankIndex - 2 : currentRankIndex + 2;
            ProcessPotentialKnightMove(verticalBottomRightFileIndex, verticalBottomRightRankIndex, validMoves);

            int verticalBottomLeftFileIndex = Player.Color == Colors.White ? currentFileIndex - 1 : currentFileIndex + 1;
            int verticalBottomLeftRankIndex = Player.Color == Colors.White ? currentRankIndex - 2 : currentRankIndex + 2;
            ProcessPotentialKnightMove(verticalBottomLeftFileIndex, verticalBottomLeftRankIndex, validMoves);

            int horizontalBottomLeftFileIndex = Player.Color == Colors.White ? currentFileIndex - 2 : currentFileIndex + 2;
            int horizontalBottomLeftRankIndex = Player.Color == Colors.White ? currentRankIndex - 1 : currentRankIndex + 1;
            ProcessPotentialKnightMove(horizontalBottomLeftFileIndex, horizontalBottomLeftRankIndex, validMoves);

            return validMoves;
        }

        internal override ICollection<Move> GetLegalMoves()
        {
            return RemoveIllegalMoves(GetValidMoves());
        }

        internal override Piece ClonePiece()
        {
            return new Knight(Player, CurrentFile.Value, CurrentRank.Value);
        }

        private void ProcessPotentialKnightMove(int fileIndex, int rankIndex, List<Move> validMoves)
        {
            if (fileIndex >= 0 && fileIndex < 8 && rankIndex >= 0 && rankIndex < 8 &&
                    (Player.Engine.Board.Squares[fileIndex, rankIndex] == null ||
                        Player.Engine.Board.Squares[fileIndex, rankIndex].Player.Color != Player.Color))
            {
                validMoves.Add(new Move()
                {
                    Piece = this,
                    FromFile = CurrentFile.Value,
                    FromRank = CurrentRank.Value,
                    ToFile = (Files)Enum.ToObject(typeof(Files), fileIndex + 1),
                    ToRank = (Ranks)Enum.ToObject(typeof(Ranks), rankIndex + 1),
                    AttackedPiece = Player.Engine.Board.Squares[fileIndex, rankIndex]
                });
            }
        }
    }
}