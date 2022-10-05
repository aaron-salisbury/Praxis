using System.Collections.Generic;
using System.Linq;

namespace Praxis.Base.Logging
{
    public static class Logger
    {
        private static bool _targetInvokingSet = false;

        public static void Error(string errorMessage)
        {
            if (!_targetInvokingSet)
            {
                AppLogger.SetTargetInvoking(WriteLatestToConsole);
                _targetInvokingSet = true;
            }

            AppLogger.Write(errorMessage, AppLogger.LogCategories.Error);
        }

        public static void WriteLatestToConsole(IList<string> logs)
        {
            if (logs == null && logs.Count > 0)
            {
                System.Console.WriteLine(logs.LastOrDefault());
            }
        }
    }
}
