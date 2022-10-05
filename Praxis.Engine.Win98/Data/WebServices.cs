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

                using (WebResponse response = webRequest.EndGetResponse(asyncResult))
                using (StreamReader responseReader = new StreamReader(response.GetResponseStream()))
                {
                    return responseReader.ReadToEnd();
                }
            }
            catch (Exception e)
            {
                Engine.Logger.Error($"SOAP request failed: {e.Message}");
                return null;
            }
        }

        internal static string GetCurlResponse(string curlURL)
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
                Engine.Logger.Error($"Curl request failed: {e.Message}");
                return null;
            }
        }
    }
}
