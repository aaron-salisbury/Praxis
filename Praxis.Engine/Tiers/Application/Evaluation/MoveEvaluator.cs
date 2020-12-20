using Praxis.Engine.Tiers.Application.BoardRepresentation;

namespace Praxis.Engine.Tiers.Application.Evaluation
{
    internal abstract class MoveEvaluator
    {
        internal Engine Engine { get; set; }

        internal MoveEvaluator(Engine engine)
        {
            Engine = engine;
        }

        internal abstract Move GetBestMove();
    }
}
