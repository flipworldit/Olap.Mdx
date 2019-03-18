using System.Collections.Generic;

namespace OLAP.Mdx.Common
{
    public static class PeriodTypeExt
    {
        static readonly Dictionary<PeriodType, int> DaysInPeriodType = new Dictionary<PeriodType, int>
        {
            {PeriodType.Day, 1},
            {PeriodType.ProductionWeek, 7},
            {PeriodType.CalendarMonth, 30},
            {PeriodType.PreviousMonth, 30},
            {PeriodType.ProductionMonth, 28},
            {PeriodType.Quarter, 90},
            {PeriodType.HalfYear, 178},
            {PeriodType.Year, 356},
            {PeriodType.FourWeekPeriod, 28}
        };

        public static int DaysCount(this PeriodType periodType)
        {
            return DaysInPeriodType[periodType];
        }
    }
}