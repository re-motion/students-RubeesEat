
namespace RubeesEat.Model;

public class InMemoryPersonRepository : IPersonRepository
{
    private readonly IDictionary<Guid, Person> _persons;
    private Guid CurrentUser { get; }

    public InMemoryPersonRepository(Guid defaultUser)
    {
        CurrentUser = defaultUser;
        _persons = new Dictionary<Guid, Person>();
    }

    public InMemoryPersonRepository(Guid defaultUser, IDictionary<Guid, Person> storage)
    {
        CurrentUser = defaultUser;
        _persons = storage;
    }

    public IReadOnlyList<Person> GetAll()
    {
        return _persons.Values.ToList();
    }

    public void Add(Person person)
    {
        ArgumentNullException.ThrowIfNull(person);

        _persons.Add(person.Id, person);
    }

    public Person GetCurrentUser()
    {
        return _persons.TryGetValue(CurrentUser, out var person) 
            ? person 
            : throw new ArgumentException("default User is not in the dictionary");
    }

    public Person? GetById(Guid id)
    {
        return _persons.TryGetValue(id, out var person)
            ? person
            : null;
    }
}
