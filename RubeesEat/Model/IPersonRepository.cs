namespace RubeesEat.Model;

public interface IPersonRepository
{
    IReadOnlyList<Person> GetAll();
    void Add(Person person);
    Person GetCurrentUser();
}
