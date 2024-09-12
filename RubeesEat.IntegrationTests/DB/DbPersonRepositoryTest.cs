using RubeesEat.Model;
using RubeesEat.Model.DB;

namespace RubeesEat.IntegrationTests.DB;

[TestFixture]
[Explicit("does not run on teamcity")]
public class DbPersonRepositoryTest : DatabaseIntegrationTestBase
{
    private DbPersonRepository _dbPersonRepository;
    
    [SetUp]
    public void Setup()
    {
        _dbPersonRepository = new DbPersonRepository(Factory);
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
        Person person = new Person(personId, "Joel", "Fredericka");
        _dbPersonRepository.Add(person);
        var persons = _dbPersonRepository.GetAll().ToList();
        Assert.That(persons[1].FirstName, Is.EqualTo("Joel"));
        Assert.That(persons[1].LastName, Is.EqualTo("Fredericka"));
        Assert.That(persons[1].Id, Is.EqualTo(personId));
    }

    [Test]
    public void GetCurrentUser_ReturnsCurrentUser()
    {
        var person = _dbPersonRepository.GetCurrentUser();
        Assert.That(person.FirstName, Is.EqualTo("Patrick"));
        Assert.That(person.LastName, Is.EqualTo("Widener"));
        Assert.That(person.Id, Is.EqualTo(Guid.Parse("883703f3-eea8-4bce-bacd-4a77ffe0c294")));
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
}
