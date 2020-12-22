using Praxis.Engine.Application.BoardRepresentation;

namespace Praxis.Engine.Application.Evaluation
{
    internal abstract class BaseTablebaseProber
    {
        internal bool RequiresInternet { get; set; }
        internal int NumberPieceMax { get; set; }

        internal abstract Move GetBestMove(Engine engine);
    }
}
