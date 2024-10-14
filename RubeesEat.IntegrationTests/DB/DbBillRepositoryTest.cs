using System.Globalization;
using System.Security.Claims;
using RubeesEat.Model;
using RubeesEat.Model.DB;
using RubeesEat.Model.EqualityComparer;

namespace RubeesEat.IntegrationTests.DB;

[TestFixture]
[Explicit("does not run on teamcity")]
public class DbBillRepositoryTest : DatabaseIntegrationTestBase
{
    private DbBillRepository _dbBillRepository;
    private DbPersonRepository _dbPersonRepository;
    private BillPropertyEqualityComparer _billPropertyComparer;
    
    [SetUp]
    public void Setup()
    {
        _billPropertyComparer = new BillPropertyEqualityComparer();
        _dbBillRepository = new DbBillRepository(Factory);
        _dbPersonRepository = new DbPersonRepository(Factory);
    }
    
    [Test]
    public void GetAll_ReturnsAllBills()
    {
        var bills = _dbBillRepository.GetAll();
        
        Assert.That(bills[0].Id, Is.EqualTo(Guid.Parse("91CD57E6-418E-4628-82CC-09D471153CF6")));
        Assert.That(bills[0].Description, Is.EqualTo("Mittagessen auf Patrick sein Nacken"));
        Assert.That(bills[0].Date.ToString(CultureInfo.InvariantCulture), Is.EqualTo("09/09/2024 10:34:09"));
        Assert.That(bills[0].EntryLines.Length, Is.EqualTo(4));
        
        Assert.That(bills[1].Id, Is.EqualTo(Guid.Parse("01CD57E6-418E-4628-82CC-09D471153CF6")));
        Assert.That(bills[1].Description, Is.EqualTo("Mittagessen auf Michi sein Nacken"));
        Assert.That(bills[1].Date.ToString(CultureInfo.InvariantCulture), Is.EqualTo("11/09/2021 10:34:09"));
        Assert.That(bills[1].EntryLines.Length, Is.EqualTo(4));
    }

    [Test]
    public void Add_AddsNewBill()
    {
        var persons = _dbPersonRepository.GetAll();
        var bill = new Bill(Guid.NewGuid(), new DateTime(2025, 9, 9), "Mittagessen auf Item sein Nacken",
        [
            new EntryLine(persons[1], 1_000_000.50m),
            new EntryLine(persons[2], -500_000.50m),
            new EntryLine(persons[3], -500_000m)
        ]);
        _dbBillRepository.Add(bill);
        Assert.That(_dbBillRepository.GetAll()[0], Is.EqualTo(bill).Using(_billPropertyComparer));
    }

    [Test]
    public void Update_UpdatesBill()
    {
        var persons = _dbPersonRepository.GetAll();
        var bill = new Bill(Guid.NewGuid(), new DateTime(2025, 9, 9), "Mittagessen auf Item sein Nacken",
        [
            new EntryLine(persons[1], 1_000_000.50m),
            new EntryLine(persons[2], -500_000.50m),
            new EntryLine(persons[3], -500_000m)
        ]);
        _dbBillRepository.Add(bill);
        Assert.That(_dbBillRepository.GetAll()[0], Is.EqualTo(bill).Using(_billPropertyComparer));

        var updatedBill = new Bill(bill.Id, bill.Date, "Updated description.",
        [
            new EntryLine(persons[1], 10m),
            new EntryLine(persons[2], -5m),
            new EntryLine(persons[3], -5m)
        ]);
        _dbBillRepository.Update(updatedBill);
        Assert.That(_dbBillRepository.GetAll()[0], Is.EqualTo(updatedBill).Using(_billPropertyComparer));
    }
    
