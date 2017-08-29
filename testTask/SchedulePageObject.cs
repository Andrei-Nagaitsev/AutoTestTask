using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

namespace testTask
{
    internal class SchedulePageObject
    {
        public SchedulePageObject()
        {
            PageFactory.InitElements(PropertiesCollection.Driver, this);
        }

        [FindsBy(How = How.XPath, Using = ".//label[text()='Электричка']")]
        public IWebElement TrainButtonElement { get; set; }

        [FindsBy(How = How.Name, Using = "fromName")]
        public IWebElement FromCityElement { get; set; }

        [FindsBy(How = How.Name, Using = "toName")]
        public IWebElement ToCityElement { get; set; }

        [FindsBy(How = How.XPath, Using = "(.//label[text()='Когда']/following-sibling::div//input)[1]")]
        public IWebElement DateElement { get; set; }

        [FindsBy(How = How.XPath, Using = ".//button[span[text()='Найти']]")]
        public IWebElement SearchButtonElement { get; set; }

        
        public void SetTrain()
        {
            TrainButtonElement.Click();
        }
        public void SetFromCity(string fromCity)
        {
            FromCityElement.Clear();
            FromCityElement.SendKeys(fromCity);
        }

        public void SetToCity(string toCity)
        {
            ToCityElement.Clear();
            ToCityElement.SendKeys(toCity);
        }

        public void SetNearestDateByDayWeek(Days day)
        {
            var date = Helper.GetNearestDay(day);
            DateElement.SendKeys(date.ToShortDateString());
        }

        public SearchResultPageObject Search()
        {
            SearchButtonElement.Click();
            return new SearchResultPageObject();
        }
    }
}
