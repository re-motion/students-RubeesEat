using OpenQA.Selenium;
using RubeesEat.IntegrationTests.WebTesting;

namespace RubeesEat.IntegrationTests.PageObjects;

public class TodoControlObject : ControlObject
{
    public TodoControlObject(PageObject page, IWebElement me) : base(page, me)
    {
    }

    public string Text => Me.Text;

    public bool IsChecked => FindElement(By.CssSelector(".checkbox")).Selected;

    public void ToggleChecked()
    {
        FindElement(By.CssSelector(".checkbox")).ClickAndWaitUntilStale();
    }
}
