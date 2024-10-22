using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using RubeesEat.IntegrationTests.PageObjects;

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
        
        public IWebElement FindElement(string id)
        {
            return WebTestElementFinder.FindElement(Me, id);
        }
        
        public void InvokeAction(string action)
        {
            WebTestElementFinder.InvokeAction(Me, action);
        }
        
        public TPageObject CreatePageObject<TPageObject>()
            where TPageObject : PageObject, new()
        {
            return Page.CreatePageObject<TPageObject>();
        }

        public ChromeDriver Driver { get; set; }
    }
}
