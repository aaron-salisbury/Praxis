using Praxis.Engine.Win98.Application.ToolKit;
using System.Collections.Generic;
using static Praxis.Engine.Win98.Application.Types;

namespace Praxis.Engine.Win98.Application
{
    //TODO:
    // http://www.chessfiles.com/fools-mate.html
    // http://www.chessfiles.com/scholars-mate.html

    internal class Opening
    {
        internal string Code { get; set; }
        internal string Name { get; set; }
        internal string Value { get; set; }
        internal Styles Style { get; set; }

        internal static bool PotentialOpeningsExist(Engine engine)
        {
            bool potentialOpeningsExist = false;

            foreach (Opening opening in engine.Openings)
            {
                if (opening.Value.StartsWith(engine.SAN) && opening.Value.CountPeriods() > engine.SAN.CountPeriods())
                {
                    potentialOpeningsExist = true;
                    break;
                }
            }

            return potentialOpeningsExist;
        }

        internal static List<Opening> GetPotentialOpenings(Engine engine)
        {
            List<Opening> potentialOpenings = new List<Opening>();

            foreach (Opening opening in engine.Openings)
            {
                if (opening.Value.StartsWith(engine.SAN) && opening.Value.CountPeriods() > engine.SAN.CountPeriods())
                {
                    potentialOpenings.Add(opening);
                }
            }

            return potentialOpenings;
        }
    }
}
