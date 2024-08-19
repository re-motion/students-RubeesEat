using RubeesEat.Model;

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
