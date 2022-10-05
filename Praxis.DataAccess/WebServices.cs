using Praxis.Base.Logging;
using System;
using System.IO;
using System.Net;
//using System.Net.Http;
//using System.Text;
//using System.Threading.Tasks;
using System.Xml;

namespace Praxis.DataAccess
{
    public class WebServices
    {
        public static string GetSOAPResponse(string actionURL, string soapEnvelope, Uri requestUri)
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

                using (WebResponse response = webRequest.EndGetResponse(asyncResult))
                using (StreamReader responseReader = new StreamReader(response.GetResponseStream()))
                {
                    return responseReader.ReadToEnd();
                }
            }
            catch (Exception e)
            {
                Logger.Error($"SOAP request failed: {e.Message}");
                return null;
            }
        }

        //internal static async Task<string> GetSOAPResponseAsync(string actionURL, string soapEnvelope, Uri requestUri)
        //{
        //    try
        //    {
        //        using (HttpClient client = new HttpClient())
        //        {
        //            client.DefaultRequestHeaders.Add("SOAPAction", actionURL);
        //            StringContent httpContent = new StringContent(soapEnvelope, Encoding.UTF8, "text/xml");

        //            using (HttpResponseMessage message = await client.PostAsync(requestUri, httpContent))
        //            {
        //                byte[] soapResponseData = await message.Content.ReadAsByteArrayAsync();

        //                return Encoding.UTF8.GetString(soapResponseData, 0, soapResponseData.Length);
        //            }
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        Logger.Error($"SOAP request failed: {e.Message}");
        //        return null;
        //    }
        //}

        public static string GetCurlResponse(string curlURL)
        {
            try
            {
                WebRequest webRequest = WebRequest.Create(curlURL);

                using (WebResponse response = webRequest.GetResponse())
                using (StreamReader responseReader = new StreamReader(response.GetResponseStream()))
                {
                    return responseReader.ReadToEnd();
                }
            }
            catch (Exception e)
            {
                Logger.Error($"Curl request failed: {e.Message}");
                return null;
            }
        }

        //internal static async Task<string> GetCurlResponseAsync(string curlURL)
        //{
        //    try
        //    {
        //        using (HttpClient client = new HttpClient())
        //        {
        //            return await client.GetStringAsync(curlURL);
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        Logger.Error($"Curl request failed: {e.Message}");
        //        return null;
        //    }
        //}
    }
}
