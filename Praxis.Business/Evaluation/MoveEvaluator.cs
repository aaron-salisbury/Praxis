using Praxis.Business.BoardRepresentation;

namespace Praxis.Business.Evaluation
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
