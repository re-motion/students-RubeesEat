using OpenQA.Selenium;
using RubeesEat.IntegrationTests.WebTesting;

namespace RubeesEat.IntegrationTests.PageObjects;

public class PersonAmountControlObject : ControlObject
{
    public PersonAmountControlObject(PageObject page, IWebElement me) : base(page, me)
    {
    }

    public string Text => Me.Text;

    public void SetAmountForPerson(string value)
    {
        var element = FindElement("input");
        element.Clear();
        element.SendKeys(value);
    }

    public void ClickRemoveButton()
    {
        InvokeAction("removePeople");
    }
}
