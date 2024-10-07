namespace RubeesEat.Model;

public interface IPersonRepository
{
    IReadOnlyList<Person> GetAll();
    IReadOnlyList<Person> GetAllActive();
    void Add(Person person);
    Person GetCurrentUser();
    Person? GetById(Guid id);
}
