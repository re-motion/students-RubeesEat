using System.Diagnostics.CodeAnalysis;

namespace RubeesEat.Model.EqualityComparer;

public class PersonPropertyEqualityComparer : IEqualityComparer<Person>
{
    public bool Equals(Person? x, Person? y)
    {
        if (ReferenceEquals(x, y))
            return true;
        if (x == null || y == null)
            return false;
        return x.Id == y.Id &&
               x.FirstName == y.FirstName &&
               x.LastName == y.LastName;
    }

    public int GetHashCode([DisallowNull] Person person)
    {
        ArgumentNullException.ThrowIfNull(person);
        var hash = new HashCode();
        hash.Add(person.Id);
        hash.Add(person.FirstName);
        hash.Add(person.LastName);

        return hash.ToHashCode();
    }
}
