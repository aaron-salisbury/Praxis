using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Praxis.Engine.Tiers.Data
{
    class WebServices
    {
        /// <summary>
        /// Tablebase WebService Documentation: http://www.lokasoft.nl/tbapi.aspx
        /// </summary>
        internal static Dictionary<int, List<string>> GetBestMovesByScores(string fen)
        {
            const string RQUEST_URI = "http://www.lokasoft.nl/tbweb/tbapi.asp";
            const string ACTION_URL = "http://lokasoft.org/action/TB2ComObj.GetBestMoves";
            const string RESULTS_NODE = "Result";

            Dictionary<int, List<string>> movesByScores = new Dictionary<int, List<string>>();

            string content = string.Format(@"<soapenv:Envelope xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:mes=""http://lokasoft.org/message/"">
                    <soapenv:Body>
                        <mes:GetBestMoves soapenv:encodingStyle=""http://schemas.xmlsoap.org/soap/encoding/"">
                            <fen xsi:type=""xsd:string"">{0}</fen>
                        </mes:GetBestMoves>
                    </soapenv:Body>
                </soapenv:Envelope>", fen);

            try
            {
                string soapResponse = SoapLather(ACTION_URL, content, new Uri(RQUEST_URI)).Result;
                XDocument responseXML = XDocument.Parse(soapResponse);

                XElement resultElement = responseXML.Descendants(RESULTS_NODE).FirstOrDefault();
                string resultValue = resultElement.Value;
                string[] movesAndScores = resultValue.Split(default(string[]), StringSplitOptions.RemoveEmptyEntries);

                #region Convert Service Moves & Scores
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
                #endregion

                return movesByScores;
            }
            catch (Exception e)
            {
                Engine.Logger.Error($"Failed to retrieve and build moves from tablebase: {e.Message}");
                return null;
            }
        }

        private static async Task<string> SoapLather(string actionURL, string content, Uri requestUri)
        {
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("SOAPAction", actionURL);
                StringContent httpContent = new StringContent(content, Encoding.UTF8, "text/xml");

                using (HttpResponseMessage message = await client.PostAsync(requestUri, httpContent))
                {
                    byte[] soapResponseData = await message.Content.ReadAsByteArrayAsync();

                    return Encoding.UTF8.GetString(soapResponseData, 0, soapResponseData.Length);
                }
            }
        }
    }
}
