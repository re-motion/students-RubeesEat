using System.Collections.Immutable;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using RubeesEat.Model;

namespace RubeesEat.IntegrationTests;

public class WithTestBills : WithTestPersons
{
    protected WithTestBills(string defaultSite) : base(defaultSite)
    {
    }
    
    protected override void ConfigureServices(IServiceCollection services)
    {
        base.ConfigureServices(services);
        var persons = PersonRepository.GetAll().ToImmutableList();
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
        services.Replace(ServiceDescriptor.Singleton<IBillRepository>(billRepository));
    }
}
