using OpenQA.Selenium;
using RubeesEat.IntegrationTests.WebTesting;

namespace RubeesEat.IntegrationTests.PageObjects;

public class RecentPeopleControlObject : ControlObject
{
    private PageObject _page;

    public RecentPeopleControlObject(PageObject page, IWebElement me) : base(page, me)
    {
        _page = page;
    }

    public PersonButtonControlObject[] GetAllButtons()
    {
        return FindElements("addPersonButton")
               .Select(e => new PersonButtonControlObject(_page, e))
               .ToArray();
    }
}
