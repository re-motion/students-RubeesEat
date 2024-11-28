using System.Collections.Immutable;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using RubeesEat.Model;
using RubeesEat.Model.DB;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;

namespace RubeesEat;

public class Startup
{
    public IConfiguration Configuration { get; }
    private readonly IHostingEnvironment _environment;

    public Startup(IConfiguration configuration, IHostingEnvironment environment)
    {
        Configuration = configuration;
        _environment = environment;
    }

    public void ConfigureServices(IServiceCollection services)
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
