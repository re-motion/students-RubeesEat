using OpenQA.Selenium;
using RubeesEat.IntegrationTests.WebTesting;

namespace RubeesEat.IntegrationTests.PageObjects;

public class PersonControlObject : ControlObject
{
    public PersonControlObject(PageObject page, IWebElement me) : base(page, me)
    {
    }

    public string Text => Me.Text;
    
    public void ClickRemoveButton()
    {
        InvokeAction("removePeople");
    }
}
