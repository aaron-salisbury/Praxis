using System.Collections.Generic;

namespace Praxis.Engine.Tiers.Application.Evaluation
{
    internal abstract class BaseTablebaseProber
    {
        internal enum RequestTypes
        {
            SOAP,
            Curl
        }

        internal bool RequiresInternet { get; set; }
        internal int NumberPieceMax { get; set; }
        internal RequestTypes RequestType { get; set; }
        internal string RequestContent { get; set; }

        //TODO: Conditional required validation attributes based on request type, and require all of the above properties except content.
        internal string CurlURL { get; set; }
        internal string RequestURI { get; set; }
        internal string ActionURL { get; set; }

        internal abstract Dictionary<int, List<string>> GetScoredMoves(Engine engine);

        protected abstract void BuildRequestContent(string fen);

        protected abstract string MakeRequestForMoves();

        protected abstract Dictionary<int, List<string>> ConvertResponseToScoredMoves(string serviceResponce);
    }
}
