using Praxis.Engine.Win98.Application.BoardRepresentation;

namespace Praxis.Engine.Win98.Application.Evaluation
{
    internal abstract class BaseTablebaseProber
    {
        internal bool RequiresInternet { get; set; }
        internal int NumberPieceMax { get; set; }

        internal abstract Move GetBestMove(Engine engine);
    }
}
