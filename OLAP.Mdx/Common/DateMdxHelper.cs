using System;
using System.Collections.Generic;
using SystemExt;

namespace OLAP.Mdx.Common
{
    public class DateMdxHelper
    {
        public static string GetProductionMonthTitle(DateTime date)
        {
            var productionYear = date.ProductionYear();

            date = date.BeginProductionMonth();

            var productionWeekNumber = date.ProductionWeekNumber();

            return String.Format("{0}{1} нед. {2} г.",
                productionWeekNumber,
                (productionWeekNumber == 53
                    ? ""
                    : "-" + (productionWeekNumber + 3)),
                productionYear);
        }

        public static string GetProductionMonthYearString(DateTime date)
        {
            return string.Format(
                "{0}-{1}",
                date.ProductionMonthNumber(),
                date.ProductionYear());
        }

        public static DateTime GetDateByWeekNumber(string week)
        {
            try
            {
                var dateParts = week.Split('/');

                var yearNumber = int.Parse(dateParts[0]);

                var weekNumber = int.Parse(dateParts[1]);

                var date = new DateTime(yearNumber, 1, 1);

                var dayOfWeek = date.RussianDayOfWeek();

                date = dayOfWeek < 4 ? date.BeginWeek() : date.AddDays(7 - dayOfWeek);

                return date.AddDays((weekNumber - 1) * 7);

            }
            catch (Exception)
            {
                throw new Exception("Не удалось преобразовать " + week + " в дату начала недели");
            }
        }


        public static readonly Dictionary<PeriodType, Func<DateTime, string[]>> GetMdxDateValue =
            new Dictionary<PeriodType, Func<DateTime, string[]>>
            {
                {
                    PeriodType.Day, d => new[]
                    {
                        String.Format("{0}T00:00:00", d.ToString("yyyy-MM-dd"))
                    }
                },
                {
                    PeriodType.ProductionWeek, d => new[]
                    {
                        d.ProductionYear(),
                        d.ProductionWeekNumber()
                    }.ToStringArray()
                },
                {
                    PeriodType.CalendarMonth, d => new[]
                    {
                        d.Year,
                        d.Month
                    }.ToStringArray()
                },
                {
                    PeriodType.ProductionMonth, d => new[]
                    {
                        d.ProductionYear(),
                        d.ProductionMonthNumber()
                    }.ToStringArray()
                },
                {
                    PeriodType.Quarter, d => new[]
                    {
                        d.Year,
                        ((d.Month - 1)/3 + 1)
                    }.ToStringArray()
                },
                {
                    PeriodType.HalfYear, d => new[]
                    {
                        d.Year,
                        d.GetHalfYear()
                    }.ToStringArray()
                },
                {
                    PeriodType.Year, d => new[]
                    {
                        d.Year
                    }.ToStringArray()
                },
                //{
                //    PeriodType.FourWeekPeriod, d => new[]
                //    {
                //        d.FourWeekPeriodYear(),
                //        d.FourWeekPeriodMonthNumber()

                //    }.ToStringArray()
                //}


            };

        public static readonly Dictionary<PeriodType, Func<DateTime, string>> GetDateTitleByPeriod =
            new Dictionary<PeriodType, Func<DateTime, string>>
            {
                {
                    PeriodType.Day, d => d.ToString("dd.MM.yyyy")
                },
                {
                    PeriodType.ProductionWeek, d => string.Format("{0} неделя {1}г.", d.ProductionWeekNumber(), d.Year)
                },
                {
                    PeriodType.CalendarMonth, d => d.ToString("MMMM yyyy")
                },
                {
                    PeriodType.ProductionMonth,
                    d =>
                    {
                        var productionWeekNumber = d.ProductionWeekNumber();

                        return
                            productionWeekNumber == 14
                                ? String.Format("{0} неделя {1}г.", productionWeekNumber, d.Year)
                                :String.Format("{0}-{1} недели {2}г.", productionWeekNumber, productionWeekNumber + 3,
                                    d.Year);
                    }
                },
                {
                    PeriodType.Quarter, d => string.Format("{0} квартал {1}г.", d.GetQuarter(), d.Year)
                },
                {
                    PeriodType.HalfYear, d => string.Format("{0} полугодие {1}г.", d.GetHalfYear(), d.Year)
                },
                {
                    PeriodType.Year, d => d.Year.ToString()
                },
                //{
                //    PeriodType.FourWeekPeriod, d =>
                //    {
                //        var fourWeekNumber  = d.FourWeekPeriodWeekNumber();

                //        return string.Format("{0}-{1} неделя {2}", fourWeekNumber, fourWeekNumber + 3, d.Year);
                //    }
                //}
            };

        public static readonly Dictionary<PeriodType, Func<DateTime, DateTime>> GetEndDateByPeriod =
            new Dictionary<PeriodType, Func<DateTime, DateTime>>
            {
                {
                    PeriodType.Day, d => d
                },
                {
                    PeriodType.ProductionWeek, d => d.EndWeek()
                },
                {
                    PeriodType.CalendarMonth, d => d.EndOfMonth()
                },
                {
                    PeriodType.ProductionMonth, d => d.EndProductionMonth()
                },
                {
                    PeriodType.Quarter, d => d.EndQuarter()
                },
                {
                    PeriodType.HalfYear, d => d.EndHalfYear()
                },
                {
                    PeriodType.Year, d => d.EndYear()
                },
                //{
                //    PeriodType.FourWeekPeriod, d => d.EndFourWeekPeriodMonth()
                //}
            };

        public static readonly Dictionary<PeriodType, Func<DateTime, DateTime>> GetBeginDateByPeriod =
            new Dictionary<PeriodType, Func<DateTime, DateTime>>
            {
                {
                    PeriodType.Day, d => d
                },
                {
                    PeriodType.ProductionWeek, d => d.BeginWeek()
                },
                {
                    PeriodType.CalendarMonth, d => d.BeginOfMonth()
                },
                {
                    PeriodType.ProductionMonth, d => d.BeginProductionMonth()
                },
                {
                    PeriodType.Quarter, d => d.BeginQuarter()
                },
                {
                    PeriodType.HalfYear, d => d.BeginHalfYear()
                },
                {
                    PeriodType.Year, d => d.BeginYear()
                },
                //{
                //    PeriodType.FourWeekPeriod, d => d.BeginFourWeekPeriodMonth()
                //}
            };
    }
}