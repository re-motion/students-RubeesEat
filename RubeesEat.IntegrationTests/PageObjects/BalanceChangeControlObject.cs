using OpenQA.Selenium;
using RubeesEat.IntegrationTests.WebTesting;

namespace RubeesEat.IntegrationTests.PageObjects;

public class BalanceChangeControlObject : ControlObject
{
    public BalanceChangeControlObject(PageObject page, IWebElement me) : base(page, me)
    {
    }
    
    public string Amount => Me.FindElement(By.CssSelector(".amount")).Text;
    public string Date => Me.FindElement(By.CssSelector(".dateHomepage")).Text;
    public string Description => Me.FindElement(By.CssSelector(".descriptionHomepage")).Text;
    public string Text => Me.Text;
    
    public BillDetailsPageObject ClickBillDetails()
    {
        FindElement(By.CssSelector(".clickBillDetails")).ClickAndWaitUntilStale();
        return CreatePageObject<BillDetailsPageObject>();
    }
}
