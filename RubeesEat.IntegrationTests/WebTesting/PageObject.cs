﻿using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace RubeesEat.IntegrationTests.WebTesting
{
    public abstract class PageObject
    {
        private ChromeDriver _driver;

        private ChromeDriver Driver => _driver ?? throw new InvalidOperationException("PageObject is not assigned to a driver.");

        internal void SetDriver(ChromeDriver driver)
        {
            _driver = driver;
        }

        public ChromeDriver GetDriver() => _driver;

        public IWebElement FindElement(By by)
        {
            return Driver.FindElement(by);
        }

        public IReadOnlyCollection<IWebElement> FindElements(By by)
        {
            return Driver.FindElements(by);
        }
    }
}