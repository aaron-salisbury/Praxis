using Praxis.Business.BoardRepresentation;
using Praxis.Business.Evaluation.TablebaseProbers;

namespace Praxis.Business.Evaluation
{
    internal class EndGameMoveEvaluator : BaseMoveEvaluator
    {
        internal ITablebaseProberService TablebaseProber { get; set; }

        internal EndGameMoveEvaluator(Engine engine) : base(engine)
        {
            TablebaseProber = new SyzygyTablebaseProberService();
        }

        internal override Move GetBestMove()
        {
            //TODO: Add fallback logic. If one prober fails, set/try another.
            //TODO: If all tablebase porbers require internet, add error upfront when no internet is detected and dont even start the game.
            //TODO: Look into using or making an event for losing internet in case they disconnect mid-game.

            return TablebaseProber.GetBestMove(Engine);
        }
    }
}
