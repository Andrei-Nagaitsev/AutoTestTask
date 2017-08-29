using System;
using System.Globalization;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace testTask
{
    internal class PropertiesCollection
    {
        //Auto-Implemented Property
        public static string SourceCity = "Екатеринбург";
        public static string DistanationCity = "Каменск-Уральский";
        public static Days DayWeek = Days.Friday;
        public static DateTime MaxTime = DateTime.ParseExact("12:00", "HH:mm", CultureInfo.InvariantCulture);
        public static decimal MaxPrice = 200;
        public static IWebDriver Driver { get; set;}
        public static WebDriverWait Wait { get; set;}
    }
}
