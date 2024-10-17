using System.Security.Claims;

namespace RubeesEat.Model;

public interface IPersonRepository
{
    IReadOnlyList<Person> GetAll();
    IReadOnlyList<Person> GetAllActive();
    void Add(Person person);
    Person GetOrCreateUser(ClaimsPrincipal user);
    Person? GetById(Guid id);
}
