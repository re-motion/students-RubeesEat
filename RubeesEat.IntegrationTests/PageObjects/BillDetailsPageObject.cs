using OpenQA.Selenium;
using RubeesEat.IntegrationTests.WebTesting;
using RubeesEat.Model;

namespace RubeesEat.IntegrationTests.PageObjects;

public class BillDetailsPageObject : PageObject
{
    public string PersonWhoPaid => FindElement(By.CssSelector(".personWhoPayed")).Text;

    public string Description => FindElement(By.CssSelector(".description")).Text;

    public string Date => FindElement(By.CssSelector(".date")).Text;

    public string Amount => FindElement(By.CssSelector(".amount")).Text;

    public EntryLineControlObject[] GetEntryLines()
    {
        return FindElements(By.CssSelector(".entryLine"))
               .Select(e => new EntryLineControlObject(this, e))
               .ToArray();
    }
}
