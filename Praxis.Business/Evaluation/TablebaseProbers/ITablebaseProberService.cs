using Praxis.Business.BoardRepresentation;

namespace Praxis.Business.Evaluation.TablebaseProbers
{
    internal interface ITablebaseProberService
    {
        int NumberPieceMax();
        Move GetBestMove(Engine engine);
    }
}
