using System;

namespace Praxis.Engine.Tiers.Application.ToolKit
{
    public static class ExtensionMethods
    {
        public static int CountPeriods(this string str)
        {
            int numberOfPeriods = 0;

            foreach (char character in str)
            {
                if (character == '.')
                {
                    numberOfPeriods++;
                }
            }

            return numberOfPeriods;
        }
    }
}
