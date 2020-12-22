using Praxis.Engine.Application.BoardRepresentation;

namespace Praxis.Engine.Application.Evaluation
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
