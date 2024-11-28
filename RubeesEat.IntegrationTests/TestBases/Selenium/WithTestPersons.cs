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
        Person defaultUser = new Person(Guid.NewGuid(), "DefaultFirstName", "DefaultLastName", "DefaultLoginName", "Default@bla");
        PersonRepository = new InMemoryPersonRepository(defaultUser.Id);
        PersonRepository.Add(defaultUser);
        PersonRepository.Add(new Person(Guid.NewGuid(), "Item", "Arslan", "item.arslan", "item.arslan@bla"));
        PersonRepository.Add(new Person(Guid.NewGuid(), "Patrick", "Widener", "patrick.widener", "patrick.widener@bla"));
        PersonRepository.Add(new Person(Guid.NewGuid(), "Lilli", "Grubber", "lilli.grubber", "lilli.grubber@bla"));
        PersonRepository.Add(new Person(Guid.NewGuid(), "Mich", "Ludwig", "mich.ludwig", "mich.ludwig@bla"));

        services.Replace(ServiceDescriptor.Singleton<IPersonRepository>(PersonRepository));
        var billRepository = new InMemoryBillRepository();
        services.Replace(ServiceDescriptor.Singleton<IBillRepository>(billRepository));
    }
}
