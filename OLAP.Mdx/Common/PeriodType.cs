using System;

namespace OLAP.Mdx.Common
{
    [Flags]
    public enum PeriodType
    {
        Day=0,
        ProductionWeek=1,
        CalendarMonth=2,
        ProductionMonth=3,
        Quarter=4,
        HalfYear=5, 
        Year,
        //FourWeekPeriod,
        Null 
    }
}