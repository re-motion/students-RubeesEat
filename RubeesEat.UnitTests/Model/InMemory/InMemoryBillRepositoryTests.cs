using System.Collections.Immutable;
using RubeesEat.Model;

namespace RubeesEat.UnitTests.Model.InMemory;

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

    [Test]
    public void GetAllFromUser_EntriesExist_ReturnsUserEntries()
    {
        Person defaultUser = Person.Create("DefaultFirstName", "DefaultLastName", "defaulLoginName");
        var billRepository = new InMemoryBillRepository();

        var personRepository = new InMemoryPersonRepository(defaultUser.Id);
        personRepository.Add(defaultUser);
        personRepository.Add(new Person(Guid.NewGuid(), "Item", "Arslan", "item.arslan"));
        personRepository.Add(new Person(Guid.NewGuid(), "Patrick", "Widener", "patrick.widener"));
        var persons = personRepository.GetAll().ToImmutableList();
        var firstBill = new Bill(Guid.NewGuid(), new DateTime(2024, 9, 9), "Mittagessen auf Patrick sein Nacken",
        [
            new EntryLine(persons[0], 500),
            new EntryLine(persons[1], -500),
        ]);
        billRepository.Add(firstBill);
        billRepository.Add(new Bill(Guid.NewGuid(), new DateTime(2024, 9, 9), "Mittagessen auf Patrick sein Nacken",
        [
            new EntryLine(persons[1], 1_000_000),
            new EntryLine(persons[2], -500_000),
            new EntryLine(persons[1], -500_000)
        ]));

        var billsFromUser = billRepository.GetAllForUser(defaultUser);
        var expected = new List<Bill> { firstBill };

        Assert.That(billsFromUser.Count, Is.EqualTo(1));
        Assert.That(billsFromUser[0].Id, Is.EqualTo(expected[0].Id));
    }

    [Test]
    public void GetRecentBalanceChanges_OneBalanceChange_ReturnsOneBalanceChange()
    {
        Person defaultUser = Person.Create("DefaultFirstName", "DefaultLastName", "defaulLoginName");
        var billRepository = new InMemoryBillRepository();

        var personRepository = new InMemoryPersonRepository(defaultUser.Id);
        personRepository.Add(defaultUser);
        personRepository.Add(new Person(Guid.NewGuid(), "Item", "Arslan", "item.arslan"));
        personRepository.Add(new Person(Guid.NewGuid(), "Patrick", "Widener", "patrick.widener"));
        var persons = personRepository.GetAll().ToImmutableList();
        DateTime firstDate = new DateTime(2024, 9, 9);
        string firstDescription = "Mittagessen auf Patrick sein Nacken";
        var firstBill = new Bill(Guid.NewGuid(), firstDate, firstDescription,
        [
            new EntryLine(persons[0], 500),
            new EntryLine(persons[0], -300),
            new EntryLine(persons[1], -200),
        ]);
        billRepository.Add(firstBill);
        billRepository.Add(new Bill(Guid.NewGuid(), new DateTime(2024, 9, 9), "Mittagessen auf Patrick sein Nacken",
        [
            new EntryLine(persons[0], 1_000_000),
            new EntryLine(persons[2], -500_000),
            new EntryLine(persons[1], -500_000)
        ]));

        var balanceChangesFromUser = billRepository.GetRecentBalanceChanges(defaultUser, 1, 1);

        Assert.That(balanceChangesFromUser.Count, Is.EqualTo(1));
        Assert.That(balanceChangesFromUser[0].Amount, Is.EqualTo(200));
        Assert.That(balanceChangesFromUser[0].Date, Is.EqualTo(firstDate));
        Assert.That(balanceChangesFromUser[0].Description, Is.EqualTo(firstDescription));
    }

    [Test]
    public void GetRecentBalanceChanges_ALotOfBalanceChecks_OnlyReturnsACertainAmount()
    {
        Person defaultUser = Person.Create("DefaultFirstName", "DefaultLastName", "defaulLoginName");
        var billRepository = new InMemoryBillRepository();

        var personRepository = new InMemoryPersonRepository(defaultUser.Id);
        personRepository.Add(defaultUser);
        personRepository.Add(new Person(Guid.NewGuid(), "Item", "Arslan", "item.arslan"));
        personRepository.Add(new Person(Guid.NewGuid(), "Patrick", "Widener", "patrick.widener"));
        var persons = personRepository.GetAll().ToImmutableList();
        var dateTime = new DateTime(2024, 9, 9);
        DateTime firstDate = dateTime;
        var description = "Mittagessen auf Patrick sein Nacken";
        string firstDescription = description;
        var firstBill = new Bill(Guid.NewGuid(), firstDate, firstDescription,
        [
            new EntryLine(persons[0], 500),
            new EntryLine(persons[0], -300),
            new EntryLine(persons[1], -200),
        ]);
        billRepository.Add(firstBill);

        var entryLine1 = new EntryLine(persons[0], 1_000_000);
        var entryLine2 = new EntryLine(persons[2], -500_000);
        var entryLine3 = new EntryLine(persons[1], -500_000);
        billRepository.Add(new Bill(Guid.NewGuid(), dateTime, description, [entryLine1, entryLine2, entryLine3]));
        billRepository.Add(new Bill(Guid.NewGuid(), dateTime, description, [entryLine1, entryLine2, entryLine3]));
        billRepository.Add(new Bill(Guid.NewGuid(), dateTime, description, [entryLine1, entryLine2, entryLine3]));
        billRepository.Add(new Bill(Guid.NewGuid(), dateTime, description, [entryLine1, entryLine2, entryLine3]));

        var balanceChangesFromUser = billRepository.GetRecentBalanceChanges(defaultUser, 1, 3);

        Assert.That(balanceChangesFromUser.Count, Is.EqualTo(3));
    }

    [Test]
    public void GetRecentBalanceChanges_ZeroBalanceChangesFromUser_ReturnsEmpty()
    {
        Person defaultUser = Person.Create("DefaultFirstName", "DefaultLastName", "defaulLoginName");
        var billRepository = new InMemoryBillRepository();

        var personRepository = new InMemoryPersonRepository(defaultUser.Id);
        personRepository.Add(defaultUser);
        personRepository.Add(new Person(Guid.NewGuid(), "Item", "Arslan", "item.arslan"));
        personRepository.Add(new Person(Guid.NewGuid(), "Patrick", "Widener", "patrick.widener"));
        var persons = personRepository.GetAll().ToImmutableList();
        DateTime firstDate = new DateTime(2024, 9, 9);
        string firstDescription = "Mittagessen auf Patrick sein Nacken";
        var firstBill = new Bill(Guid.NewGuid(), firstDate, firstDescription,
        [
            new EntryLine(persons[2], 500),
            new EntryLine(persons[2], -300),
            new EntryLine(persons[1], -200),
        ]);
        billRepository.Add(firstBill);
        billRepository.Add(new Bill(Guid.NewGuid(), new DateTime(2024, 9, 9), "Mittagessen auf Patrick sein Nacken",
        [
            new EntryLine(persons[1], 1_000_000),
            new EntryLine(persons[2], -500_000),
            new EntryLine(persons[1], -500_000)
        ]));
        billRepository.Add(new Bill(Guid.NewGuid(), new DateTime(2024, 9, 9), "Mittagessen auf Patrick sein Nacken",
        [
            new EntryLine(persons[1], 1_000_000),
            new EntryLine(persons[2], -500_000),
            new EntryLine(persons[1], -500_000)
        ]));

        var balanceChangesFromUser = billRepository.GetRecentBalanceChanges(defaultUser, 1, 1);

        Assert.That(balanceChangesFromUser.Count, Is.EqualTo(0));
    }

    [Test]
    public void GetRecentBalanceChanges_EntriesExist_OrderedByDate()
    {
        Person defaultUser = Person.Create("DefaultFirstName", "DefaultLastName", "defaulLoginName");
        var billRepository = new InMemoryBillRepository();

        var personRepository = new InMemoryPersonRepository(defaultUser.Id);
        personRepository.Add(defaultUser);
        personRepository.Add(new Person(Guid.NewGuid(), "Item", "Arslan", "item.arslan"));
        personRepository.Add(new Person(Guid.NewGuid(), "Patrick", "Widener", "patrick.widener"));
        var persons = personRepository.GetAll().ToImmutableList();
        var firstBill = new Bill(Guid.NewGuid(), new DateTime(2024, 9, 9), "Mittagessen auf Patrick sein Nacken",
        [
            new EntryLine(persons[0], 500),
            new EntryLine(persons[1], -500),
        ]);
        billRepository.Add(firstBill);
        billRepository.Add(new Bill(Guid.NewGuid(), new DateTime(2004, 5, 11), "Mittagessen auf Patrick sein Nacken",
        [
            new EntryLine(persons[0], 1_000_000),
            new EntryLine(persons[2], -500_000),
            new EntryLine(persons[1], -500_000)
        ]));
        billRepository.Add(new Bill(Guid.NewGuid(), new DateTime(2023, 1, 5), "Mittagessen auf Patrick sein Nacken",
        [
            new EntryLine(persons[0], 1_000_000),
            new EntryLine(persons[2], -500_000),
            new EntryLine(persons[1], -500_000)
        ]));

        var recentBalanceChanges = billRepository.GetRecentBalanceChanges(defaultUser, 1, 3);

        Assert.That(recentBalanceChanges[0].Date, Is.EqualTo(new DateTime(2024, 9, 9)));
        Assert.That(recentBalanceChanges[1].Date, Is.EqualTo(new DateTime(2023, 1, 5)));
        Assert.That(recentBalanceChanges[2].Date, Is.EqualTo(new DateTime(2004, 5, 11)));
    }
    
    [Test] 
    public void GetBalance_ReturnsAccountBalance()
    {
        var storage = new Dictionary<Guid, Bill>();
        storage.Add(TestDomain.BillCafeLeBlanc.Id, TestDomain.BillCafeLeBlanc);
        storage.Add(TestDomain.BillMaidCafe.Id, TestDomain.BillMaidCafe);
        
        EntryLine[] entryLines =
        [
            new EntryLine(TestDomain.Jack, 60),
            new EntryLine(TestDomain.Yusuke, -60)
        ];
        Bill mcDonalds = Bill.Create("McDonalds", entryLines);
        storage.Add(mcDonalds.Id, mcDonalds);
        
        var billRepository = new InMemoryBillRepository(storage);
        
        Assert.That(billRepository.GetBalance(TestDomain.Jack), Is.EqualTo(10));
    }
    
    [Test] 
    public void GetBalance_PersonNotInAnyBill_ReturnsZero()
    {
        var storage = new Dictionary<Guid, Bill>();
        storage.Add(TestDomain.BillCafeLeBlanc.Id, TestDomain.BillCafeLeBlanc);
        storage.Add(TestDomain.BillMaidCafe.Id, TestDomain.BillMaidCafe);
        var billRepository = new InMemoryBillRepository(storage);
        
        Assert.That(billRepository.GetBalance(TestDomain.Levi), Is.EqualTo(0));
    }

    [Test]
    public void GetBalanceChangesForPage_Page1_ReturnsBalanceChangesForPage1()
    {
        var storage = new Dictionary<Guid, Bill>();
        EntryLine[] entryLines =
        [
            new EntryLine(TestDomain.Jack, 60),
            new EntryLine(TestDomain.Yusuke, -60)
        ];
        var mcDonalds = Bill.Create("McDonalds", entryLines);

        var balanceChange = new PaginatedView<BalanceChange>(
            ImmutableArray.Create(new BalanceChange(60, mcDonalds.Date, mcDonalds.Description, mcDonalds.Id)),
            1,
            1
        );
        storage.Add(mcDonalds.Id, mcDonalds);
        storage.Add(TestDomain.BillCafeLeBlanc.Id, TestDomain.BillCafeLeBlanc);
        storage.Add(TestDomain.BillMaidCafe.Id, TestDomain.BillMaidCafe);
        var billRepository = new InMemoryBillRepository(storage);
        Assert.That(billRepository.GetRecentBalanceChanges(TestDomain.Jack, 1, 1), Is.EqualTo(balanceChange));
    }
}
