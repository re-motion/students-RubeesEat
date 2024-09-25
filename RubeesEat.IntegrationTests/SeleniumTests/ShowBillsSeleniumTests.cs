using RubeesEat.IntegrationTests.PageObjects;

namespace RubeesEat.IntegrationTests.SeleniumTests;

[TestFixture]
public class ShowBillsSeleniumTests() : SeleniumIntegrationTestBase("ShowBills")
{
    [Test]
    public void ShowBills_FirstPage()
    {
        var page = Start<ShowBillsPageObject>();
        var balanceChanges = page.GetBalanceChanges();
        
        Assert.That(balanceChanges.Length, Is.LessThan(4));

        Assert.That(balanceChanges[0].Amount, Is.EqualTo("1000€"));
        Assert.That(balanceChanges[0].Date, Is.EqualTo("Tuesday, August 19, 2025"));
        Assert.That(balanceChanges[0].Description, Is.EqualTo("Default user paid lunch 2025"));

        Assert.That(balanceChanges[1].Amount, Is.AnyOf("1000000,50\u20ac", "1000000.50\u20ac"));
        Assert.That(balanceChanges[1].Date, Is.EqualTo("Monday, September 9, 2024"));
        Assert.That(balanceChanges[1].Description, Is.EqualTo("Mittagessen auf Patrick sein Nacken"));

        Assert.That(balanceChanges[2].Amount, Is.EqualTo("-800€"));
        Assert.That(balanceChanges[2].Date, Is.EqualTo("Sunday, April 28, 2024"));
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
        Assert.That(balanceChanges[0].Date, Is.EqualTo("Friday, April 28, 2023"));
        Assert.That(balanceChanges[0].Description, Is.EqualTo("Lunch with default user in 2023"));

        Assert.That(balanceChanges[1].Amount, Is.EqualTo("-800€"));
        Assert.That(balanceChanges[1].Date, Is.EqualTo("Thursday, April 28, 2022"));
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
        Assert.That(balanceChanges[0].Date, Is.EqualTo("Tuesday, August 19, 2025"));
        Assert.That(balanceChanges[0].Description, Is.EqualTo("Default user paid lunch 2025"));

        Assert.That(balanceChanges[1].Amount, Is.AnyOf("1000000,50\u20ac", "1000000.50\u20ac"));
        Assert.That(balanceChanges[1].Date, Is.EqualTo("Monday, September 9, 2024"));
        Assert.That(balanceChanges[1].Description, Is.EqualTo("Mittagessen auf Patrick sein Nacken"));

        Assert.That(balanceChanges[2].Amount, Is.EqualTo("-800€"));
        Assert.That(balanceChanges[2].Date, Is.EqualTo("Sunday, April 28, 2024"));
        Assert.That(balanceChanges[2].Description, Is.EqualTo("Lunch with default user in 2024"));
    }
    
}
