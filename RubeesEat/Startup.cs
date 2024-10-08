using System.Collections.Immutable;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using RubeesEat.Model;
using RubeesEat.Model.DB;

namespace RubeesEat;

public class Startup
{
    public IConfiguration Configuration { get; }

    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
                })
                .AddCookie()
                .AddOpenIdConnect(options =>
                {
                    options.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                    options.ClientId = "BdOG7g762eoAF7sXkoH2Gu2UXBVwzViJ";
                    options.ClientSecret = "_pIXrOgfWvvFhOqUvuBMSASLhL2RpryArgEGdDRfWWCxnFVp7aqEGRt7_8lcBHkF";
                    options.Authority = "https://dev-amcvh6f04mjr767m.us.auth0.com";
                    options.GetClaimsFromUserInfoEndpoint = true;
                });
        services.AddAuthorization(option =>
        {
            option.FallbackPolicy = option.DefaultPolicy;
        });
        services.AddRazorPages();

        var connectionString = Configuration["ConnectionStrings:DefaultConnectionString"];
        var sqlConnectionFactory = new SqlConnectionFactory(connectionString);
        services.AddSingleton<IDbConnectionFactory>(sqlConnectionFactory);

        var dbPersonRepository = new DbPersonRepository(sqlConnectionFactory);
        services.AddSingleton<IPersonRepository>(dbPersonRepository);

        var dbBillRepository = new DbBillRepository(sqlConnectionFactory);
        services.AddSingleton<IBillRepository>(dbBillRepository);
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (!env.IsDevelopment())
        {
            app.UseHsts();
        }

        app.UseHttpsRedirection();

        app.UseDefaultFiles();
        app.UseStaticFiles();

        app.UseRouting();
        app.UseAuthentication();
        app.UseAuthorization();

        app.UseEndpoints(
            endpoints =>
            {
                endpoints.MapRazorPages();
                endpoints.MapControllers();
            });
    }
}
