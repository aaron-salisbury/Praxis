using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Praxis.Engine.Tiers.Data
{
    class WebServices
    {
        internal static async Task<string> GetSOAPResponse(string actionURL, string content, Uri requestUri)
        {
            try
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
            catch (Exception e)
            {
                Engine.Logger.Error($"SOAP request failed: {e.Message}");
                return null;
            }
        }

        internal static async Task<string> GetCurlResponse(string curlURL, string content)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    FormUrlEncodedContent encodedContent = new FormUrlEncodedContent(new[] { new KeyValuePair<string, string>("text", content) });

                    using (HttpResponseMessage message = await client.PostAsync(curlURL, encodedContent))
                    using (var reader = new StreamReader(await message.Content.ReadAsStreamAsync()))
                    {
                        return await reader.ReadToEndAsync();
                    }
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
