using Praxis.Engine.Win98.Application.ToolKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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
            return engine.Openings
                .AsQueryable()
                .Any(OpeningBuildsUponSANPredicate(engine.SAN));
        }

        internal static List<Opening> GetPotentialOpenings(Engine engine)
        {
            return engine.Openings
                .AsQueryable()
                .Where(OpeningBuildsUponSANPredicate(engine.SAN))
                .ToList();
        }

        private static Expression<Func<Opening, bool>> OpeningBuildsUponSANPredicate(string san)
        {
            return (opening => opening.Value.StartsWith(san) && opening.Value.CountPeriods() > san.CountPeriods());
        }
    }
}
