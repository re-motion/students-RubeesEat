using OpenQA.Selenium;

using RubeesEat.IntegrationTests.WebTesting;

namespace RubeesEat.IntegrationTests.PageObjects;

public class RenamePageObject : PageObject
{
    public void SetNewRenameText(string value)
    {
        FindElement(By.Id("renameTodo")).SendKeys(value);
    }

    public void ClickConfirmRename()
    {
        FindElement(By.Id("confirmRename")).ClickAndWaitUntilStale();
    }

    public void ClickConfirmRenameNotUntilStale()
    {
        FindElement(By.Id("confirmRename")).Click();
    }

    public void ClickCancelRename()
    {
        FindElement(By.Id("cancelRename")).ClickAndWaitUntilStale();
    }


}
