using OpenQA.Selenium;
using RubeesEat.IntegrationTests.WebTesting;

namespace RubeesEat.IntegrationTests.PageObjects;

public class TodoControlObject : ControlObject
{
    public TodoControlObject(PageObject page, IWebElement me) : base(page, me)
    {
    }

    public string Text => Me.Text;
}
