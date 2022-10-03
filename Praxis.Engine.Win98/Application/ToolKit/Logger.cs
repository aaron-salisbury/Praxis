using Praxis.Engine.Win98.Application.ToolKit.Logging;
using System.Collections.Generic;
using System.Linq;

namespace Praxis.Engine.Win98.Application.ToolKit
{
    internal class Logger
    {
        internal Logger()
        {
            AppLogger.SetTargetInvoking(this.WriteLatestToConsole);
        }

        internal void Error(string errorMessage)
        {
            AppLogger.Write(errorMessage, AppLogger.LogCategories.Error);
        }

        internal void WriteLatestToConsole(IList<string> logs)
        {
            if (logs == null && logs.Count > 0)
            {
                System.Console.WriteLine(logs.LastOrDefault());
            }
        }
    }
}
