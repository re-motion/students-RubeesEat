using OpenQA.Selenium;
using RubeesEat.IntegrationTests.WebTesting;

namespace RubeesEat.IntegrationTests.PageObjects;

public class TodoControlObject(PageObject page, IWebElement me) : ControlObject(page, me)
{
    public string Text => Me.FindElement(By.CssSelector(".text")).Text;
    public bool IsChecked => FindElement(By.CssSelector(".checkbox")).Selected;

    public void ClickDeleteTodo()
    {
        FindElement(By.CssSelector(".deleteButton")).ClickAndWaitUntilStale();
    }

    public void ToggleChecked()
    {
        FindElement(By.CssSelector(".checkbox")).ClickAndWaitUntilStale();
    }
}
