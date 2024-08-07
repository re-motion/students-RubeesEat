using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace RubeesEat.IntegrationTests.WebTesting
{
    public static class WebElementExtensions
    {
        public static void ClickAndWaitForChanged<T>(this IWebElement webElement, Func<T> selector)
            where T : class
        {
            var before = selector();
            webElement.Click();

            WaitUntil(webElement, _ => before != selector());
        }

        public static void ClickAndWaitUntilStale(this IWebElement webElement)
        {
            webElement.Click();
            WaitUntil(webElement, e =>
            {
                try
                {
                    var attribute = e.GetAttribute("dummy");
                    return false;
                }
                catch (StaleElementReferenceException)
                {
                    return true;
                }
            });
        }

        public static string GetInnerHtml(this IWebElement webElement)
        {
            return webElement.GetAttribute("innerHTML");
        }

        private static void WaitUntil(IWebElement webElement, Func<IWebElement, bool> condition)
        {
            var wait = new DefaultWait<IWebElement>(webElement)
            {
                Timeout = TimeSpan.FromSeconds(3)
            };
            wait.Until(condition);
        }
    }
}
