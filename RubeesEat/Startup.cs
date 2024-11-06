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

        app.UseEndpoints(
            endpoints =>
            {
                endpoints.MapRazorPages();
                endpoints.MapControllers();
            });
    }
}
