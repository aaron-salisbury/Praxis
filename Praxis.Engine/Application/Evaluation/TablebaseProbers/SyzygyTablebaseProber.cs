using Newtonsoft.Json;
using Praxis.Engine.Application.BoardRepresentation;
using Praxis.Engine.Data;
using System;

namespace Praxis.Engine.Application.Evaluation
{
    /// <summary>
    /// Tablebase WebService Documentation: https://github.com/niklasf/lila-tablebase#http-api
    /// </summary>
    internal class SyzygyTablebaseProber : BaseTablebaseProber
    {
        private const string CURL_URL = "http://tablebase.lichess.ovh/standard";

        internal SyzygyTablebaseProber()
        {
            RequiresInternet = true;
            NumberPieceMax = 7;
        }

        internal override Move GetBestMove(Engine engine)
        {
            string requestContent = $"?fen={engine.FEN.Replace(" ", "_")}";

            string curl = $"{CURL_URL}{requestContent}";

            string response = WebServices.GetCurlResponseAsync(curl).Result;

            try
            {
                SyzygyWebStandard syzygyWebStandard = JsonConvert.DeserializeObject<SyzygyWebStandard>(response);

                if (syzygyWebStandard != null && syzygyWebStandard.Moves.Length > 0)
                {
                    SyzygyWebStandardMove bestSyzygyMove = syzygyWebStandard.Moves[0]; // Syzygy returns best move first.

                    return Move.ConvertAlgebraicToMove(bestSyzygyMove.UCI, engine);
                }
            }
            catch (Exception e)
            {
                Engine.Logger.Error($"Failed to deserialize and convert moves from the Syzygy tablebase response: {e.Message}");
                return null;
            }

            return null;
        }

        [Serializable]
        internal class SyzygyWebStandard
        {
            public int? WDL { get; set; }
            public int? DTZ { get; set; }
            public int? DTM { get; set; }
            public bool Checkmate { get; set; }
            public bool Stalemate { get; set; }
            public bool Variant_Win { get; set; }
            public bool Variant_Loss { get; set; }
            public bool Insufficient_Material { get; set; }
            public SyzygyWebStandardMove[] Moves { get; set; }
        }

        [Serializable]
        internal class SyzygyWebStandardMove
        {
            public string UCI { get; set; }
            public string SAN { get; set; }
            public int? WDL { get; set; }
            public int? DTZ { get; set; }
            public int? DTM { get; set; }
            public bool Zeroing { get; set; }
            public bool Checkmate { get; set; }
            public bool Stalemate { get; set; }
            public bool Varient_Win { get; set; }
            public bool Varient_Loss { get; set; }
            public bool Insufficient_Material { get; set; }
        }
    }
}
