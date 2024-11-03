
using System.Globalization;

namespace AirshowSchedules;
    public class cCalenderYear
    {

        public class AirshowWeekend
        {
            public DateTime Date;

            public AirshowWeekend(DateTime date)
            {
                Date = date;
            }


            public override string ToString()
            {
                return $"{Date.Month} - {Date.Day}";
            }

            public int weekofyear
            {
                get
                {
                    // Get the week number using the current culture's calendar
                    CultureInfo culture = CultureInfo.CurrentCulture;
                    Calendar calendar = culture.Calendar;
                    int weekNumber = calendar.GetWeekOfYear(Date, culture.DateTimeFormat.CalendarWeekRule, culture.DateTimeFormat.FirstDayOfWeek);
                    return weekNumber;
                }
            }
        }

        public static List<AirshowWeekend> GetSaturdaysList(int year)
        {
            // Create a list to store the Saturdays.
            List<DateTime> saturdays = new List<DateTime>();

            // Loop through the months in the year.
            for (int month = 1; month <= 12; month++)
            {
                // Get the first day of the month.
                DateTime firstDayOfMonth = new DateTime(year, month, 1);

                // Check if the first day of the month is a Saturday.
                if (firstDayOfMonth.DayOfWeek == DayOfWeek.Saturday)
                {
                    // Add the first day of the month to the list of Saturdays.
                    saturdays.Add(firstDayOfMonth);
                }

                // Loop through the days of the month.
                for (int day = 2; day <= DateTime.DaysInMonth(year, month); day++)
                {
                    // Get the current day.
                    DateTime currentDay = new DateTime(year, month, day);

                    // Check if the current day is a Saturday.
                    if (currentDay.DayOfWeek == DayOfWeek.Saturday)
                    {
                        // Add the current day to the list of Saturdays.
                        saturdays.Add(currentDay);
                    }
                }
            }

            List<AirshowWeekend> weekends = new List<AirshowWeekend>();
            for (int ii = 0; ii < saturdays.Count; ii++)
            {
                weekends.Add(new AirshowWeekend(saturdays[ii]));
            }

            return weekends;
        }
    }