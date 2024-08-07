using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace RubeesEat.IntegrationTests.WebTesting
{
    public abstract class ControlObject
    {
        public PageObject Page { get; }

        public IWebElement Me { get; }

        protected ControlObject(PageObject page, IWebElement me)
        {
            Page = page;
            Me = me;
        }

        public ChromeDriver GetDriver() => Page.GetDriver();

        public IWebElement FindElement(By by)
        {
            return Me.FindElement(by);
        }

        public IReadOnlyCollection<IWebElement> FindElements(By by)
        {
            return Me.FindElements(by);
        }
    }
}
