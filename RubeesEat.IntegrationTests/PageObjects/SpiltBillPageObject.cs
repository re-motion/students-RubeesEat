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
        FindElement(By.Id("billDescription")).SendKeys(value);
    }

    public void SetNewTotalPriceText(string value)
    {
        FindElement(By.Id("billAmount")).SendKeys(value);
    }

    public void ClickAddPerson()
    {
        FindElement(By.Id("addPersonButton")).Click(); // QUESTION: Is it okay to use click if we stay on the same page
    }

    public void ClickSplitBill()
    {
        FindElement(By.Id("splitBillButton")).Click();
    }
    
    public PersonAmountControlObject[] GetPersonAmounts()
    {
        return FindElements(By.CssSelector("#addedPeople > span"))
               .Select(e => new PersonAmountControlObject(this, e))
               .ToArray();
    }
}
