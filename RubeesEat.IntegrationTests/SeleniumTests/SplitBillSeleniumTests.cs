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

        var pageObject = page.ClickSplitBill();
        var balanceChanges = pageObject.GetBalanceChanges();

        Assert.That(balanceChanges[1].Description, Is.EqualTo("Test"));
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
        
        Assert.That(page.GetErrorMessage(), Is.EqualTo("Bitte eine gültige Zahl für den Gesamtbetrag eingeben."));
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
        
        Assert.That(page.GetErrorMessage(), Is.EqualTo("-10 ist ungültig. Bitte eine gültige Zahl für den Gesamtbetrag eingeben."));
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
        
        Assert.That(page.GetErrorMessage(), Is.EqualTo("10.00002 ist ungültig. Bitte nur 2 Nachkommastellen eingeben."));
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
        
        Assert.That(page.GetErrorMessage(), Is.EqualTo("Deine Eingabe ist ungültig. Bitte eine positive Zahl eingeben."));
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
        
        Assert.That(page.GetErrorMessage(), Is.EqualTo("Die Summe von den einzelnen Beträgen stimmt nicht mit dem Gesamtbetrag überein."));
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

    [Test]
    public void ShowPreviousBillParticipantAsButton()
    {
        var page = Start<SplitBillPageObject>();
        page.SetNewDesciptionText("Test");
        page.SetNewTotalPriceText("5");
        var person = page.ClickAddPerson();
        person.SetAmountForPerson("5");
        page.ClickSplitBill();
        page = Start<SplitBillPageObject>();
        var recentPeople = page.GetRecentPeople();
        var buttons = recentPeople.GetAllButtons();

        Assert.That(buttons[0].Me.Text, Is.EqualTo("DEFAULTFIRSTNAME DEFAULTLASTNAME"));
    }

    [Test]
    public void ClickPreviousBillParticipantAsButton_ButtonDisabledAndAddsPerson()
    {
        var page = Start<SplitBillPageObject>();
        page.SetNewDesciptionText("Test");
        page.SetNewTotalPriceText("5");
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
        page.SetNewTotalPriceText("5");
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
