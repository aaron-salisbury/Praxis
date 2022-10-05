using System;
using System.IO;
using System.Net;
using System.Xml;

namespace Praxis.Engine.Win98.Data
{
    class WebServices
    {
        internal static string GetSOAPResponse(string actionURL, string soapEnvelope, Uri requestUri)
        {
            // stackoverflow.com/questions/4791794/client-to-send-soap-request-and-receive-response

            try
            {
                HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(requestUri);
                webRequest.Headers.Add("SOAPAction", actionURL);
                webRequest.ContentType = "text/xml;charset=\"utf-8\"";
                webRequest.Accept = "text/xml";
                webRequest.Method = "POST";

                XmlDocument soapEnvelopeDocument = new XmlDocument();
                soapEnvelopeDocument.LoadXml(soapEnvelope);
                using (Stream webRequestStream = webRequest.GetRequestStream())
                {
                    soapEnvelopeDocument.Save(webRequestStream);
                }

                IAsyncResult asyncResult = webRequest.BeginGetResponse(null, null);
                asyncResult.AsyncWaitHandle.WaitOne();

                using (WebResponse webResponse = webRequest.EndGetResponse(asyncResult))
                using (StreamReader responseReader = new StreamReader(webResponse.GetResponseStream()))
                {
                    return responseReader.ReadToEnd();
                }

                //using (HttpClient client = new HttpClient())
                //{
                //    client.DefaultRequestHeaders.Add("SOAPAction", actionURL);
                //    StringContent httpContent = new StringContent(content, Encoding.UTF8, "text/xml");

                //    using (HttpResponseMessage message = await client.PostAsync(requestUri, httpContent))
                //    {
                //        byte[] soapResponseData = await message.Content.ReadAsByteArrayAsync();

                //        return Encoding.UTF8.GetString(soapResponseData, 0, soapResponseData.Length);
                //    }
                //}
            }
            catch (Exception e)
            {
                Engine.Logger.Error($"SOAP request failed: {e.Message}");
                return null;
            }
        }

        internal static async Task<string> GetCurlResponse(string curlURL)
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
