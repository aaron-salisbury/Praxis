using Praxis.Engine.Application.BoardRepresentation;
using System.Collections.Generic;


namespace Praxis.Engine.Application.Evaluation
{
    internal class MiddleGameMoveEvaluator : MoveEvaluator
    {
        internal MiddleGameMoveEvaluator(Engine engine) : base(engine) { }

        internal override Move GetBestMove()
        {
            List<Move> potentialMoves = Engine.WhitePlayer.MovesNext ? Engine.WhitePlayer.GetLegalMoves() : Engine.BlackPlayer.GetLegalMoves();

            //TODO: For now, just returning random move.
            int moveIndex = Engine.RandomGenerator.Next(0, potentialMoves.Count);
            Move bestMove = potentialMoves[moveIndex];

            return bestMove;
        }
    }
}
