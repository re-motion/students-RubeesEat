using RubeesEat.IntegrationTests.PageObjects;

namespace RubeesEat.IntegrationTests.SeleniumTests;

[TestFixture]
public class OpenIDSeleniumTests() : SeleniumIntegrationTestBase("Index")
{
    [Test]
    public void RedirectsToOpenIDProvider()
    {
        var page = Start<UserHomepagePageObject>();
        Assert.That(page.GetCurrentUrl(), Does.StartWith("https://dev-amcvh6f04mjr767m.us.auth0.com/"));
    }
}
