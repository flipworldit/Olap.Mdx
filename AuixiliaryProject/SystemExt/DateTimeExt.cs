using System;
using System.Globalization;
using System.Linq;

namespace SystemExt
{
   public static partial class DateTimeExt
   {
      private static readonly GregorianCalendar GregorianCalendar = new GregorianCalendar();

      public static int ToInt(this DayOfWeek dayOfWeek)
      {
         return ((int) dayOfWeek + 6)%7;
      }

      public static bool IsPrevMonth(this DateTime date)
      {
         return date.AddMonths(1).BeginOfMonth() == DateTime.Now.BeginOfMonth();
      }

      public static DateTime BeginWeek(this DateTime date)
      {
         return date.Date
            .AddDays(-ToInt(date.DayOfWeek));
      }

      public static DateTime BeginQuarter(this DateTime date)
      {
         var quarterNumber = (date.Month - 1)/3 + 1;

         var firstDayOfQuarter = new DateTime(date.Year, (quarterNumber - 1)*3 + 1, 1);

         return firstDayOfQuarter;
      }

      public static DateTime EndQuarter(this DateTime date)
      {
         return date.BeginQuarter().AddMonths(3).AddDays(-1);
      }

      public static DateTime BeginHalfYear(this DateTime date)
      {
         var halfYearNumber = (date.Month - 1)/6 + 1;

         var firstDayOfHalfYear = new DateTime(date.Year, (halfYearNumber - 1)*6 + 1, 1);

         return firstDayOfHalfYear;
      }

      public static DateTime EndHalfYear(this DateTime date)
      {
         return date.BeginHalfYear().AddMonths(6).AddDays(-1);
      }

      public static DateTime BeginYear(this DateTime date)
      {
         return new DateTime(date.Year, 1, 1);
      }

      public static DateTime EndYear(this DateTime date)
      {
         return new DateTime(date.Year, 12, 31);
      }

      public static DateTime BeginProductionMonth(this DateTime date)
      {
         var beginProductionWeek = date.Date
            .AddDays(-ToInt(date.DayOfWeek));

         var weekCount = (date.ProductionWeekNumber() + 3)%4;

         return beginProductionWeek.AddDays(-weekCount*7);
      }

      public static DateTime BeginOfProductionMonth(int month, int year)
      {
         var date = new DateTime(year, 1, 1);

         var dayOfWeek = date.RussianDayOfWeek();

         date = dayOfWeek < 4 ? date.BeginWeek() : date.AddDays(7 - dayOfWeek);

         return date.AddDays((month - 1)*28);
      }

      public static int CountOfProductionMonthsInYear(int year)
      {
         var dateTime = new DateTime(year, 12, 31);

         var result = dateTime.ProductionMonthNumber();

         return result != 1 ? result : dateTime.AddDays(-7).ProductionMonthNumber();
      }

      public static DateTime EndProductionMonth(this DateTime date)
      {
         var beginProductionMonth = date.BeginProductionMonth();

         // Если это 53-я неделя (14-й месяц), то в этом месяце только 1 неделя, а не 4
         var countOfDays = CountOfDaysInProductionMonth(beginProductionMonth);

         return beginProductionMonth.AddDays(countOfDays).AddMilliseconds(-1);
      }

      public static int CountOfDaysInProductionMonth(this DateTime date)
      {
         return (date.ProductionMonthNumber() == 14) ? 7 : 28;
      }

      public static DateTime PrevProductionMonth(this DateTime date)
      {
         return date
            .BeginProductionMonth()
            .AddDays(-date.CountOfDaysInProductionMonth());
      }

      public static DateTime NextProductionMonth(this DateTime date)
      {
         return date
            .EndProductionMonth()
            .AddDays(1)
            .Date;
      }

      public static int CalendarWeekNumber(this DateTime date)
      {
         var dayOfWeek = (int) date.BeginYear().DayOfWeek - 1;

         dayOfWeek = dayOfWeek == -1 ? 6 : dayOfWeek;

         return (date.DayOfYear + dayOfWeek - 1)/7 + 1;
      }

      public static int RussianDayOfWeek(this DateTime date)
      {
         var dayOfWeek = (int) date.DayOfWeek - 1;

         dayOfWeek = dayOfWeek == -1 ? 6 : dayOfWeek;

         return dayOfWeek;
      }

