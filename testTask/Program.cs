using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Threading;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

namespace testTask
{
    public class Program
    {
        public static void Main(string[] args)
        {
        }
        [SetUp]
        public void Initialize()
        {
            PropertiesCollection.Driver = new ChromeDriver();
            PropertiesCollection.Wait = new WebDriverWait(PropertiesCollection.Driver, TimeSpan.FromSeconds(5));
            //Navigate to yandex page
            
        }

        [TestCase(200, "12:00", "Екатеринбург", "Каменск-Уральский", Days.Saturday)]
        //[TestCase(200, "12:00", "Екатеринбург", "Каменск-Уральский", Days.Friday)]
        //[TestCase(200, "12:00", "Екатеринбург", "Нижний Тагил", Days.Friday)]
        public void ExecuteTest(decimal maxCost, string time, string sourceCity, string distanationCity, Days day)
        {
            PropertiesCollection.MaxPrice = maxCost;
            PropertiesCollection.MaxTime = Helper.ParseSourceDistanationTime(time);
            PropertiesCollection.SourceCity = sourceCity;
            PropertiesCollection.DistanationCity = distanationCity;
            PropertiesCollection.DayWeek = day;
            //Test episode 1
            PropertiesCollection.Driver.Navigate().GoToUrl("http://yandex.ru");
            Console.WriteLine("Открыта страница yandex.ru (Заглавная страница)");

            //Test episode 2
            var yandexPage = new YandexPageObject();
            var schedulePage = yandexPage.NavigateToSchedule();
            Console.WriteLine("Осуществлен переход rasp.yandex.ru (страница расписания)");
            
            //Test episode 3
            schedulePage.SetTrain();
            Console.WriteLine("Выбран поиск по электричкам");

            schedulePage.SetFromCity(PropertiesCollection.SourceCity);
            Console.WriteLine("Выбран город отправления");

            schedulePage.SetToCity(PropertiesCollection.DistanationCity);
            Console.WriteLine("Выбран город прибытия");

            schedulePage.SetNearestDateByDayWeek(PropertiesCollection.DayWeek);
            Console.WriteLine("Выбран день недели");

            var searchResultPage = schedulePage.Search();
            Console.WriteLine("Поиск осуществлен");

            IsSearchTableHeaderCorrect(PropertiesCollection.SourceCity, PropertiesCollection.DistanationCity, searchResultPage);
            Console.WriteLine("Заголовок таблицы результатов совпадает с параметрами поиска");

            var searchResilt = GetSearchResult(searchResultPage);
            Console.WriteLine("Сохранены результаты удовлетворяющие условиям поиска");

            //Test episode 6
            if (searchResilt.Count == 0)
            {
                Console.WriteLine("В расписании отсутствуют рейсы удовлетворяющие параметрам поиска");
                return;
            }
            Console.WriteLine("Вывод результатов поиска (самый ранний рейс)");
            OutputResult(searchResilt);
            
            //Test episode 7
            var routeInfoPage = searchResultPage.OpenInfoPage(searchResilt[0]);
            InfoIsCorrect(searchResilt[0], routeInfoPage);

            Console.WriteLine("Тест пройден");
            Console.WriteLine("*********");

        }
        
        [TearDown]
        public void CleanUp()
        {
            PropertiesCollection.Driver.Quit();
        }

        private static void OutputResult(IReadOnlyList<RouteEntry> searchResilt)
        {
            Console.WriteLine(
                $"Самый ранний рейс удовлетворяющий условиям поиска: {searchResilt[0].SourceCity}" +
                $" - {searchResilt[0].DistanationCity}, Время отправления: {searchResilt[0].SourceTime.ToShortTimeString()}," +
                $" цена в рублях: {searchResilt[0].PriceRu}, цена в долларах: {searchResilt[0].PriceUs}.");
        }

        private static List<RouteEntry> GetSearchResult(SearchResultPageObject searchResult)
        {
            var routeEntryListParsed = new List<RouteEntry>();
            Thread.Sleep(3000);

            searchResult.ToggleBySourceTime();
            searchResult.ToggleDownSortDirection();

            foreach (var routeEntryElement in searchResult.RouteEntryList)
            {
                var routeEntry = ParseEntry(routeEntryElement, searchResult);


                if (routeEntry.SourceTime < PropertiesCollection.MaxTime &&
                    routeEntry.PriceRu < PropertiesCollection.MaxPrice)
                {
                    routeEntryListParsed.Add(routeEntry);
                    break;
                }
                if (routeEntry.SourceTime >= PropertiesCollection.MaxTime)
                {
                    break;
                }
            }
            return routeEntryListParsed;
        }

