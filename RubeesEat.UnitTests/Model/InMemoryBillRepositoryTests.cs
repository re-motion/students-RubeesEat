using System.Collections.Immutable;
using RubeesEat.Model;

namespace RubeesEat.UnitTests.Model;

[TestFixture]
public class InMemoryBillRepositoryTests
{
    [Test]
    public void GetAll_EntriesInDictionary_ReturnsEntries()
    {
        var storage = new Dictionary<Guid, Bill>();
        storage.Add(TestDomain.BillCafeLeBlanc.Id, TestDomain.BillCafeLeBlanc);
        storage.Add(TestDomain.BillMaidCafe.Id, TestDomain.BillMaidCafe);
        var billRepository = new InMemoryBillRepository(storage);

        var allEntries = billRepository.GetAll();

        Assert.That(allEntries.Count, Is.EqualTo(2));
        Assert.That(allEntries[0].Id, Is.EqualTo(TestDomain.BillCafeLeBlanc.Id));
        Assert.That(allEntries[1].Id, Is.EqualTo(TestDomain.BillMaidCafe.Id));
    }

    [Test]
    public void Add_NoEntriesInDictionary_EntriesSuccessfullyAdded()
    {
        var storage = new Dictionary<Guid, Bill>();
        var billRepository = new InMemoryBillRepository(storage);

        billRepository.Add(TestDomain.BillCafeLeBlanc);
        billRepository.Add(TestDomain.BillMaidCafe);

        Assert.That(billRepository.GetById(TestDomain.BillCafeLeBlanc.Id), Is.EqualTo(TestDomain.BillCafeLeBlanc));
        Assert.That(billRepository.GetById(TestDomain.BillMaidCafe.Id), Is.EqualTo(TestDomain.BillMaidCafe));
    }
}
