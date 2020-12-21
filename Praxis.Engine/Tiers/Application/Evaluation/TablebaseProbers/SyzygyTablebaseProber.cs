using Praxis.Engine.Tiers.Data;
using System;
using System.Collections.Generic;

namespace Praxis.Engine.Tiers.Application.Evaluation
{
    /// <summary>
    /// Tablebase WebService Documentation: https://github.com/niklasf/lila-tablebase#http-api
    /// </summary>
    internal class SyzygyTablebaseProber : BaseTablebaseProber
    {
        internal SyzygyTablebaseProber()
        {
            RequiresInternet = true;
            NumberPieceMax = 7; //TODO: Verify this, it might be 6.
            RequestType = RequestTypes.Curl;
            CurlURL = "http://tablebase.lichess.ovh/standard";
        }

        internal override Dictionary<int, List<string>> GetScoredMoves(Engine engine)
        {
            BuildRequestContent(engine.FEN);

            return ConvertResponseToScoredMoves(MakeRequestForMoves());
        }

        protected override void BuildRequestContent(string fen)
        {
            RequestContent = $"fen={fen}";
        }

        protected override string MakeRequestForMoves()
        {
            return WebServices.GetCurlResponse(CurlURL, RequestContent).Result;
        }

        protected override Dictionary<int, List<string>> ConvertResponseToScoredMoves(string serviceResponce)
        {
            Dictionary<int, List<string>> movesByScores = new Dictionary<int, List<string>>();

            try
            {
                //TODO: 

                return movesByScores;
            }
            catch (Exception e)
            {
                Engine.Logger.Error($"Failed to parse and convert moves from the Syzygy tablebase response: {e.Message}");
                return null;
            }
        }
    }
}
