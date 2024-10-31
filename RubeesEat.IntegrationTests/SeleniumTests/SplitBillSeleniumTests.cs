using RubeesEat.IntegrationTests.PageObjects;
namespace RubeesEat.IntegrationTests.SeleniumTests;

[TestFixture]
public class SplitBillSeleniumTests() : WithTestBills("SplitBill")
{
    [Test]
    public void AddPerson()
    {
        var page = Start<SplitBillPageObject>();
        
        page.ClickAddPerson();
        
        Assert.That(page.GetPersonAmountAndNames()[0], Is.EqualTo("DefaultFirstName DefaultLastName"));
    }
    
    [Test]
    public void InvalidAmountPerPerson()
    {
        var page = Start<SplitBillPageObject>();
        
        page.SetNewDesciptionText("Test");
        var person = page.ClickAddPerson();
        person.SetAmountForPerson("e");
        page.ClickSplitBill();
        
        Assert.That(page.GetErrorMessage(), Is.EqualTo("Deine Eingabe ist ungültig. Bitte eine positive Zahl eingeben."));
    }

    [Test]
    public void AddAndRemovePerson()
    {
        var page = Start<SplitBillPageObject>();
        
        var selectionsStart = page.GetSelection();
        var selectedPerson = selectionsStart[0];
        var expectedSelections = selectionsStart.Skip(1).ToList();
        
        var person = page.ClickAddPerson();
        
        Assert.That(page.GetPersonAmountAndNames().Length, Is.EqualTo(1));
        Assert.That(page.GetSelection(), Is.EqualTo(expectedSelections));
        
        person.ClickRemoveButton();
        expectedSelections.Add(selectedPerson);
        
        Assert.That(page.GetPersonAmountAndNames().Length, Is.EqualTo(0));
        Assert.That(page.GetSelection(), Is.EqualTo(expectedSelections));
    }

    [Test]
    public void OnlyShowActivePersons()
    {
        var page = Start<SplitBillPageObject>();
        
        var selectionsStart = page.GetSelection();
        
        Assert.That(selectionsStart.Count, Is.EqualTo(5));
        Assert.That(selectionsStart[0], Is.EqualTo("DefaultFirstName DefaultLastName"));
        Assert.That(selectionsStart[1], Is.EqualTo("Item Arslan"));
        Assert.That(selectionsStart[2], Is.EqualTo("Patrick Widener"));
        Assert.That(selectionsStart[3], Is.EqualTo("Lilli Grubber"));
        Assert.That(selectionsStart[4], Is.EqualTo("Mich Ludwig"));
    }
}
