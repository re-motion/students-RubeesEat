using System.Collections.Immutable;
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
        if (!_environment.IsEnvironment("Test"))
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
                        options.ClientId = Configuration["OpenId:ClientId"];
                        options.ClientSecret = Configuration["OpenId:ClientSecret"];
                        options.Authority = Configuration["OpenId:Authority"];
                        options.GetClaimsFromUserInfoEndpoint = true;
                    });
            services.AddAuthorization(option =>
            {
                option.FallbackPolicy = option.DefaultPolicy;
            });
        }
        services.AddHttpContextAccessor();

    
        services.AddRazorPages();

        var connectionString = Configuration["ConnectionStrings:DefaultConnectionString"];
        var sqlConnectionFactory = new SqlConnectionFactory(connectionString);
        services.AddSingleton<IDbConnectionFactory>(sqlConnectionFactory);
        
        services.AddScoped<IPersonRepository, DbPersonRepository>();
        services.AddScoped<IBillRepository, DbBillRepository>();
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
