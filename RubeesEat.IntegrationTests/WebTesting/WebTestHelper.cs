using System.Configuration;
using System.Diagnostics;
using OpenQA.Selenium.Chrome;

namespace RubeesEat.IntegrationTests.WebTesting
{
    public class WebTestHelper : IDisposable
    {
        private ChromeDriver _chromeDriver;

        public WebTestHelper()
        {
            var chromeOptions = new ChromeOptions();

            if (!bool.TryParse(ConfigurationManager.AppSettings["Headless"], out var runHeadless))
                runHeadless = false;

            // For ease of debugging we disable headless automatically if a debugger is attached
            if (Debugger.IsAttached)
                runHeadless = false;

            if (runHeadless)
                chromeOptions.AddArgument("--headless=new");

            // Disable the choose a search engine screen that pops up
            chromeOptions.AddArgument("--disable-search-engine-choice-screen");

            _chromeDriver = new ChromeDriver(chromeOptions);
            DeleteAllCookies();
        }

        public void DeleteAllCookies()
        {
            _chromeDriver.Manage().Cookies.DeleteAllCookies();
        }

        public void Visit(string url)
        {
            _chromeDriver.Navigate().GoToUrl(url);
        }

        public void Dispose()
        {
            _chromeDriver.Dispose();
            _chromeDriver = null;
        }

        public TPageObject CreatePageObject<TPageObject>() where TPageObject : PageObject, new()
        {
            var pageObject = new TPageObject();
            pageObject.SetDriver(_chromeDriver);

            return pageObject;
        }
    }
}
