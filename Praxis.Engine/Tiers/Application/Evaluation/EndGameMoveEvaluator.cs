using Praxis.Engine.Tiers.Application.BoardRepresentation;
using Praxis.Engine.Tiers.Data;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Praxis.Engine.Tiers.Application.Evaluation
{
    internal class EndGameMoveEvaluator : MoveEvaluator
    {
        internal EndGameMoveEvaluator(Engine engine) : base(engine) { }

        internal override Move GetBestMove()
        {
            //TODO: Right now just using Lokasoft's tablebase for the best move. That tablebase only handles 5 pieces on the board.
            //      There are tablebases that hold more, but they are very large and I haven't found free services for them yet.
            //      Also, the fact that accessing the tablebase uses a web service, means that the application requires an internet connection.
            Dictionary<int, List<string>> movesByScores = WebServices.GetBestMovesByScores(Engine.FEN);

            if (movesByScores == null)
            {
                return null;
            }

            string bestMove = movesByScores
                .Where(mbs => mbs.Key > 0)
                .OrderBy(mbs => mbs.Key)
                .Select(mbs => mbs.Value.FirstOrDefault()).FirstOrDefault();

            if (string.IsNullOrEmpty(bestMove))
            {
                bestMove = movesByScores
                    .Where(mbs => mbs.Key == 0)
                    .Select(mbs => mbs.Value.FirstOrDefault()).FirstOrDefault();
            }

            if (string.IsNullOrEmpty(bestMove))
            {
                bestMove = movesByScores
                    .OrderByDescending(mbs => Math.Abs(mbs.Key))
                    .Select(mbs => mbs.Value.FirstOrDefault()).FirstOrDefault();
            }

            return Move.ConvertAlgebraicToMove(bestMove, Engine);
        }
    }
}
