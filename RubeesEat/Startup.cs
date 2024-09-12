using System.Collections.Immutable;
using RubeesEat.Model;
using RubeesEat.Model.DB;
using RubeesEat.Model.InMemory;

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

        var todoRepository = new InMemoryTodoRepository();
        todoRepository.Add(TodoItem.Create("My first todo"));

        services.AddSingleton<ITodoRepository>(todoRepository);

        ConfigureServicesEAT(services);
    }

    public void ConfigureServicesEAT(IServiceCollection services)
    {
        var sqlConnectionFactory = new SqlConnectionFactory(Configuration, "DefaultConnectionString");
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
