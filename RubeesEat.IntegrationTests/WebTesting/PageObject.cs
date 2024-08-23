using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

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
        Driver.SwitchTo().Window(Driver.WindowHandles.Last());
    }

    public void ToFirstWindow()
    {
        Driver.SwitchTo().Window(Driver.WindowHandles.First());
    }
}
