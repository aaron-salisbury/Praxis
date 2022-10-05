using Praxis.Engine.Win98.Application.BoardRepresentation;
using Praxis.Engine.Win98.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;

namespace Praxis.Engine.Win98.Application.Evaluation
{
    /// <summary>
    /// Tablebase WebService Documentation: http://www.lokasoft.nl/tbapi.aspx
    /// </summary>
    internal class LokasoftTablebaseProber: BaseTablebaseProber
    {
        private const string REQUEST_URI = "http://www.lokasoft.nl/tbweb/tbapi.asp";
        private const string ACTION_URL = "http://lokasoft.org/action/TB2ComObj.GetBestMoves";

        internal LokasoftTablebaseProber()
        {
            RequiresInternet = true;
            NumberPieceMax = 5;
        }

        internal override Move GetBestMove(Engine engine)
        {
            string requestContent = BuildRequestContent(engine.FEN);

            string response = WebServices.GetSOAPResponse(ACTION_URL, requestContent, new Uri(REQUEST_URI)).Result;

            Dictionary<int, List<string>> scoredMoves = GetScoredMoves(response);

            if (scoredMoves != null)
            {
                // Lokasoft "score is given as distance to mat, or 0 when the position is a draw.".
                string bestMove = scoredMoves
                    .Where(mbs => mbs.Key > 0)
                    .OrderBy(mbs => mbs.Key)
                    .Select(mbs => mbs.Value.FirstOrDefault()).FirstOrDefault();

                if (string.IsNullOrEmpty(bestMove))
                {
                    bestMove = scoredMoves
                        .Where(mbs => mbs.Key == 0)
                        .Select(mbs => mbs.Value.FirstOrDefault()).FirstOrDefault();
                }

                if (string.IsNullOrEmpty(bestMove))
                {
                    bestMove = scoredMoves
                        .OrderByDescending(mbs => Math.Abs(mbs.Key))
                        .Select(mbs => mbs.Value.FirstOrDefault()).FirstOrDefault();
                }

                return Move.ConvertAlgebraicToMove(bestMove, engine);
            }

            return null;
        }

        private string BuildRequestContent(string fen)
        {
            return string.Format(@"<soapenv:Envelope xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:mes=""http://lokasoft.org/message/"">
                    <soapenv:Body>
                        <mes:GetBestMoves soapenv:encodingStyle=""http://schemas.xmlsoap.org/soap/encoding/"">
                            <fen xsi:type=""xsd:string"">{0}</fen>
                        </mes:GetBestMoves>
                    </soapenv:Body>
                </soapenv:Envelope>", fen);
        }

        private Dictionary<int, List<string>> GetScoredMoves(string response)
        {
            if (string.IsNullOrEmpty(response))
            {
                return null;
            }

            const string RESULTS_NODE = "Result";

            Dictionary<int, List<string>> movesByScores = new Dictionary<int, List<string>>();

            try
            {
                XmlTextReader reader = new XmlTextReader(new StringReader(response));
                reader.ReadToFollowing(RESULTS_NODE);

                string resultValue = reader.ReadElementContentAsString();
                string[] movesAndScores = resultValue.Split(default(string[]), StringSplitOptions.RemoveEmptyEntries);

                for (int i = 0; i < movesAndScores.Length; i += 2)
                {
                    string move = movesAndScores[i];

                    if (!move.Equals("O-O") && !move.Equals("O-O-O"))
                    {
                        move = move.Replace("-", string.Empty);

                        if (char.IsUpper(move[0]))
                        {
                            move = move.Replace(move[0].ToString(), string.Empty);
                        }
                    }

                    string stringScore = movesAndScores[i + 1];

                    stringScore = stringScore.Replace("M", string.Empty);

                    int score = Convert.ToInt32(stringScore);

                    if (movesByScores.ContainsKey(score))
                    {
                        movesByScores[score].Add(move);
                    }
                    else
                    {
                        movesByScores[score] = new List<string>() { move };
                    }
                }

                return movesByScores;
            }
            catch (Exception e)
            {
                Engine.Logger.Error($"Failed to parse and convert moves from the Lokasoft tablebase response: {e.Message}");
                return null;
            }
        }
    }
}
