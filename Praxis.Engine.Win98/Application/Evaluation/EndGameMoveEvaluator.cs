using Praxis.Engine.Win98.Application.BoardRepresentation;

namespace Praxis.Engine.Win98.Application.Evaluation
{
    internal class EndGameMoveEvaluator : MoveEvaluator
    {
        internal BaseTablebaseProber TablebaseProber { get; set; }

        internal EndGameMoveEvaluator(Engine engine) : base(engine)
        {
            TablebaseProber = new SyzygyTablebaseProber();
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
