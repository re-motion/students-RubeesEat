using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using RubeesEat.Model;
using RubeesEat.Model.DB;

namespace RubeesEat;

public class Startup
{
    public IConfiguration Configuration { get; }

    private readonly IWebHostEnvironment _environment;

    public Startup(IConfiguration configuration, IWebHostEnvironment environment)
    {
        Configuration = configuration;
        _environment = environment;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        if (!_environment.IsDevelopment())
        {
            var openIdSettings = Configuration.GetSection("OpenId").Get<OpenIDSettings>();
            Validator.ValidateObject(openIdSettings, new ValidationContext(openIdSettings), validateAllProperties: true);

            services.AddAuthentication(options =>
                    {
                        options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                        options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
                    })
                    .AddCookie()
                    .AddOpenIdConnect(options =>
                    {
                        options.MetadataAddress = openIdSettings.MetadataAddress;
                        options.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                        options.ClientId = openIdSettings.ClientId;
                        options.ClientSecret = openIdSettings.ClientSecret;
                        options.GetClaimsFromUserInfoEndpoint = true;
                    });

            services.AddAuthorization(option =>
            {
                option.FallbackPolicy = option.DefaultPolicy;
            });
        }

        services.AddRazorPages();

        if (_environment.IsDevelopment())
        {
            services.AddSingleton<IClaimsPrincipalPersonFactory, DebugClaimsPrincipalPersonFactory>();
        }
        else
        {
            services.AddSingleton<IClaimsPrincipalPersonFactory, ClaimsPrincipalPersonFactory>();
        }

        var connectionString = Configuration["ConnectionStrings:DefaultConnectionString"];
        var sqlConnectionFactory = new SqlConnectionFactory(connectionString);
        services.AddSingleton<IDbConnectionFactory>(sqlConnectionFactory);

        services.AddSingleton<IPersonRepository, DbPersonRepository>();
        services.AddSingleton<IBillRepository, DbBillRepository>();
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
        if (!env.IsDevelopment())
        {
            app.UseAuthentication();
            app.UseAuthorization();
        }

        app.UseEndpoints(
            endpoints =>
            {
                endpoints.MapRazorPages();
                endpoints.MapControllers();
            });
    }
}
