using System;
using System.Globalization;

namespace testTask
{
    public enum Days
    {
        Sunday = 0,
        Monday = 1,
        Tuesday = 2,
        Wednesday = 3,
        Thursday = 4,
        Friday = 5,
        Saturday = 6
    }
    class Helper
    {
        public static DateTime GetNearestDay(Days day)
        {
            var dateNow = DateTime.Now;
            var dayWeek = dateNow.DayOfWeek;
            var variance = ((int)day - (int)dayWeek);
            var addition = variance > 0
                ? variance
                : 7 + variance;
            var firstWeekDay = dateNow.AddDays(addition);
            return firstWeekDay;
        }

        public static DateTime ParseTravelTime(string time)
        {
            //SetUp travel time pattern
            var patternTravelTime = "H ч m мин";
            return DateTime.ParseExact(time, patternTravelTime, CultureInfo.InvariantCulture);
        }
        public static DateTime ParseSourceDistanationTime(string time)
        {
            //SetUp travel time pattern
            var patternTravelTime = "HH:mm";
            return DateTime.ParseExact(time, patternTravelTime, CultureInfo.InvariantCulture);
        }
    }
}
