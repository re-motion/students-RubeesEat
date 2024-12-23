using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using RubeesEat.Model;

namespace RubeesEat.UnitTests.Model.InMemory;

[TestFixture]
public class InMemoryPersonRepositoryTests
{
    [Test]
    public void GetAll_EntriesInDictionary_ReturnsEntries()
    {
        var storage = new Dictionary<Guid, Person>();
        storage.Add(TestDomain.Jack.Id, TestDomain.Jack);
        storage.Add(TestDomain.Erwin.Id, TestDomain.Erwin);
        storage.Add(TestDomain.Sasha.Id, TestDomain.Sasha);
        var personRepository = new InMemoryPersonRepository(TestDomain.Jack.Id, storage);

        var allEntries = personRepository.GetAll();

        Assert.That(allEntries.Count, Is.EqualTo(3));
        Assert.That(allEntries[0].Id, Is.EqualTo(TestDomain.Jack.Id));
        Assert.That(allEntries[1].Id, Is.EqualTo(TestDomain.Erwin.Id));
        Assert.That(allEntries[2].Id, Is.EqualTo(TestDomain.Sasha.Id));
    }

    [Test]
    public void Add_NoEntriesInDictionary_EntriesSuccessfullyAdded()
    {
        var storage = new Dictionary<Guid, Person>();
        var personRepository = new InMemoryPersonRepository(TestDomain.Jack.Id, storage);

        personRepository.Add(TestDomain.Jack);
        personRepository.Add(TestDomain.Erwin);
        personRepository.Add(TestDomain.Sasha);
		
        Assert.That(personRepository.GetById(TestDomain.Jack.Id),Is.EqualTo(TestDomain.Jack));
        Assert.That(personRepository.GetById(TestDomain.Erwin.Id),Is.EqualTo(TestDomain.Erwin));
        Assert.That(personRepository.GetById(TestDomain.Sasha.Id),Is.EqualTo(TestDomain.Sasha));
    }

    [Test]
    public void GetCurrentUser_IsInDictionary_ReturnsCurrentUser()
    {
        Person defaultUser = new Person(Guid.NewGuid(), "DefaultFirstName", "DefaultLastName", "DefaultLoginName", "Default@bla");

        var personRepository = new InMemoryPersonRepository(defaultUser.Id);
        personRepository.Add(defaultUser);

        Assert.That(personRepository.GetOrCreateUser(new ClaimsPrincipal()),
            Is.EqualTo(defaultUser));
    }

    [Test]
    public void GetCurrentUser_IsNotInDictionary_ThrowsException()
    {
        Person defaultUser = new Person(Guid.NewGuid(), "DefaultFirstName", "DefaultLastName", "DefaultLoginName", "Default@bla");

        var personRepository = new InMemoryPersonRepository(defaultUser.Id);

        Assert.That(
            () => personRepository.GetOrCreateUser(new ClaimsPrincipal()),
            Throws.TypeOf<ArgumentException>().With.Message
                  .EqualTo("default User is not in the dictionary"));
    }

    [Test]
    public void GetById_IsInDictionary_ReturnsUser()
    {
        Person defaultUser = new Person(Guid.NewGuid(), "DefaultFirstName", "DefaultLastName", "DefaultLoginName", "Default@bla");

        var personRepository = new InMemoryPersonRepository(defaultUser.Id);
        personRepository.Add(defaultUser);
        Assert.That(personRepository.GetById(defaultUser.Id), Is.EqualTo(defaultUser));
    }
    
    [Test]
    public void GetById_IsNotInDictionary_ReturnsNull()
    {
        Person defaultUser = new Person(Guid.NewGuid(), "DefaultFirstName", "DefaultLastName", "DefaultLoginName", "Default@bla");

        var personRepository = new InMemoryPersonRepository(defaultUser.Id);
        Assert.That(personRepository.GetById(defaultUser.Id), Is.Null);
    }
}
