using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Threading;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using OpenQA.Selenium.Support.UI;

namespace testTask
{
    internal class SearchResultPageObject
    {
        public SearchResultPageObject()
        {
            PageFactory.InitElements(PropertiesCollection.Driver, this);
        }

        [FindsBy(How = How.XPath, Using = "(//header[contains(@class, 'SearchTitle')]/span)[1]")]
        public IWebElement AdressLabelElement { get; set; }

        [FindsBy(How = How.XPath, Using = "(//header[contains(@class, 'SearchTitle')]/span)[4]")]
        public IWebElement DateElement { get; set; }

        [FindsBy(How = How.XPath, Using = "(//section[contains(@class, 'SearchSegments')]/article)")]
        public IList<IWebElement> RouteEntryList { get; set; }

        [FindsBy(How = How.XPath, Using = "//div[select[option[text()='Р рубли']]]")]
        public IWebElement SelecterToggleElement { get; set; }

        [FindsBy(How = How.XPath, Using = ".//div[span[text()='Р рубли']]")]
        public IWebElement ToggleRubElement { get; set; }

        [FindsBy(How = How.XPath, Using = ".//div[span[text()='$ доллары']]")]
        public IWebElement ToggleUsdElement { get; set; }

        [FindsBy(How = How.XPath, Using = ".//div[select[option[text()='по времени отправления']]]")]
        public IWebElement ToggleOrderRuleElement { get; set; }

        [FindsBy(How = How.XPath, Using = ".//div[span[text()='по времени отправления']]")]
        public IWebElement BySourceTimeRuleElemente { get; set; }

        [FindsBy(How = How.XPath, Using = ".//button[contains(@class, 'Button SearchSorting__direction')]/span")]
        public IWebElement DirectionRuleElemente { get; set; }

        

        

        public void ToggleDownSortDirection()
        {
            if (DirectionRuleElemente.Text != "сначала ранние")
            {
                DirectionRuleElemente.Click();
            }
        }

        public void ToggleRub()
        {
            SelecterToggleElement.Click();
            PropertiesCollection.Wait.Until(
                ExpectedConditions.ElementIsVisible(By.XPath(".//div[span[text()='Р рубли']]")));
            ToggleRubElement.Click();
        }
        public void ToggleUsd()
        {
            PropertiesCollection.Wait.Until(
                ExpectedConditions.ElementToBeClickable(By.XPath("//div[select[option[text()='Р рубли']]]")));
            SelecterToggleElement.Click();
            PropertiesCollection.Wait.Until(
                ExpectedConditions.ElementIsVisible(By.XPath(".//div[span[text()='$ доллары']]")));
            ToggleUsdElement.Click();
        }
        public void ToggleBySourceTime()
        {
            PropertiesCollection.Wait.Until(
                ExpectedConditions.ElementToBeClickable(By.XPath("//div[select[option[text()='Р рубли']]]")));
            ToggleOrderRuleElement.Click();
            PropertiesCollection.Wait.Until(
                ExpectedConditions.ElementIsVisible(By.XPath(".//div[span[text()='по времени отправления']]")));
            BySourceTimeRuleElemente.Click();
        }

        public RouteInfoPageObject OpenInfoPage(RouteEntry routeEntry)
        {
            PropertiesCollection.Driver.Navigate().GoToUrl(routeEntry.LinkToInfoPage);
            return new RouteInfoPageObject();
        }

    }
}