    [Test]
    public void Update_EntriesNotInDatabase_ThrowsKeyNotFoundException()
    {
        var persons = _dbPersonRepository.GetAll();

        var nonExistingBill = new Bill(Guid.NewGuid(), new DateTime(2025, 9, 9), "Not existing bill",
        [
            new EntryLine(persons[1], 1_000_000.50m),
            new EntryLine(persons[2], -500_000.50m),
            new EntryLine(persons[3], -500_000m)
        ]);

        var updatedBill = new Bill(nonExistingBill.Id, nonExistingBill.Date, "Updated description",
        [
            new EntryLine(persons[1], 10m),
            new EntryLine(persons[2], -5m),
            new EntryLine(persons[3], -5m)
        ]);

        Assert.That(() => _dbBillRepository.Update(updatedBill), 
            Throws.TypeOf<KeyNotFoundException>().With.Message.EqualTo("Bill not found"));
    }

    
    [Test]
    public void GetAllForUserTest()
    {
        var currentUser = _dbPersonRepository.GetOrCreateUser(new ClaimsPrincipal());
        var bills = _dbBillRepository. GetAllForUser(currentUser);
        
        Assert.That(bills, Has.Count.EqualTo(2));
    }
    
    [Test]
    public void GetRecentBalanceChanges_GetsTheMostRecentBalanceChanges()
    {
        var currentUser = _dbPersonRepository.GetOrCreateUser(new ClaimsPrincipal());
        var balanceChanges = _dbBillRepository.GetRecentBalanceChanges(currentUser, 1, 2);
        Assert.That(balanceChanges[0].Amount, Is.EqualTo(1000000m));
        Assert.That(balanceChanges[0].Description, Is.EqualTo("Mittagessen auf Patrick sein Nacken"));
        Assert.That(balanceChanges[0].Date.ToString(CultureInfo.InvariantCulture), Is.EqualTo("09/09/2024 10:34:09"));
        
        Assert.That(balanceChanges[1].Amount, Is.EqualTo(-20m));
        Assert.That(balanceChanges[1].Description, Is.EqualTo("Mittagessen auf Michi sein Nacken"));
        Assert.That(balanceChanges[1].Date.ToString(CultureInfo.InvariantCulture), Is.EqualTo("11/09/2021 10:34:09"));
    }

    [Test]
    public void GetBalance_ReturnsBalanceOfUser()
    {
        var currentUser = _dbPersonRepository.GetOrCreateUser(new ClaimsPrincipal());
        decimal balance = _dbBillRepository.GetBalance(currentUser);
        Assert.That(balance, Is.EqualTo(999980m));
    }
    
    [Test]
    public void GetBalanceWhenNoBills_ReturnsBalanceOfUser()
    {
        var user = _dbPersonRepository.GetById(Guid.Parse("8e3edf3b-7dcd-4815-bf16-4bc9b9d6dd3b"));
        decimal balance = _dbBillRepository.GetBalance(user);
        Assert.That(balance, Is.EqualTo(0m));
    }
    
    [Test]
    public void GetById_ReturnsBalanceOfUser()
    {
        var bill = _dbBillRepository.GetById(Guid.Parse("91CD57E6-418E-4628-82CC-09D471153CF6"));
        
        Assert.That(bill.Id, Is.EqualTo(Guid.Parse("91CD57E6-418E-4628-82CC-09D471153CF6")));
        Assert.That(bill.Description, Is.EqualTo("Mittagessen auf Patrick sein Nacken"));
        Assert.That(bill.Date.ToString(CultureInfo.InvariantCulture), Is.EqualTo("09/09/2024 10:34:09"));
        Assert.That(bill.EntryLines.Length, Is.EqualTo(4));
    }

    [Test]
    public void GetRecentBillParticipants_WithLimitIs2_ReturnsLast2Participants()
    {
        var bill = _dbBillRepository.GetRecentBillParticipants(_dbPersonRepository.GetCurrentUser(), 2);

        Assert.That(bill.Count, Is.EqualTo(2));
        Assert.That(bill[0].Id, Is.EqualTo(Guid.Parse("a05764e0-c2f5-4a3f-8f04-746aee8b355b")));
        Assert.That(bill[0].FirstName, Is.EqualTo("Item"));
        Assert.That(bill[0].LastName, Is.EqualTo("Arslan"));

        Assert.That(bill[1].Id, Is.EqualTo(Guid.Parse("544febd0-05f8-471a-bafc-ca7135538031")));
        Assert.That(bill[1].FirstName, Is.EqualTo("Lilli"));
        Assert.That(bill[1].LastName, Is.EqualTo("Grubber"));
    }
}
