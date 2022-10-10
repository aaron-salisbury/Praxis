using Praxis.Base.Logging;
using Praxis.Base.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml;

namespace Praxis.DataAccess
{
    public class CRUD
    {
        private const string EMBEDDED_ECO_FILENAME = "Praxis.DataAccess.EcoClassifications.xml";
        private const string CLASSIFICATION_ELEMENT_NAME = "Classification";

        public static List<Opening> ReadOpenings()
        {
            try
            {
                List<Opening> openings = new List<Opening>(10231);

                #region System.Xml Version
                XmlTextReader reader = new XmlTextReader(new StringReader(GetEmbeddedResourceText(EMBEDDED_ECO_FILENAME)));
                reader.ReadToFollowing(CLASSIFICATION_ELEMENT_NAME);

                do
                {
                    string code = reader.GetAttribute("code");

                    if (!code.Equals("A00") && !code.Equals("B00"))
                    {
                        openings.Add(new Opening()
                        {
                            Code = code,
                            Name = reader.GetAttribute("name"),
                            Value = reader.ReadElementContentAsString()
                        });
                    }
                } while (reader.ReadToFollowing(CLASSIFICATION_ELEMENT_NAME));
                #endregion

                #region System.Xml.Linq Version
                //XElement ecoCodesXML = XElement.Parse(Properties.Resources.EcoClassifications);

                //foreach (XElement openingElement in ecoCodesXML.Elements("Classification"))
                //{
                //    string code = (string)openingElement.Attribute("code");

                //    if (!code.Equals("A00") && !code.Equals("B00"))
                //    {
                //        openings.Add(new Opening()
                //        {
                //            Code = code,
                //            Name = (string)openingElement.Attribute("name"),
                //            Value = openingElement.Value
                //        });
                //    }
                //}
                #endregion

                return openings.OrderBy(opening => opening.Value.Length).ToList();
            }
            catch (Exception e)
            {
                Logger.Error($"Failed to retrieve ECO Classifications: {e.Message}");
                return null;
            }
        }

        private static string GetEmbeddedResourceText(string filename)
        {
            string result = string.Empty;

            using (Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(filename))
            using (StreamReader streamReader = new StreamReader(stream))
            {
                result = streamReader.ReadToEnd();
            }

            return result;
        }
    }
}
