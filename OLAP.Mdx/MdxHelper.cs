using System;
using SystemExt;
using OLAP.Mdx.Common;
using OLAP.Mdx.MdxElements;

namespace OLAP.Mdx
{
    public class MdxHelper
    {
        public static string GetMdxStringDate(DateTime date)
        {
            var mdxDate = String.Format("{0}T00:00:00", date.ToString("yyyy-MM-dd"));
            return mdxDate;
        }

        public static IMdxElement GetWhereDateRangeProductionMonth(DateTime productionMonthBeginDate)
        {
            var dateRange = new Range<DateTime>(productionMonthBeginDate,
                productionMonthBeginDate.EndProductionMonth());

            return GetWhereDateRange(dateRange);
        }

        public static IMdxElement GetWhereDateRange(Range<DateTime> dateRange)
        {
            var mdxDateFunc = DateMdxHelper.GetMdxDateValue[PeriodType.Day];

            var beginDate = mdxDateFunc(dateRange.StartValue)[0];
            var endDate = mdxDateFunc(dateRange.EndValue)[0];

            return GetMdxDateRange("[Dates].[Date]", beginDate, endDate);

        }

        public static MdxRangeElement GetMdxDateRange(string hierarchies, string prevBeginDate, string prevEndDate)
        {
            return new MdxRangeElement(
                new MdxHierarchy(hierarchies).Value(prevBeginDate),
                new MdxHierarchy(hierarchies).Value(prevEndDate));
        }

        
        public static MdxRangeElement GetDatesFaceDateRange(string prevBeginDate, string prevEndDate)
        {
            return GetMdxDateRange("[Dates].[Face Date]", prevBeginDate, prevEndDate);
        }

        public static MdxRangeElement GetDatesDateRange(string prevBeginDate, string prevEndDate)
        {
            return GetMdxDateRange("[Dates].[Date]", prevBeginDate, prevEndDate);
        }

        public static MdxRangeElement GetMdxDateRange(string hierarchies, Range<DateTime> range)
        {
            return GetMdxDateRange(hierarchies, GetMdxStringDate(range.StartValue), GetMdxStringDate(range.EndValue));
        }

        public static MdxRangeElement GetMonthOffset(string dateHierarchyName="[Dates].[Date]", int firstLagValue=29, int secondLagValue=0)
        {
            return new MdxRangeElement(
                new MdxHierarchy(dateHierarchyName).CurrentMember().Lag(firstLagValue),
                new MdxHierarchy(dateHierarchyName).CurrentMember().Lag(secondLagValue));
        }

        public static MdxMovAvg MovAvg(string dateDimension, IMdxElement measure, int offsetDays)
        {
            return new MdxMovAvg(
                new MdxHierarchy(dateDimension).CurrentMember(),
                measure,
                offsetDays);
        }
    }
}