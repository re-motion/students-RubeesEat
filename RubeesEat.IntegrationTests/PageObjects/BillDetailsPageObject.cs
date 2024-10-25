using OpenQA.Selenium;
using RubeesEat.IntegrationTests.WebTesting;
using RubeesEat.Model;

namespace RubeesEat.IntegrationTests.PageObjects;

public class BillDetailsPageObject : PageObject
{
    public string PersonWhoPaid => FindElement("personWhoPaid").Text;

    public string Description => FindElement("description").Text;

    public string Date => FindElement("date").Text;

    public string Amount => FindElement("amount").Text;

    public EntryLineControlObject[] GetEntryLines()
    {
        return FindElements("entryLine")
               .Select(e => new EntryLineControlObject(this, e))
               .ToArray();
    }
    
    public EditBillPageObject ClickEdit()
    {
        InvokeAction("editBillButton");
        return CreatePageObject<EditBillPageObject>();
    }
}
