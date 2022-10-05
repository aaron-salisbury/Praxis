using Praxis.Business.BoardRepresentation;

namespace Praxis.Business.Evaluation.TablebaseProbers
{
    internal abstract class BaseTablebaseProber
    {
        internal bool RequiresInternet { get; set; }
        internal int NumberPieceMax { get; set; }

        internal abstract Move GetBestMove(Engine engine);
    }
}
