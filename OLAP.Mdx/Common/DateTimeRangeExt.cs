using System;
using System.Collections.Generic;
using SystemExt;

namespace OLAP.Mdx.Common
{
    public static class DateTimeRangeExt
    {
        public static DateTime[] SplitByPeriodType(this Range<DateTime> dateRange, PeriodType periodType)
        {
            var dates = new List<DateTime>();

            var date = dateRange.StartValue;

            while (date <= dateRange.EndValue)
            {
                dates.Add(date.Date);
                if (periodType == PeriodType.Null)
                {
                    date = dateRange.EndValue.AddDays(1).AddMilliseconds(-1);
                }
                else
                {
                    var endDateByPeriod = DateMdxHelper.GetEndDateByPeriod;

                    if (endDateByPeriod.ContainsKey(periodType))
                    {
                        date = endDateByPeriod[periodType](date).AddDays(1);
                    }
                    else
                    {
                        throw new KeyNotFoundException(string.Format("Данный ключ отсутствует в словаре ({0}).",
                            periodType.ToString()));
                    }
                }
            }

            return dates.ToArray();
        }

        public static Dictionary<DateTime, Tuple<DateTime, int>> GetDatesByProductionMonths(this Range<DateTime> dateRange)
        {
            var dateDict = new Dictionary<DateTime, Tuple<DateTime, int>>();

            var currentDate = dateRange.StartValue.BeginProductionMonth();

            while (currentDate <= dateRange.EndValue)
            {
                var nextMonth = currentDate.NextProductionMonth();
                var daysCount = currentDate.CountOfDaysInProductionMonth();

                dateDict.Add(currentDate.BeginProductionMonth(), new Tuple<DateTime, int>(currentDate.AddDays(daysCount), daysCount));
                currentDate = nextMonth;
            }

            return dateDict;
        }

        public static Dictionary<DateTime, Tuple<DateTime,int>> GetDatesByCalendersMonths(this Range<DateTime> dateRange)
        {
            var dateDict = new Dictionary<DateTime, Tuple<DateTime, int>>();

            var currentDate = new DateTime(dateRange.StartValue.Year, dateRange.StartValue.Month,1);

            while (currentDate <= dateRange.EndValue)
            {
                var nextMonth = currentDate.AddMonths(1);
                var daysCount = (nextMonth - currentDate).Days;

                dateDict.Add(currentDate,new Tuple<DateTime, int>(currentDate.AddDays(daysCount), daysCount));
                currentDate = nextMonth;
            }

            return dateDict;
        }

    }
}