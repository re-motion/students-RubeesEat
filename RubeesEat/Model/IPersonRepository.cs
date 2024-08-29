using System.Diagnostics.CodeAnalysis;

namespace RubeesEat.Model;

public interface IPersonRepository
{
    IReadOnlyList<Person> GetAll();
    void Add(Person person);
    Person GetCurrentUser();
    bool TryGetById(Guid guid, [NotNullWhen(true)] out Person? person);
}
