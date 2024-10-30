using RubeesEat.IntegrationTests.PageObjects;

namespace RubeesEat.IntegrationTests.SeleniumTests;

[TestFixture]
public class RubeesEatSeleniumTests() : WithTestBills("Index")
{
    [Test]
    public void GetDefaultBalanceChanges()
    {
        var page = Start<UserHomepagePageObject>();
        var balanceChanges = page.GetBalanceChanges();

        Assert.That(balanceChanges[0].Amount, Is.EqualTo("1 000,00 €"));
        Assert.That(balanceChanges[0].Date, Is.EqualTo("Dienstag, 19. August 2025"));
        Assert.That(balanceChanges[0].Description, Is.EqualTo("Default user paid lunch 2025"));
        
        Assert.That(balanceChanges[1].Amount, Is.EqualTo("1 000 000,50 €"));
        Assert.That(balanceChanges[1].Date, Is.EqualTo("Montag, 9. September 2024"));
        Assert.That(balanceChanges[1].Description, Is.EqualTo("Mittagessen auf Patrick sein Nacken"));

        Assert.That(balanceChanges[2].Amount, Is.EqualTo("-800,00 €"));
        Assert.That(balanceChanges[2].Date, Is.EqualTo("Sonntag, 28. April 2024"));
        Assert.That(balanceChanges[2].Description, Is.EqualTo("Lunch with default user in 2024"));

        Assert.That(balanceChanges[3].Amount, Is.EqualTo("-800,00 €"));
        Assert.That(balanceChanges[3].Date, Is.EqualTo("Freitag, 28. April 2023"));
        Assert.That(balanceChanges[3].Description, Is.EqualTo("Lunch with default user in 2023"));

        Assert.That(balanceChanges[4].Amount, Is.EqualTo("-800,00 €"));
        Assert.That(balanceChanges[4].Date, Is.EqualTo("Donnerstag, 28. April 2022"));
        Assert.That(balanceChanges[4].Description, Is.EqualTo("Lunch with default user 2022"));
    }
    
    [Test]
    public void ClickBalanceChange()
    {
        var page = Start<UserHomepagePageObject>();
        var balanceChanges = page.GetBalanceChanges();
        var balanceChange = balanceChanges[0];
        using var billDetailPage = balanceChange.ClickBillDetails();

        Assert.That(billDetailPage.PersonWhoPaid, Is.EqualTo("DefaultFirstName DefaultLastName"));
        Assert.That(billDetailPage.Description, Is.EqualTo("Default user paid lunch 2025"));
        Assert.That(billDetailPage.Amount, Is.EqualTo("3 000,00 €"));
        Assert.That(billDetailPage.Date, Is.AnyOf("Dienstag, 19. August 2025", "Tuesday, 19 August 2025", "19 August 2025"));
        Assert.That(billDetailPage.GetEntryLines()[0].Person, Is.EqualTo("DefaultFirstName DefaultLastName:"));
        Assert.That(billDetailPage.GetEntryLines()[0].AmountPerPerson, Is.EqualTo("2 000,00 €"));
        Assert.That(billDetailPage.GetEntryLines()[1].Person, Is.EqualTo("Patrick Widener:"));
        Assert.That(billDetailPage.GetEntryLines()[1].AmountPerPerson, Is.EqualTo("500,00 €"));
        Assert.That(billDetailPage.GetEntryLines()[2].Person, Is.EqualTo("Lilli Grubber:"));
        Assert.That(billDetailPage.GetEntryLines()[2].AmountPerPerson, Is.EqualTo("500,00 €"));
    }

    [Test]
    public void GetAccountBalance()
    {
        var page = Start<UserHomepagePageObject>();
        Assert.That(
            page.GetAccountBalance(),
            Is.EqualTo("Dein Guthaben: 998 600,50 €"));
    }
}