      public static int ProductionWeekNumber(this DateTime date)
      {
         var calendarWeekNumber = date.CalendarWeekNumber();

         var dayOfWeekOfBeginYear = date.BeginYear().DayOfWeek;
         var dateEndPrevYear = date.BeginYear().AddDays(-1);
         var dayOfWeekOfBeginPrevYear = date.BeginYear().AddYears(-1).DayOfWeek;

         var firstFourDays = new[] {DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday, DayOfWeek.Thursday};
         var firstThreeDays = new[] {DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday};

         if (calendarWeekNumber > 1 && calendarWeekNumber < 53)
         {
            if (firstFourDays.Contains(dayOfWeekOfBeginYear))
            {
               return calendarWeekNumber;
            }

            return calendarWeekNumber - 1;
         }

         if (calendarWeekNumber == 1)
         {
            if (firstFourDays.Contains(dayOfWeekOfBeginYear))
            {
               return calendarWeekNumber;
            }

            if (firstFourDays.Contains(dayOfWeekOfBeginPrevYear))
            {
               return dateEndPrevYear.CalendarWeekNumber();
            }

            return dateEndPrevYear.CalendarWeekNumber() - 1;
         }

         if (firstFourDays.Contains(dayOfWeekOfBeginYear))
         {
            if (firstThreeDays.Contains(date.EndYear().DayOfWeek))
            {
               return 1;
            }

            return calendarWeekNumber;
         }

         return calendarWeekNumber - 1;
      }

      public static int ProductionMonthNumber(this DateTime date)
      {
         return ((date.ProductionWeekNumber() - 1)/4) + 1;
      }

      public static int ProductionYear(this DateTime date)
      {
         var calendarWeekNumber = date.CalendarWeekNumber();

         if (calendarWeekNumber > 1 && calendarWeekNumber < 53)
         {
            return date.Year;
         }

         var dayOfWeekOfBeginYear = date.BeginYear().DayOfWeek;

         var firstFourDays = new[] {DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday, DayOfWeek.Thursday};
         var firstThreeDays = new[] {DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday};

         if (calendarWeekNumber == 1)
         {
            if (firstFourDays.Contains(dayOfWeekOfBeginYear))
            {
               return date.Year;
            }
            return date.Year - 1;
         }

         if (firstFourDays.Contains(dayOfWeekOfBeginYear))
         {
            if (firstThreeDays.Contains(date.EndYear().DayOfWeek))
            {
               return date.Year + 1;
            }

            return date.Year;
         }

         return date.Year;
      }





      public static int FourWeekPeriodYear(this DateTime date)
      {
         int weekFW = date.FourWeekPeriodWeekNumber();
         int weekPr = date.ProductionWeekNumber();

         if ((weekFW - 1 == weekPr) || (weekFW == weekPr))
         {
            return date.ProductionYear();
         }
         else
         {
            return date.ProductionYear() + 1;
         }
      }

      public static int FourWeekPeriodMonthNumber(this DateTime date)
      {
         return (date.FourWeekPeriodWeekNumber() + 3)/4;
      }

      public static int FourWeekPeriodWeekNumber(this DateTime date)
      {
         return (date.ProductionWeekNumber() - 1 + GetLongWeekYearsCount(date))%52 + 1;
      }

      private static int GetLongWeekYearsCount(DateTime date)
      {
         int count = 0;

         int firstYear = 2014;
         int lastYear = date.ProductionYear();

         for (int i = firstYear; i < lastYear; i++)
         {
            if (CountOfProductionMonthsInYear(i) > 52)
            {
               count++;
            }
         }

         return count;
      }


      public static DateTime BeginFourWeekPeriodMonth(this DateTime date)
      {
         var beginFourWeekPeriodWeek = date.Date
            .AddDays(-ToInt(date.DayOfWeek));

         var weekCount = (date.FourWeekPeriodWeekNumber() + 3)%4;

         return beginFourWeekPeriodWeek.AddDays(-weekCount*7);
      }

      public static DateTime EndFourWeekPeriodMonth(this DateTime date)
      {
         var beginFourWeekPeriodMonth = date.BeginFourWeekPeriodMonth();

         return beginFourWeekPeriodMonth.AddDays(7*4).AddMilliseconds(-1);
      }

      public static DateTime StartDay(this DateTime date)
      {
         return date.Date.AddDays(-1).AddMilliseconds(+1);
      }



      public static DateTime EndWeek(this DateTime date)
      {
         return date.BeginWeek().AddDays(7).AddMilliseconds(-1);
      }

      public static DateTime EndDay(this DateTime date)
      {
         return date.Date.AddDays(1).AddMilliseconds(-1);
      }

      public static DateTime AddOneMonth(this DateTime date)
      {
         return date.AddMonths(1).AddMilliseconds(-1);
      }

      public static DateTime BeginCurrentMonth()
      {
         return DateTime.Today.AddDays(1 - DateTime.Today.Day);
      }

