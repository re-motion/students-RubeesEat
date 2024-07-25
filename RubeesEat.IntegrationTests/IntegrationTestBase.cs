using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;

namespace RubeesEat.IntegrationTests;

public abstract class IntegrationTestBase
{
    protected TestServer TestServer { get; private set; } = default!;

    protected HttpClient HttpClient { get; private set; } = default!;

    [SetUp]
    public void SetUp()
    {
        TestServer = new TestServer(new WebHostBuilder().UseStartup<Startup>());
        HttpClient = TestServer.CreateClient();
    }

    [TearDown]
    public void TearDown()
    {
        HttpClient.Dispose();
        TestServer.Dispose();
    }
}
