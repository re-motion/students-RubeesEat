using System.Security.Claims;
using RubeesEat.Model;
using RubeesEat.Model.DB;
using RubeesEat.Model.EqualityComparer;

namespace RubeesEat.IntegrationTests.DB;

[TestFixture]
[Explicit("does not run on teamcity")]
public class DbPersonRepositoryTest : DatabaseIntegrationTestBase
{
    private DbPersonRepository _dbPersonRepository;
    private PersonPropertyEqualityComparer _personPropertyComparer;
    
    [SetUp]
    public void Setup()
    {
        _dbPersonRepository = new DbPersonRepository(Factory, new ClaimsPrincipalPersonFactory());
        _personPropertyComparer = new PersonPropertyEqualityComparer();
    }
    
    [Test]
    public void GetAll_ReturnsAllPersons()
    {
        var persons = _dbPersonRepository.GetAll().ToList();
        Assert.That(persons[3].FirstName, Is.EqualTo("Patrick"));
        Assert.That(persons[3].LastName, Is.EqualTo("Widener"));
        Assert.That(persons[3].Id, Is.EqualTo(Guid.Parse("883703F3-EEA8-4BCE-BACD-4A77FFE0C294")));
        
        Assert.That(persons[0].FirstName, Is.EqualTo("Item"));
        Assert.That(persons[0].LastName, Is.EqualTo("Arslan"));
        Assert.That(persons[0].Id, Is.EqualTo(Guid.Parse("A05764E0-C2F5-4A3F-8F04-746AEE8B355B")));
        
        Assert.That(persons[2].FirstName, Is.EqualTo("Mich"));
        Assert.That(persons[2].LastName, Is.EqualTo("Ludwig"));
        Assert.That(persons[2].Id, Is.EqualTo(Guid.Parse("7DBF157B-CBFF-43CE-BFDD-B367611BB1A5")));
        
        Assert.That(persons[1].FirstName, Is.EqualTo("Lilli"));
        Assert.That(persons[1].LastName, Is.EqualTo("Grubber"));
        Assert.That(persons[1].Id, Is.EqualTo(Guid.Parse("544FEBD0-05F8-471A-BAFC-CA7135538031")));
    }

    [Test]
    public void Add_AddsNewPerson()
    {
        Guid personId = Guid.NewGuid();
        Person person = new Person(personId, "Joel", "Fredericka", "joel.fredericka", "joel.fredericka@bla");
        _dbPersonRepository.Add(person);
        var persons = _dbPersonRepository.GetAll().ToList();
        Assert.That(persons[1].FirstName, Is.EqualTo("Joel"));
        Assert.That(persons[1].LastName, Is.EqualTo("Fredericka"));
        Assert.That(persons[1].Id, Is.EqualTo(personId));
    }

    [Test]
    public void GetById_ReturnsRightPerson()
    {
        Guid guid = Guid.Parse("883703F3-EEA8-4BCE-BACD-4A77FFE0C294");
        var person = _dbPersonRepository.GetById(guid);
        Assert.That(person.FirstName, Is.EqualTo("Patrick"));
        Assert.That(person.LastName, Is.EqualTo("Widener"));
        Assert.That(person.Id, Is.EqualTo(Guid.Parse("883703F3-EEA8-4BCE-BACD-4A77FFE0C294")));
    }

    [Test]
    public void GetOrCreateCurrentUser_UserExistsInDb_ReturnsPersonFromDb()
    {
        var claims = new[]
        {
            new Claim("name", "item.arslan@test.com"),
            new Claim("nickname", "Item.Arslan")
        };
        var claimsIdentity = new ClaimsIdentity(claims, "TestAuthType");
        var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
        
        var person = _dbPersonRepository.GetOrCreateUser(claimsPrincipal);
        var comparisonPerson = _dbPersonRepository.GetById(Guid.Parse("A05764E0-C2F5-4A3F-8F04-746AEE8B355B"));

        Assert.That(person, Is.EqualTo(comparisonPerson).Using(_personPropertyComparer));
    }
    
    [Test]
    public void GetOrCreateCurrentUser_UserDoesNotExist_CreatesNewPersonAndAddsToDb()
    {
        var allPersons = _dbPersonRepository.GetAll();
        Assert.That(allPersons, Has.None.Matches<Person>(p =>
            p is { LoginName: "newPerson", FirstName: "New", LastName: "Person" }));
        
        var claims = new[]
        {
            new Claim("name", "new.person@test.com"),
            new Claim("nickname", "New.Person")
        };
        var claimsIdentity = new ClaimsIdentity(claims, "TestAuthType");
        var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
        var person = _dbPersonRepository.GetOrCreateUser(claimsPrincipal);
        Assert.That(person, Is.EqualTo(_dbPersonRepository.GetById(person.Id)).Using(_personPropertyComparer));
    }
    
    [Test]
    public void GetOrCreateCurrentUser_NameFieldMissing_ThrowsException()
    {
        var claims = new[]
        {
            new Claim("nickname", "New.Person")
        };
        var claimsIdentity = new ClaimsIdentity(claims, "TestAuthType");
        var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
        Assert.That(() => _dbPersonRepository.GetOrCreateUser(claimsPrincipal), 
            Throws.TypeOf<InvalidOperationException>().With.Message.EqualTo("User's claim is missing name field"));
    }
    
    [Test]
    public void GetOrCreateCurrentUser_NickNameFieldMissing_ThrowsException()
    {
        var claims = new[]
        {
            new Claim("name", "new.person@test.com")
        };
        var claimsIdentity = new ClaimsIdentity(claims, "TestAuthType");
        var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
        Assert.That(() => _dbPersonRepository.GetOrCreateUser(claimsPrincipal), 
            Throws.TypeOf<InvalidOperationException>().With.Message.EqualTo("User's claim is missing nickname field"));
    }
    
    [Test]
    public void GetOrCreateCurrentUser_NickNameDoesNotContainDot_ThrowsException()
    {
        var claims = new[]
        {
            new Claim("name", "new.person@test.com"),
            new Claim("nickname", "NewPerson")
        };
        var claimsIdentity = new ClaimsIdentity(claims, "TestAuthType");
        var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
        Assert.That(() => _dbPersonRepository.GetOrCreateUser(claimsPrincipal), 
            Throws.TypeOf<InvalidOperationException>().With.Message.EqualTo("Nickname in User's claim has unexpected formating (does not contain '.')"));
    }
}
