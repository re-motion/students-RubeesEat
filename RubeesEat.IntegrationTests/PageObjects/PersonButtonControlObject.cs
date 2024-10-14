using OpenQA.Selenium;
using RubeesEat.IntegrationTests.WebTesting;

namespace RubeesEat.IntegrationTests.PageObjects;

public class PersonButtonControlObject : ControlObject
{
    private readonly PageObject _page;

    public PersonButtonControlObject(PageObject page, IWebElement me) : base(page, me)
    {
        _page = page;
    }
    
    public string Text => Me.Text;
}
