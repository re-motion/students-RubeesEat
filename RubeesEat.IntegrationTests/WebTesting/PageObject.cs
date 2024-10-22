using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using RubeesEat.IntegrationTests.PageObjects;

namespace RubeesEat.IntegrationTests.WebTesting;

public abstract class PageObject : IDisposable
{
    private ChromeDriver _driver;

    public ChromeDriver Driver =>
        _driver ?? throw new InvalidOperationException("PageObject is not assigned to a driver.");

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
    
    public IWebElement FindElement(string id)
    {
        return WebTestElementFinder.FindElement(Driver, id);
    }
    
    public IReadOnlyCollection<IWebElement> FindElements(string id)
    {
        return WebTestElementFinder.FindElements(Driver, id);
    }

    public void InvokeAction(string action)
    {
        WebTestElementFinder.InvokeAction(Driver, action);
    }

    public void Refresh()
    {
        Driver.Navigate().Refresh();
    }

    public void Dispose()
    {
    }

    public void NewWindow()
    {
        Driver.SwitchTo().NewWindow(WindowType.Tab);
    }

    public void ToFirstWindow()
    {
        Driver.SwitchTo().Window(Driver.WindowHandles.First());
    }
    
    public string GetCurrentUrl()
    {
        return Driver.Url;
    }

    public TPageObject CreatePageObject<TPageObject>()
        where TPageObject : PageObject, new()
    {
        var page = new TPageObject();
        page.SetDriver(_driver);

        return page;
    }
}
