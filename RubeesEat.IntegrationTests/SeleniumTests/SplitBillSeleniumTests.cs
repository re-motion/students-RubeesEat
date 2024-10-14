using RubeesEat.IntegrationTests.PageObjects;
namespace RubeesEat.IntegrationTests.SeleniumTests;

[TestFixture]
public class SplitBillSeleniumTests() : WithTestPersons("SplitBill")
{
    [Test]
    public void AddPerson()
    {
        var page = Start<SplitBillPageObject>();
        
        page.ClickAddPerson();
        
        Assert.That(page.GetPersonAmountAndNames()[0], Is.EqualTo("Patrick Widener"));
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
        
        Assert.That(selectionsStart.Count, Is.EqualTo(2));
        Assert.That(selectionsStart[0], Is.EqualTo("Patrick Widener"));
        Assert.That(selectionsStart[1], Is.EqualTo("Lilli Grubber"));
    }

    [Test]
    public void ShowPreviousBillParticipantAsButton()
    {
        var page = Start<SplitBillPageObject>();
        page.SetNewDesciptionText("Test");
        var person = page.ClickAddPerson();
        person.SetAmountForPerson("5");
        page.ClickSplitBill();
        page = Start<SplitBillPageObject>();
        var recentPeople = page.GetRecentPeople();
        var buttons = recentPeople.GetAllButtons();

        Assert.That(buttons[0].Me.Text, Is.EqualTo("DefaultFirstName DefaultLastName"));
    }

    [Test]
    public void ClickPreviousBillParticipantAsButton_ButtonDisabledAndAddsPerson()
    {
        var page = Start<SplitBillPageObject>();
        page.SetNewDesciptionText("Test");
        var person = page.ClickAddPerson();
        person.SetAmountForPerson("5");
        page.ClickSplitBill();
        page = Start<SplitBillPageObject>();
        var recentPeople = page.GetRecentPeople();
        var buttons = recentPeople.GetAllButtons();

        Assert.That(page.GetPersonAmounts().Length, Is.EqualTo(0));

        buttons[0].Me.Click();

        Assert.That(buttons[0].Me.Enabled, Is.False);
        Assert.That(page.GetPersonAmounts().Length, Is.EqualTo(1));
        Assert.That(page.GetPersonAmounts()[0].Me.Text, Is.EqualTo("DefaultFirstName DefaultLastName"));
    }

    [Test]
    public void RemovePreviousBillParticipantAsButton_ButtonEnabled()
    {
        var page = Start<SplitBillPageObject>();
        page.SetNewDesciptionText("Test");
        var person = page.ClickAddPerson();
        person.SetAmountForPerson("5");
        page.ClickSplitBill();
        page = Start<SplitBillPageObject>();
        var recentPeople = page.GetRecentPeople();
        var buttons = recentPeople.GetAllButtons();
        buttons[0].Me.Click();
        var lastPerson = page.GetPersons()[0];
        lastPerson.ClickRemoveButton();

        Assert.That(buttons[0].Me.Enabled, Is.True);
    }
}
