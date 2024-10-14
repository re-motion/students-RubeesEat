using OpenQA.Selenium;
using RubeesEat.IntegrationTests.WebTesting;

namespace RubeesEat.IntegrationTests.PageObjects;

public class SplitBillPageObject : PageObject
{
    public string[] GetPersonAmountAndNames()
    {
        return GetPersonAmounts()
               .Select(e => e.Text)
               .ToArray();
    }

    public string? GetErrorMessage()
    {
        return FindElements("placeForErrorMessage").FirstOrDefault()?.Text;
    }

    public void SetNewDesciptionText(string value)
    {
        FindElement("billDescription").SendKeys(value);
    }

    public PersonAmountControlObject ClickAddPerson()
    {
        InvokeAction("addPerson"); 
        return new PersonAmountControlObject(this, 
            FindElement("addedPeople").FindElement(By.CssSelector("div:last-of-type")));
    }

    public UserHomepagePageObject ClickSplitBill()
    {
        InvokeAction("splitBill");
        return CreatePageObject<UserHomepagePageObject>();
    }

    public PersonAmountControlObject[] GetPersonAmounts()
    {
        return FindElement("addedPeople").FindElements(By.CssSelector("div > .text"))
               .Select(e => new PersonAmountControlObject(this, e))
               .ToArray();
    }
    
    public PersonControlObject[] GetPersons()
    {
        return FindElement("addedPeople").FindElements(By.CssSelector("div"))
                                         .Select(e => new PersonControlObject(this, e))
                                         .ToArray();
    }

    public List<string> GetSelection()
    {
        var selectElement = FindElement("billPeople");
        var options = selectElement.FindElements(By.TagName("option"));
        return options.Select(e => e.Text).ToList();
    }

    public RecentPeopleControlObject GetRecentPeople()
    {
        return new RecentPeopleControlObject(this, FindElement("recentPeople"));
    }
}
