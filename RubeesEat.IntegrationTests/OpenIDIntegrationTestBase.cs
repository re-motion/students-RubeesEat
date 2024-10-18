using Microsoft.AspNetCore.Hosting;
using System.Collections.Immutable;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using RubeesEat.IntegrationTests.WebTesting;
using RubeesEat.Model;

namespace RubeesEat.IntegrationTests;

public class OpenIDIntegrationTestBase
{
    private const string c_hostingUrl = "http://localhost:41337";

    private readonly string _defaultSite;

    private IHost _webHost = default!;

    public WebTestHelper WebTestHelper { get; private set; } = default!;

    protected OpenIDIntegrationTestBase(string defaultSite)
    {
        _defaultSite = defaultSite;
    }

    [SetUp]
    public void SetUp()
    {
        
        _webHost = Host.CreateDefaultBuilder()
                       .ConfigureWebHostDefaults(c => c
                                                      .UseUrls(c_hostingUrl)
                                                      .UseStartup<Startup>()
                                                      .UseContentRoot("..\\..\\..\\..\\RubeesEat"))
                       .Build();
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
}
