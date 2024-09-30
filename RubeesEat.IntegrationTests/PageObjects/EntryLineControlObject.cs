using OpenQA.Selenium;
using RubeesEat.IntegrationTests.WebTesting;

namespace RubeesEat.IntegrationTests.PageObjects;

public class EntryLineControlObject : ControlObject
{
    public EntryLineControlObject(PageObject page, IWebElement me) : base(page, me)
    {
    }
    public string Person => FindElement("person").Text;
    public string AmountPerPerson => FindElement("amountPerPerson").Text;
    
}
