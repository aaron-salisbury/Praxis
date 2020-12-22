using Praxis.Engine.Tiers.Application.BoardRepresentation;

namespace Praxis.Engine.Tiers.Application.Evaluation
{
    internal abstract class BaseTablebaseProber
    {
        internal bool RequiresInternet { get; set; }
        internal int NumberPieceMax { get; set; }

        internal abstract Move GetBestMove(Engine engine);
    }
}
