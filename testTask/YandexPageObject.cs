using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

namespace testTask
{
    internal class YandexPageObject
    {
        public YandexPageObject()
        {
            PageFactory.InitElements(PropertiesCollection.Driver, this);
        }

        [FindsBy(How = How.LinkText, Using = "Расписания")]
        public IWebElement LinkToScheduleElment { get; set; }

        public SchedulePageObject NavigateToSchedule()
        {
            LinkToScheduleElment.Click();
            return new SchedulePageObject();
        }
    }
}
