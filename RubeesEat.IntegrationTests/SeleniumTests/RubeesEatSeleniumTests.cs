using RubeesEat.IntegrationTests.PageObjects;

namespace RubeesEat.IntegrationTests.SeleniumTests;

[TestFixture]
public class RubeesEatSeleniumTests() : SeleniumIntegrationTestBase("UserHomepage")
{
    [Test]
    public void GetDefaultBalanceChanges()
    {
        var page = Start<UserHomepagePageObject>();
        var balanceChanges = page.GetBalanceChanges();

        Assert.That(balanceChanges[0].Amount, Is.EqualTo("1000€"));
        Assert.That(balanceChanges[0].Date, Is.EqualTo("Tuesday, August 19, 2025"));
        Assert.That(balanceChanges[0].Description, Is.EqualTo("Default user paid lunch 2025"));

        Assert.That(balanceChanges[1].Amount, Is.EqualTo("-800€"));
        Assert.That(balanceChanges[1].Date, Is.EqualTo("Sunday, April 28, 2024"));
        Assert.That(balanceChanges[1].Description, Is.EqualTo("Lunch with default user in 2024"));

        Assert.That(balanceChanges[2].Amount, Is.EqualTo("-800€"));
        Assert.That(balanceChanges[2].Date, Is.EqualTo("Friday, April 28, 2023"));
        Assert.That(balanceChanges[2].Description, Is.EqualTo("Lunch with default user in 2023"));

        Assert.That(balanceChanges[3].Amount, Is.EqualTo("-800€"));
        Assert.That(balanceChanges[3].Date, Is.EqualTo("Thursday, April 28, 2022"));
        Assert.That(balanceChanges[3].Description, Is.EqualTo("Lunch with default user 2022"));
    }

    [Test]
    public void GetAccountBalance()
    {
        var page = Start<UserHomepagePageObject>();
        Assert.That(
            page.GetAccountBalance(),
            Is.EqualTo("+ 1.000.000,50 €"));
    }
}
