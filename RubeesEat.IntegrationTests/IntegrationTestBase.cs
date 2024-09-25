using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using System.Collections.Immutable;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using RubeesEat.Model;

namespace RubeesEat.IntegrationTests;

public abstract class IntegrationTestBase
{
    protected TestServer TestServer { get; private set; } = default!;

    protected HttpClient HttpClient { get; private set; } = default!;

    [SetUp]
    public void SetUp()
    {
        TestServer = new TestServer(new WebHostBuilder()
                                    .UseStartup<Startup>()
                                    .ConfigureTestServices(ConfigureServices));
        HttpClient = TestServer.CreateClient();
    }
    
     protected virtual void ConfigureServices(IServiceCollection services)
    {
        Person defaultUser = new Person(Guid.NewGuid(), "DefaultFirstName", "DefaultLastName");
        var personRepository = new InMemoryPersonRepository(defaultUser.Id);
        personRepository.Add(defaultUser);
        personRepository.Add(new Person(Guid.NewGuid(), "Item", "Arslan"));
        personRepository.Add(new Person(Guid.NewGuid(), "Patrick", "Widener"));
        personRepository.Add(new Person(Guid.NewGuid(), "Lilli", "Grubber"));
        personRepository.Add(new Person(Guid.NewGuid(), "Mich", "Ludwig"));

        var persons = personRepository.GetAll().ToImmutableList();
        var billRepository = new InMemoryBillRepository();
        billRepository.Add(new Bill(Guid.NewGuid(), new DateTime(2024, 9, 9), "Mittagessen auf Patrick sein Nacken",
        [
            new EntryLine(persons[0], 1_000_000.50m),
            new EntryLine(persons[2], -500_000.50m),
            new EntryLine(persons[3], -500_000m)
        ]));
        billRepository.Add(new Bill(Guid.NewGuid(), new DateTime(2025, 8, 19), "Default user paid lunch 2025",
        [
            new EntryLine(persons[0], 3_000),
            new EntryLine(persons[0], -2_000),
            new EntryLine(persons[2], -500),
            new EntryLine(persons[3], -500)
        ]));
        billRepository.Add(new Bill(Guid.NewGuid(), new DateTime(2022, 4, 28), "Lunch with default user 2022",
        [
            new EntryLine(persons[1], 1_000),
            new EntryLine(persons[0], -800),
            new EntryLine(persons[3], -200)
        ]));
        billRepository.Add(new Bill(Guid.NewGuid(), new DateTime(2023, 4, 28), "Lunch with default user in 2023",
        [
            new EntryLine(persons[1], 4_000),
            new EntryLine(persons[0], -800),
            new EntryLine(persons[3], -3200)
        ]));
        billRepository.Add(new Bill(Guid.NewGuid(), new DateTime(2024, 4, 28), "Lunch with default user in 2024",
        [
            new EntryLine(persons[1], 1_000),
            new EntryLine(persons[0], -800),
            new EntryLine(persons[3], -200)
        ]));

        services.Replace(ServiceDescriptor.Singleton<IPersonRepository>(personRepository));
        services.Replace(ServiceDescriptor.Singleton<IBillRepository>(billRepository));
    }

    [TearDown]
    public void TearDown()
    {
        HttpClient.Dispose();
        TestServer.Dispose();
    }
}
