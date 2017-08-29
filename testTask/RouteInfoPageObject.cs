using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

namespace testTask
{
    internal class RouteInfoPageObject
    {
        public RouteInfoPageObject()
        {
            PageFactory.InitElements(PropertiesCollection.Driver, this);
        }
        [FindsBy(How = How.XPath, Using = "//div[@class='b-page-title']/h1/span[2]")]
        public IWebElement RouteIndexElement { get; set; }

        [FindsBy(How = How.XPath, Using = "//div[@class='b-page-title']/h1/span[4]")]
        public IWebElement SourceCityElement { get; set; }

        [FindsBy(How = How.XPath, Using = "//div[@class='b-page-title']/h1/span[5]")]
        public IWebElement DistanationCityElement { get; set; }

        [FindsBy(How = How.XPath, Using = "//tr[td[div[@class='b-timetable__ico b-timetable__ico_type_start']]]")]
        public IWebElement SourceRouteEntry { get; set; }

        [FindsBy(How = How.XPath, Using = "//tr[td[div[@class='b-timetable__ico b-timetable__ico_type_finish']]]")]
        public IWebElement FinishRouteEntry { get; set; }

        

        
    }
}
