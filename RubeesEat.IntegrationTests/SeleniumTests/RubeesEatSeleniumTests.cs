using Microsoft.AspNetCore.Mvc.RazorPages;
using RubeesEat.IntegrationTests.PageObjects;

namespace RubeesEat.IntegrationTests.SeleniumTests;

[TestFixture]
public class RubeesEatSeleniumTests : SeleniumIntegrationTestBase
{
    public RubeesEatSeleniumTests() : base("SplitBill")
    {
    }

    [Test]
    public void AddPerson()
    {
        var page = Start<SplitBillPageObject>();
        
        page.ClickAddPerson();
        
        Assert.That(page.GetPersonAmountAndNames()[0], Is.EqualTo("DefaultFirstName DefaultLastName"));
    }

    [Test]
    public void CorrectInput()
    {
        var page = Start<SplitBillPageObject>();
        
        page.SetNewDesciptionText("Test");
        page.SetNewTotalPriceText("5");
        page.ClickAddPerson();
        var persons = page.GetPersonAmounts();
        persons[0].SetAmountForPerson("5");
        page.ClickSplitBill();
        
        Assert.That(page.GetPersonAmountAndNames(), Is.Empty);
    }
    
    [Test]
    public void WrongAmount()
    {
        var page = Start<SplitBillPageObject>();
        
        page.SetNewDesciptionText("Test");
        page.SetNewTotalPriceText("10");
        page.ClickAddPerson();
        var persons = page.GetPersonAmounts();
        persons[0].SetAmountForPerson("5");
        page.ClickSplitBill();
        
        Assert.That(page.GetErrorMessage(), Is.EqualTo("The sum of individual amounts doesn't match the total bill amount."));
    }

    [Test]
    public void EmptyDescription()
    {
        var page = Start<SplitBillPageObject>();
        
        page.SetNewTotalPriceText("10");
        page.ClickAddPerson();
        var persons = page.GetPersonAmounts();
        persons[0].SetAmountForPerson("10");
        page.ClickSplitBill();
        
        Assert.That(page.GetErrorMessage(), Is.EqualTo("Please enter a description."));
    }
    
    [Test]
    public void NoPersonAdded()
    {
        var page = Start<SplitBillPageObject>();
        
        page.SetNewDesciptionText("Test");
        page.SetNewTotalPriceText("10");
        page.ClickSplitBill();
        
        Assert.That(page.GetErrorMessage(), Is.EqualTo("Please add a person."));
    }
    
}
