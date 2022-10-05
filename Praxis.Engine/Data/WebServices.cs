using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Praxis.Engine.Data
{
    class WebServices
    {
        internal static async Task<string> GetSOAPResponseAsync(string actionURL, string soapEnvelope, Uri requestUri)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Add("SOAPAction", actionURL);
                    StringContent httpContent = new StringContent(soapEnvelope, Encoding.UTF8, "text/xml");

                    using (HttpResponseMessage message = await client.PostAsync(requestUri, httpContent))
                    {
                        byte[] soapResponseData = await message.Content.ReadAsByteArrayAsync();

                        return Encoding.UTF8.GetString(soapResponseData, 0, soapResponseData.Length);
                    }
                }
            }
            catch (Exception e)
            {
                Engine.Logger.Error($"SOAP request failed: {e.Message}");
                return null;
            }
        }

        internal static async Task<string> GetCurlResponseAsync(string curlURL)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    return await client.GetStringAsync(curlURL);
                }
            }
            catch (Exception e)
            {
                Engine.Logger.Error($"Curl request failed: {e.Message}");
                return null;
            }
        }
    }
}
