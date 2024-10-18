using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RubeesEat.IntegrationTests.WebTesting;

namespace RubeesEat.IntegrationTests;

public class SeleniumIntegrationTestBase
{
    private const string c_hostingUrl = "http://localhost:41337";

    private readonly string _defaultSite;

    private IHost _webHost = default!;
    protected IHostBuilder _hostBuilder = default!;

    public WebTestHelper WebTestHelper { get; private set; } = default!;

    protected SeleniumIntegrationTestBase(string defaultSite)
    {
        _defaultSite = defaultSite;
    }

    [SetUp]
    public void SetUp()
    {

        _hostBuilder = Host.CreateDefaultBuilder()
                           .ConfigureWebHostDefaults(c => c
                                                          .UseUrls(c_hostingUrl)
                                                          .UseStartup<Startup>()
                                                          .UseContentRoot("..\\..\\..\\..\\RubeesEat")
                                                          .ConfigureServices(ConfigureServices))
                           .UseEnvironment("Test");
        ConfigureHostBuilder(_hostBuilder);
        _webHost = _hostBuilder.Build();
        _webHost.Start();
        WebTestHelper = new WebTestHelper();
    }
    
    [TearDown]
    public void TearDown()
    {
        WebTestHelper.Dispose();
        _webHost.Dispose();
    }

    protected TPageObject Start<TPageObject>(string relativeUrl = null)
        where TPageObject : PageObject, new()
    {
        var url = $"{c_hostingUrl}/{relativeUrl ?? _defaultSite}";

        WebTestHelper.Visit(url);

        return WebTestHelper.CreatePageObject<TPageObject>();
    }
    
    protected virtual void ConfigureHostBuilder(IHostBuilder hostBuilder) {}
    protected virtual void ConfigureServices(IServiceCollection services) {}
}
