using OpenQA.Selenium;
using RubeesEat.IntegrationTests.WebTesting;

namespace RubeesEat.IntegrationTests.PageObjects;

public class BalanceChangeControlObject : ControlObject
{
    public BalanceChangeControlObject(PageObject page, IWebElement me) : base(page, me)
    {
    }
    
    public string Amount => FindElement("amount").Text;
    public string Date => FindElement("date").Text;
    public string Description => FindElement("description").Text;
    public string Text => Me.Text;
    
    public BillDetailsPageObject ClickBillDetails()
    {
        InvokeAction("showBillDetails");
        return CreatePageObject<BillDetailsPageObject>();
    }
}