        private static RouteEntry ParseEntry(ISearchContext routeEntryElement, SearchResultPageObject searchResult)
        {
            //SetUp regexp for source-distanation field
            var sourceAndDistanationExp = new Regex("(.*?) — (.*)");
            //SetUp regexp for priceRUB
            var priceExpRub = new Regex("(.*?) .*");
            //SetUp regexp for priceUSD
            var priceExpUsd = new Regex("[$](.*)$");

            //SetUp order of entry

            //Take fields with data
            var header = routeEntryElement.FindElement(By.XPath("(./header)"));
            var timeAndStations = routeEntryElement.FindElement(By.XPath("(./div)[1]"));
            var scheduleAndPrices = routeEntryElement.FindElement(By.XPath("(./div)[2]"));

            //Create result Entry
            var routeEntry = new RouteEntry { RouteIndex = header.FindElement(By.XPath("./h3/a/span[1]")).Text };

            //Parse route index

            //Parse source and distanation
            var sourceAndDistanationText = header.FindElement(By.XPath("./h3/a/span[2]")).Text;
            var sourceAndDistanationGroup = sourceAndDistanationExp.Match(sourceAndDistanationText);
            routeEntry.SourceCity = sourceAndDistanationGroup.Groups[1].Value;
            routeEntry.DistanationCity = sourceAndDistanationGroup.Groups[2].Value;

            //Parse link to info page
            routeEntry.LinkToInfoPage = header.FindElement(By.XPath("./h3/a")).GetAttribute("href");

            //Parse times
            routeEntry.SourceTime = Helper.ParseSourceDistanationTime(timeAndStations.FindElement(By.XPath("((./div)[1]/div)[1]/span")).Text);
            routeEntry.TravelTime = Helper.ParseTravelTime(timeAndStations.FindElement(By.XPath("((./div)[1]/div)[2]/span")).Text);
            routeEntry.DistanationTime = Helper.ParseSourceDistanationTime(timeAndStations.FindElement(By.XPath("((./div)[1]/div)[3]/span")).Text);
            //Parse Ru prices
            searchResult.ToggleRub();
            var priceTextRub = scheduleAndPrices.FindElement(By.XPath("./div/button/span/span")).Text;
            var priceGroupRub = priceExpRub.Match(priceTextRub);
            routeEntry.PriceRu = decimal.Parse(priceGroupRub.Groups[1].Value, CultureInfo.InvariantCulture);
            Thread.Sleep(500);


            //Parse Us price
            searchResult.ToggleUsd();
            var priceTextUsd = scheduleAndPrices.FindElement(By.XPath("./div/button/span/span")).Text;
            var priceGroupUsd = priceExpUsd.Match(priceTextUsd);
            routeEntry.PriceUs = decimal.Parse(priceGroupUsd.Groups[1].Value, CultureInfo.InvariantCulture);


            //Console.Write(routeEntry.PriceUs);
            return routeEntry;
        }
        private static void IsSearchTableHeaderCorrect(string sourceCity, string distanationCity, SearchResultPageObject searchResult)
        {
            var text = searchResult.AdressLabelElement.Text;
            var regex = new Regex($".*?{sourceCity}.*?{distanationCity}.*?");
            Assert.AreEqual(regex.IsMatch(text), true, "Заголовок таблицы не совпал с ожидаемым");
        }

        private static void InfoIsCorrect(RouteEntry routeEntry, RouteInfoPageObject infoPage)
        {
            IsRouteTableHeaderCorrect(routeEntry, infoPage);
            IsRouteTableBodyCorrect(routeEntry, infoPage);
        }
        private static void IsRouteTableHeaderCorrect(RouteEntry routeEntry, RouteInfoPageObject infoPage)
        {
            Assert.AreEqual(infoPage.RouteIndexElement.Text, routeEntry.RouteIndex);
            Assert.AreEqual(infoPage.SourceCityElement.Text, routeEntry.SourceCity);
            Assert.AreEqual(infoPage.DistanationCityElement.Text, routeEntry.DistanationCity);
        }
        private static void IsRouteTableBodyCorrect(RouteEntry routeEntry, RouteInfoPageObject infoPage)
        {
            var distanationCity = infoPage.FinishRouteEntry.FindElement(By.XPath(".//td[1]/div[2]/a")).Text;
            Assert.AreEqual(distanationCity, PropertiesCollection.DistanationCity);
            var distanationCityTime = Helper.ParseSourceDistanationTime(infoPage.FinishRouteEntry.FindElement(By.XPath(".//td[2]/span")).Text);
            Assert.AreEqual(distanationCityTime, routeEntry.DistanationTime);
            var travelTime = Helper.ParseTravelTime(infoPage.FinishRouteEntry.FindElement(By.XPath(".//td[5]/div")).Text);
            Assert.AreEqual(travelTime, routeEntry.TravelTime);
            var sourceCity = infoPage.SourceRouteEntry.FindElement(By.XPath(".//td[1]/div[2]/a")).Text;
            Assert.AreEqual(sourceCity, routeEntry.SourceCity);
            var sourceCityTime = Helper.ParseSourceDistanationTime(infoPage.SourceRouteEntry.FindElement(By.XPath(".//td[4]/span/span/strong")).Text);
            Assert.AreEqual(sourceCityTime, routeEntry.SourceTime);


        }
    }
}
