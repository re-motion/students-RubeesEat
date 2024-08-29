using RubeesEat.IntegrationTests.PageObjects;
namespace RubeesEat.IntegrationTests.SeleniumTests;

[TestFixture]
public class SplitBillSeleniumTests() : SeleniumIntegrationTestBase("SplitBill")
{
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
        var person = page.ClickAddPerson();
        person.SetAmountForPerson("5");
        page.ClickSplitBill();
        
        Assert.That(page.GetPersonAmountAndNames(), Is.Empty);
    }
    
    [Test]
    public void InvalidTotalAmount()
    {
        var page = Start<SplitBillPageObject>();
        
        page.SetNewDesciptionText("Test");
        page.SetNewTotalPriceText("e");
        var person = page.ClickAddPerson();
        person.SetAmountForPerson("5.5");
        page.ClickSplitBill();
        
        Assert.That(page.GetErrorMessage(), Is.EqualTo("Please enter a valid number for total amount."));
    }
    
    [Test]
    public void NegativeTotalAmount()
    {
        var page = Start<SplitBillPageObject>();
        
        page.SetNewDesciptionText("Test");
        page.SetNewTotalPriceText("-10");
        var person = page.ClickAddPerson();
        person.SetAmountForPerson("10");
        page.ClickSplitBill();
        
        Assert.That(page.GetErrorMessage(), Is.EqualTo("-10 is not valid. Please enter a valid number for total amount."));
    }
    
    [Test]
    public void TotalAmountHasTooManyDigitsAfterComma()
    {
        var page = Start<SplitBillPageObject>();
        
        page.SetNewDesciptionText("Test");
        page.SetNewTotalPriceText("10.00002");
        var person = page.ClickAddPerson();
        person.SetAmountForPerson("10");
        page.ClickSplitBill();
        
        Assert.That(page.GetErrorMessage(), Is.EqualTo("10.00002 is not valid. Please only enter 2 digits after the comma."));
    }

    
    [Test]
    public void InvalidAmountPerPerson()
    {
        var page = Start<SplitBillPageObject>();
        
        page.SetNewDesciptionText("Test");
        page.SetNewTotalPriceText("5.5");
        var person = page.ClickAddPerson();
        person.SetAmountForPerson("e");
        page.ClickSplitBill();
        
        Assert.That(page.GetErrorMessage(), Is.EqualTo("Your input is not valid. Please enter a positive number"));
    }
    
    [Test]
    public void WrongAmountPerPerson()
    {
        var page = Start<SplitBillPageObject>();
        
        page.SetNewDesciptionText("Test");
        page.SetNewTotalPriceText("10");
        var person = page.ClickAddPerson();
        person.SetAmountForPerson("5");
        page.ClickSplitBill();
        
        Assert.That(page.GetErrorMessage(), Is.EqualTo("The sum of individual amounts doesn't match the total bill amount."));
    }
}

