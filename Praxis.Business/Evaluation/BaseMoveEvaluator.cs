using Praxis.Business.BoardRepresentation;

namespace Praxis.Business.Evaluation
{
    internal abstract class BaseMoveEvaluator
    {
        internal Engine Engine { get; set; }

        internal BaseMoveEvaluator(Engine engine)
        {
            Engine = engine;
        }

        internal abstract Move GetBestMove();
    }
}
