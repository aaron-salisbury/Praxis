using Praxis.Engine.Tiers.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Praxis.Engine.Tiers.Application.Evaluation
{
    /// <summary>
    /// Tablebase WebService Documentation: http://www.lokasoft.nl/tbapi.aspx
    /// </summary>
    internal class LokasoftTablebaseProber: BaseTablebaseProber
    {
        internal LokasoftTablebaseProber()
        {
            RequiresInternet = true;
            NumberPieceMax = 5;
            RequestType = RequestTypes.SOAP;
            RequestURI = "http://www.lokasoft.nl/tbweb/tbapi.asp";
            ActionURL = "http://lokasoft.org/action/TB2ComObj.GetBestMoves";
        }

        internal override Dictionary<int, List<string>> GetScoredMoves(Engine engine)
        {
            BuildRequestContent(engine.FEN);

            return ConvertResponseToScoredMoves(MakeRequestForMoves());
        }

        protected override void BuildRequestContent(string fen)
        {
            RequestContent = string.Format(@"<soapenv:Envelope xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:mes=""http://lokasoft.org/message/"">
                    <soapenv:Body>
                        <mes:GetBestMoves soapenv:encodingStyle=""http://schemas.xmlsoap.org/soap/encoding/"">
                            <fen xsi:type=""xsd:string"">{0}</fen>
                        </mes:GetBestMoves>
                    </soapenv:Body>
                </soapenv:Envelope>", fen);
        }

        protected override string MakeRequestForMoves()
        {
            return WebServices.GetSOAPResponse(ActionURL, RequestContent, new Uri(RequestURI)).Result;
        }

        protected override Dictionary<int, List<string>> ConvertResponseToScoredMoves(string serviceResponce)
        {
            const string RESULTS_NODE = "Result";

            Dictionary<int, List<string>> movesByScores = new Dictionary<int, List<string>>();

            try
            {
                XDocument responseXML = XDocument.Parse(serviceResponce);

                XElement resultElement = responseXML.Descendants(RESULTS_NODE).FirstOrDefault();
                string resultValue = resultElement.Value;
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
