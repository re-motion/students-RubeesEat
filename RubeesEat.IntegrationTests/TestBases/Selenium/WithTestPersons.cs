using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using RubeesEat.Model;

namespace RubeesEat.IntegrationTests;

public class WithTestPersons : WithoutAuth
{
    protected InMemoryPersonRepository PersonRepository;
    protected WithTestPersons(string defaultSite) : base(defaultSite)
    {
    }
    
    protected override void ConfigureServices(IServiceCollection services)
    {
        base.ConfigureServices(services);
        Person defaultUser = new Person(Guid.NewGuid(), "DefaultFirstName", "DefaultLastName");
        PersonRepository = new InMemoryPersonRepository(defaultUser.Id);
        PersonRepository.Add(defaultUser);
        PersonRepository.Add(new Person(Guid.NewGuid(), "Item", "Arslan"));
        PersonRepository.Add(new Person(Guid.NewGuid(), "Patrick", "Widener"));
        PersonRepository.Add(new Person(Guid.NewGuid(), "Lilli", "Grubber"));
        PersonRepository.Add(new Person(Guid.NewGuid(), "Mich", "Ludwig"));

        services.Replace(ServiceDescriptor.Singleton<IPersonRepository>(PersonRepository));
        var billRepository = new InMemoryBillRepository();
        services.Replace(ServiceDescriptor.Singleton<IBillRepository>(billRepository));
    }
}
