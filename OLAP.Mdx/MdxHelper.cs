using System;
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

        public static MdxMovAvg MovAvg(string dateDimension, IMdxElement measure, int offsetDays)
        {
            return new MdxMovAvg(
                new MdxHierarchy(dateDimension).CurrentMember(),
                measure,
                offsetDays);
        }
    }
}