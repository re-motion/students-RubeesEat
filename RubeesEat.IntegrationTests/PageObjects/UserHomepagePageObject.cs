using OpenQA.Selenium;
using RubeesEat.IntegrationTests.WebTesting;

namespace RubeesEat.IntegrationTests.PageObjects;

public class UserHomepagePageObject : PageObject
{
    public string[] GetBalanceChangeTexts()
    {
        return GetBalanceChanges()
               .Select(e => e.Text)
               .ToArray();
    }

    public BalanceChangeControlObject[] GetBalanceChanges()
    {
        return FindElements(By.CssSelector("#balanceChanges > li"))
               .Select(e => new BalanceChangeControlObject(this, e))
               .ToArray();
    }

    public string GetAccountBalance()
    {
        return FindElement(By.Id("balance")).Text;
    }
}
