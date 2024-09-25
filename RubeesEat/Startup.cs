using System.Collections.Immutable;
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
