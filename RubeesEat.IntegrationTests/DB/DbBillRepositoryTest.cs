using System.Globalization;
using RubeesEat.Model;
using RubeesEat.Model.DB;

namespace RubeesEat.IntegrationTests.DB;

[TestFixture]
[Explicit("does not run on teamcity")]
public class DbBillRepositoryTest : DatabaseIntegrationTestBase
{
    private DbBillRepository _dbBillRepository;
    private DbPersonRepository _dbPersonRepository;
    
    [SetUp]
    public void Setup()
    {
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
        Assert.That(_dbBillRepository.GetAll()[0] == bill, Is.EqualTo(true));
    }
    
    [Test]
    public void GetAllForUserTest()
    {
        var currentUser = _dbPersonRepository.GetCurrentUser();
        var bills = _dbBillRepository. GetAllForUser(currentUser);
        
        Assert.That(bills, Has.Count.EqualTo(2));
    }
    
    [Test]
    public void GetRecentBalanceChanges_GetsTheMostRecentBalanceChanges()
    {
        var currentUser = _dbPersonRepository.GetCurrentUser();
        var balanceChanges = _dbBillRepository.GetRecentBalanceChanges(currentUser, 2);
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
        var currentUser = _dbPersonRepository.GetCurrentUser();
        decimal balance = _dbBillRepository.GetBalance(currentUser);
        Assert.That(balance, Is.EqualTo(999980m));
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
}
