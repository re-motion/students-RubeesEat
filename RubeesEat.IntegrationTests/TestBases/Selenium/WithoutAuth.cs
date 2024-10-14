using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RubeesEat.IntegrationTests.SeleniumTests;

namespace RubeesEat.IntegrationTests;

public class WithoutAuth : SeleniumIntegrationTestBase
{
    protected WithoutAuth(string defaultSite) : base(defaultSite)
    {
    }

    protected override void ConfigureHostBuilder(IHostBuilder hostBuilder)
    {
        var configSettings = new Dictionary<string, string?>()
        {
            {"OpenID:Authority", "https://mockAuthority"},
            {"OpenID:ClientID", "mockClientID"},
            {"OpenID:ClientSecret", "mockClientSecret"},
            {"OpenID:MetadataAddress", "https://mockMetadataAddress"}
        };
        var config = new ConfigurationBuilder().AddJsonFile("appsettings.Development.json").Build();
        hostBuilder.ConfigureAppConfiguration((context, builder) =>
                   {
                       builder.AddConfiguration(config);
                   })
                   .ConfigureAppConfiguration((hostingContext, config) => config.AddInMemoryCollection(configSettings));
    }
    
    protected override void ConfigureServices(IServiceCollection services)
    {
        services.AddAuthentication(defaultScheme: "TestScheme")
                .AddScheme<AuthenticationSchemeOptions, TestAuthHandler>(
                    "TestScheme", options => { });
        
        services.AddAuthorization(options =>
        {
            options.FallbackPolicy = new AuthorizationPolicyBuilder("TestScheme")
                                     .RequireAuthenticatedUser()
                                     .Build();
        });
    }
}
