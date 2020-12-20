using System.Collections.Generic;
using static Praxis.Engine.Tiers.Application.Types;

namespace Praxis.Engine.Tiers.Application.BoardRepresentation.Pieces
{
    internal class Queen : Piece
    {
        internal Queen(Player player, Files startingFile, Ranks startingRank) : base(player, startingFile, startingRank)
        {
            PieceType = PieceTypes.Queen;
            IsSliding = true;
            Figurine = Player.Color == Colors.White ? "♕" : "♛";
        }

        internal override List<Move> GetValidMoves()
        {
            List<Move> validMoves = Rook.GetRookLikeValidMoves(this);

            foreach (Move move in Bishop.GetBishopLikeValidMoves(this))
            {
                validMoves.Add(move);
            }

            return validMoves;
        }

        internal override ICollection<Move> GetLegalMoves()
        {
            return RemoveIllegalMoves(GetValidMoves());
        }

        internal override Piece ClonePiece()
        {
            return new Queen(Player, CurrentFile.Value, CurrentRank.Value);
        }
    }
}