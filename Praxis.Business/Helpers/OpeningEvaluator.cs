using Praxis.Base;
using Praxis.Base.Models;
using System.Collections.Generic;

namespace Praxis.Business.Helpers
{
    internal static class OpeningEvaluator
    {
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
