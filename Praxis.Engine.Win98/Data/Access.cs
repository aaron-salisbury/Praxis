using Praxis.Engine.Win98.Application;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Praxis.Engine.Win98.Data
{
    class Access
    {
        internal static bool FENRecordIsValid(string fenRecord)
        {
            if (string.IsNullOrEmpty(fenRecord))
            {
                Engine.Logger.Error("FEN record is invalid.");
                return false;
            }

            string[] fenFields = fenRecord.Split(default(string[]), StringSplitOptions.RemoveEmptyEntries);

            if (fenFields.Length != 6)
            {
                Engine.Logger.Error("The FEN record must contain exactly six space delimited fields.");
                return false;
            }

            if (!fenFields[1].Equals("w", StringComparison.OrdinalIgnoreCase) && !fenFields[1].ToString().Equals("b", StringComparison.OrdinalIgnoreCase))
            {
                Engine.Logger.Error("The second field in the FEN record must be 'w' or 'b' to designate the active player.");
                return false;
            }

            if (!fenFields[2].Contains("-") && !fenFields[2].Contains("K") && !fenFields[2].Contains("Q") && !fenFields[2].Contains("k") && !fenFields[2].Contains("q"))
            {
                Engine.Logger.Error("The third field in the FEN record must contain a '-' or a combination of 'K', 'Q', 'k', and 'q' to designate castling availability.");
                return false;
            }

            if (!fenFields[3].Equals("-") && fenFields[3].Length != 2)
            {
                Engine.Logger.Error("The fourth field in the FEN record must be a '-' or the two character algebraic chess notation that designates an en passant square.");
                return false;
            }
            else if (fenFields[3].Length == 2)
            {
                if ((!fenFields[3][0].Equals('a') && !fenFields[3][0].Equals('b') && !fenFields[3][0].Equals('c') && !fenFields[3][0].Equals('d') && !fenFields[3][0].Equals('e') && !fenFields[3][0].Equals('f') && !fenFields[3][0].Equals('g') && !fenFields[3][0].Equals('h')) ||
                    !char.IsNumber(fenFields[3][1]) ||
                    (Convert.ToInt32(fenFields[3][1].ToString()) < 1 || Convert.ToInt32(fenFields[3][1].ToString()) > 8))
                {
                    Engine.Logger.Error("The fourth field in the FEN record must be a '-' or a valid two character algebraic chess notation that designates an en passant square.");
                    return false;
                }
            }

            bool halfMoveIsANumber = true;
            foreach (char halfmoveNum in fenFields[4])
            {
                if (!char.IsNumber(halfmoveNum))
                {
                    halfMoveIsANumber = false;
                    break;
                }
            }
            if (!halfMoveIsANumber)
            {
                Engine.Logger.Error("The fifth field in the FEN record must be a number to designate the halfmove clock.");
                return false;
            }

            bool fullMoveIsANumber = true;
            foreach (char fullmoveNum in fenFields[5])
            {
                if (!char.IsNumber(fullmoveNum))
                {
                    fullMoveIsANumber = false;
                    break;
                }
            }
            if (!fullMoveIsANumber)
            {
                Engine.Logger.Error("The sixth field in the FEN record must be a number to designate the fullmove number.");
                return false;
            }

            return true;
        }

        internal static List<Opening> GetOpenings()
        {
            try
            {
                List<Opening> openings = new List<Opening>(10231);

                XElement ecoCodesXML = XElement.Parse(Properties.Resources.EcoClassifications);

                foreach (XElement openingElement in ecoCodesXML.Elements("Classification"))
                {
                    string code = (string)openingElement.Attribute("code");

                    if (!code.Equals("A00") && !code.Equals("B00"))
                    {
                        openings.Add(new Opening()
                        {
                            Code = code,
                            Name = (string)openingElement.Attribute("name"),
                            Value = openingElement.Value
                        });
                    }
                }

                return openings.OrderBy(opening => opening.Value.Length).ToList();
            }
            catch (Exception e)
            {
                Engine.Logger.Error($"Failed to retrieve ECO Classifications: {e.Message}");
                return null;
            }
        }
    }
}
