using OpenQA.Selenium;
using RubeesEat.IntegrationTests.WebTesting;

namespace RubeesEat.IntegrationTests.PageObjects;

public class ShowBillsControlObject(PageObject page, IWebElement me) : ControlObject(page, me)
{
    public string Text => Me.FindElement(By.CssSelector(".clickBillDetails")).Text;

    public void ClickOnBill()
    {
        Me.FindElement(By.CssSelector(".clickBillDetails")).ClickAndWaitUntilStale();
    }
}
