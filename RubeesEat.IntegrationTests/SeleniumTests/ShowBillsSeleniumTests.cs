using RubeesEat.IntegrationTests.PageObjects;

namespace RubeesEat.IntegrationTests.SeleniumTests;

[TestFixture]
public class ShowBillsSeleniumTests() : WithTestBills("ShowBills")
{
    [Test]
    public void ShowBills_FirstPage()
    {
        var page = Start<ShowBillsPageObject>();
        var balanceChanges = page.GetBalanceChanges();
        
        Assert.That(balanceChanges.Length, Is.LessThan(4));

        Assert.That(balanceChanges[0].Amount, Is.EqualTo("1000€"));
        Assert.That(balanceChanges[0].Date, Is.EqualTo("Dienstag, 19. August 2025"));
        Assert.That(balanceChanges[0].Description, Is.EqualTo("Default user paid lunch 2025"));

        Assert.That(balanceChanges[1].Amount, Is.AnyOf("1000000,50€", "1000000.50€"));
        Assert.That(balanceChanges[1].Date, Is.EqualTo("Montag, 9. September 2024"));
        Assert.That(balanceChanges[1].Description, Is.EqualTo("Mittagessen auf Patrick sein Nacken"));

        Assert.That(balanceChanges[2].Amount, Is.EqualTo("-800€"));
        Assert.That(balanceChanges[2].Date, Is.EqualTo("Sonntag, 28. April 2024"));
        Assert.That(balanceChanges[2].Description, Is.EqualTo("Lunch with default user in 2024"));
    }
    
    [Test]
    public void ShowBills_SecondPage()
    {
        var page = Start<ShowBillsPageObject>();
        page.ClickNext();
        var balanceChanges = page.GetBalanceChanges();
        
        Assert.That(balanceChanges.Length, Is.LessThan(4));

        Assert.That(balanceChanges[0].Amount, Is.EqualTo("-800€"));
        Assert.That(balanceChanges[0].Date, Is.EqualTo("Freitag, 28. April 2023"));
        Assert.That(balanceChanges[0].Description, Is.EqualTo("Lunch with default user in 2023"));

        Assert.That(balanceChanges[1].Amount, Is.EqualTo("-800€"));
        Assert.That(balanceChanges[1].Date, Is.EqualTo("Donnerstag, 28. April 2022"));
        Assert.That(balanceChanges[1].Description, Is.EqualTo("Lunch with default user 2022"));
    }
    
    [Test]
    public void ShowBills_GoToSecondPageAndBackToFirstPage()
    {
        var page = Start<ShowBillsPageObject>();
        page.ClickNext();
        page.ClickPrevious();
        var balanceChanges = page.GetBalanceChanges();
        
        Assert.That(balanceChanges.Length, Is.LessThan(4));

        Assert.That(balanceChanges[0].Amount, Is.EqualTo("1000€"));
        Assert.That(balanceChanges[0].Date, Is.EqualTo("Dienstag, 19. August 2025"));
        Assert.That(balanceChanges[0].Description, Is.EqualTo("Default user paid lunch 2025"));

        Assert.That(balanceChanges[1].Amount, Is.AnyOf("1000000,50€", "1000000.50€"));
        Assert.That(balanceChanges[1].Date, Is.EqualTo("Montag, 9. September 2024"));
        Assert.That(balanceChanges[1].Description, Is.EqualTo("Mittagessen auf Patrick sein Nacken"));

        Assert.That(balanceChanges[2].Amount, Is.EqualTo("-800€"));
        Assert.That(balanceChanges[2].Date, Is.EqualTo("Sonntag, 28. April 2024"));
        Assert.That(balanceChanges[2].Description, Is.EqualTo("Lunch with default user in 2024"));
    }
    
}
