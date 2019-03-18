using OLAP.Mdx.Common;
using OLAP.Mdx.MdxElements;

namespace OLAP.Mdx.MdxElementsExt
{
    public static class Month
    {
        public static MdxMember DayCount(MdxHierarchy datesMonthMeasure)
        {
            return new MdxMember("MonthDayCount",
                new MdxSum(
                    datesMonthMeasure,
                    "DayCount"));
        }

        public static MdxMultiply DayCountMultiply(string measure)
        {
            return new MdxMultiply(measure, "MonthDayCount");

        }

        public static IMdxElement Prognoz(string measure)
        {
            var dateRangeElement = new MdxRangeElement(
                new MdxHierarchy("[Dates].[Date]").CurrentMember().Lag("MonthDayCount - 1"),
                new MdxHierarchy("[Dates].[Date]").CurrentMember().Lag(0));

            return new MdxSum(dateRangeElement, new MdxMeasureElement(measure));

        }
    }
}
