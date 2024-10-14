using Microsoft.AspNetCore.Hosting;
using System.Collections.Immutable;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using RubeesEat.IntegrationTests.WebTesting;
using RubeesEat.Model;

namespace RubeesEat.IntegrationTests;

public class SplitBillSeleniumIntegrationTestBase
{
    private const string c_hostingUrl = "http://localhost:41337";

    private readonly string _defaultSite;

    private IHost _webHost = default!;

    public WebTestHelper WebTestHelper { get; private set; } = default!;

    protected SplitBillSeleniumIntegrationTestBase(string defaultSite)
    {
        _defaultSite = defaultSite;
    }

    [SetUp]
    public void SetUp()
    {
        var config = new ConfigurationBuilder().AddJsonFile("appsettings.Development.json").Build();
        _webHost = Host.CreateDefaultBuilder()
                       .ConfigureAppConfiguration((context, builder) =>
                       {
                           builder.AddConfiguration(config);
                       })
                       .ConfigureWebHostDefaults(c => c
                                                      .UseUrls(c_hostingUrl)
                                                      .UseStartup<Startup>()
                                                      .UseContentRoot("..\\..\\..\\..\\RubeesEat")
                                                      .ConfigureServices(ConfigureServices))
													  .UseEnvironment("Test")
                       .Build();
        _webHost.Start();

        WebTestHelper = new WebTestHelper();
    }
    
    protected virtual void ConfigureServices(IServiceCollection services)
    {
        Person defaultUser = new Person(Guid.NewGuid(), "DefaultFirstName", "DefaultLastName");
        var personRepository = new InMemoryPersonRepository(defaultUser.Id);
        personRepository.Add(defaultUser);
        personRepository.Add(new Person(Guid.NewGuid(), "Item", "Arslan"));
        personRepository.Add(new Person(Guid.NewGuid(), "Patrick", "Widener"));
        personRepository.Add(new Person(Guid.NewGuid(), "Lilli", "Grubber"));
        personRepository.Add(new Person(Guid.NewGuid(), "Mich", "Ludwig"));
        personRepository.Add(new Person(Guid.NewGuid(), "Inactive", "User", false));
        
        var billRepository = new InMemoryBillRepository();
        services.Replace(ServiceDescriptor.Singleton<IPersonRepository>(personRepository));
        services.Replace(ServiceDescriptor.Singleton<IBillRepository>(billRepository));
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
