using Praxis.Engine.Win98.Application.BoardRepresentation;

namespace Praxis.Engine.Win98.Application.Evaluation
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
