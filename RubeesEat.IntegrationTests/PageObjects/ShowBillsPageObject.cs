using OpenQA.Selenium;
using RubeesEat.IntegrationTests.WebTesting;

namespace RubeesEat.IntegrationTests.PageObjects;

public class ShowBillsPageObject : PageObject
{
    public BalanceChangeControlObject[] GetBalanceChanges()
    {
        return FindElements("entryLine")
               .Select(e => new BalanceChangeControlObject(this, e))
               .ToArray();
    }

    public void ClickPrevious()
    {
        InvokeAction("prevButton");
    }
    
    public void ClickNext()
    {
        InvokeAction("nextButton");

    }
}