      public static DateTime BeginOfMonth(this DateTime date)
      {
         return new DateTime(date.Year, date.Month, 1);
      }

        public static DateTime BeginOfDay(this DateTime date)
        {
            return new DateTime(date.Year, date.Month, date.Day);
        }


      public static int GetQuarter(this DateTime date)
      {
         return (date.Month + 2)/3;
      }

      public static DateTime EndOfMonth(this DateTime date)
      {
         return date.BeginOfMonth().AddMonths(1).AddMilliseconds(-1);
      }

      public static Int64 ToUtcDateTimeJs(this DateTime date)
      {
         return
            Convert.ToInt64(
               (date - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds);
      }

      public static Int64 ToLocalDateTimeJs(this DateTime date)
      {
         return
            Convert.ToInt64(
               (date - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Local)).TotalMilliseconds);
      }


      public static DateTime RoundMinutes(this DateTime date, int step)
      {
         var minute = date.Minute;
         if (minute%step == 0)
            return date;

         date = date
            .AddMinutes(step - (minute%step));

         return date;
      }

      public static DateTime ParseJS(string strDate)
      {
         try
         {
            var dateTime = DateTime.ParseExact(
               strDate,
               "yyyy-MM-dd",
               CultureInfo.InvariantCulture);

            return dateTime;
         }
         catch (Exception exception)
         {
            throw new Exception("Не удалось распарсить значение '" + strDate + "' как дату. ", exception);
         }
      }

      public static string ToJsDateString(this DateTime date)
      {
         return date.ToString("MM.dd.yyyy HH:mm:ss");
      }

      public static string ToUtcString(this DateTime beginDate)
      {
         return beginDate.ToString("yyyy-MM-ddTHH:mm:ss.000Z");
      }

      public static int NumberOfWeeksToDate(this DateTime fromDate, DateTime toDate)
      {
         return (int) (toDate - fromDate).TotalDays/7;
      }

      public static int NumberOfMonthsToDate(this DateTime fromDate, DateTime toDate)
      {
         return (toDate.Month - fromDate.Month) + 12*(toDate.Year - fromDate.Year);
      }

      public static int GetHalfYear(this DateTime date)
      {
         return (date.Month > 6) ? 2 : 1;
      }

      public static int GetWeekOfMonth(this DateTime time)
      {
         var first = time.BeginOfMonth();
         return time.GetWeekOfYear() - first.GetWeekOfYear() + 1;
      }

      public static int GetWeekOfYear(this DateTime time)
      {
         return GregorianCalendar.GetWeekOfYear(time, CalendarWeekRule.FirstDay, DayOfWeek.Monday);
      }

      public static string ToShortTimeString(this DateTime datetime)
      {
         return datetime.ToString("hh:mm");
      }

      public static DateTime ParseProductionMonth(string date)
      {
         var dates = date.Split(new[] {'-'});

         var dateOfMonth = BeginOfProductionMonth(
            Convert.ToInt32(dates[0]),
            Convert.ToInt32(dates[1])
            );

         return dateOfMonth;
      }

      public static DateTime GetFirstDayOfQuarter(this DateTime date)
      {
         var quarterNumber = (date.Month - 1)/3 + 1;

         var month = (quarterNumber - 1)*3 + 1;

         return new DateTime(date.Year, month, 1);
      }

      public static DateTime GetLastDayOfQuarter(this DateTime date)
      {

         return date.GetFirstDayOfQuarter().AddMonths(3).AddDays(-1);
      }

      public static string DateToString(this DateTime dateTime)
      {
         return dateTime.ToString("yyyy-MM-dd");
      }

      public static string StartOfDay(this DateTime dateTime)
      {
         return dateTime.ToString("yyyy-MM-ddT00:00:00");
      }

      public static DateTime FromJsMonth(string month)
      {
         var monthDate = DateTime.Now;


         if (DateTime.TryParseExact(month, "MM/d/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None,
            out monthDate))
         {
         }
         else if (DateTime.TryParseExact(month, "M/d/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None,
            out monthDate))
         {

         }
         else if (DateTime.TryParseExact(month, "yyyy/M/d", CultureInfo.InvariantCulture, DateTimeStyles.None,
            out monthDate))
         {

         }
         else if (DateTime.TryParseExact(month, "yyyy/MM/d", CultureInfo.InvariantCulture, DateTimeStyles.None,
            out monthDate))
         {

         }

         monthDate = new DateTime(monthDate.Year, monthDate.Month, 1, 0, 0, 0);

         return monthDate;
      }


       public static int DaysInMonth(this DateTime reportDate)
       {
           return DateTime.DaysInMonth(reportDate.Year, reportDate.Month);
       }
   }
}