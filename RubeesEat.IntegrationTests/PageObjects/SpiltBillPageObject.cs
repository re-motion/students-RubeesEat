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
        return FindElements(By.CssSelector("#placeForErrorMessage")).FirstOrDefault()?.Text;
    }

    public void SetNewDesciptionText(string value)
    {
        FindElement("billDescription").SendKeys(value);
    }

    public void SetNewTotalPriceText(string value)
    {
        FindElement(By.Id("billAmount")).SendKeys(value);
    }

    public PersonAmountControlObject ClickAddPerson()
    {
        FindElement(By.Id("addPersonButton")).Click(); 
        return new PersonAmountControlObject(this, 
            FindElement(By.CssSelector("#addedPeople > div:last-of-type")));
    }

    public void AddPerson()
    {
        InvokeAction("addPerson");
    }

    public void ClickSplitBill()
    {
        FindElement(By.Id("splitBillButton")).Click();
    }

    public PersonAmountControlObject[] GetPersonAmounts()
    {
        return FindElements(By.CssSelector("#addedPeople > div > .text"))
               .Select(e => new PersonAmountControlObject(this, e))
               .ToArray();
    }

    public List<string> GetSelection()
    {
        var selectElement = FindElement(By.Id("billPeople"));
        var options = selectElement.FindElements(By.TagName("option"));
        return options.Select(e => e.Text).ToList();
    }
}
