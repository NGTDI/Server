using System;

namespace NGTDI.Library.Constants
{
    public static class NGTDIPeriod
    {
        public const string AllYear = "AllYear";
        public const string Before = "Before";
        public const string After = "After";
        public const string SpecificDate = "SpecificDate";
        public const string Period = "PeriodOfTime";

        public static string ToEnglish(string _period)
        {
            if (_period != null)
            {
                if (_period.Equals(AllYear, StringComparison.InvariantCultureIgnoreCase))
                {
                    return "All Year";
                }
                
                if (_period.Equals(Before, StringComparison.InvariantCultureIgnoreCase))
                {
                    return "Before";
                }
                
                if (_period.Equals(After, StringComparison.InvariantCultureIgnoreCase))
                {
                    return "After";
                }
                
                if (_period.Equals(SpecificDate, StringComparison.InvariantCultureIgnoreCase))
                {
                    return "Specific Date";
                }
                
                if (_period.Equals(Period, StringComparison.InvariantCultureIgnoreCase))
                {
                    return "Time Range";
                }
            }

            return null;
        }
    }
}
