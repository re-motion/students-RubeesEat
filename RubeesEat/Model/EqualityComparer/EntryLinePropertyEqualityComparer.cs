using System.Diagnostics.CodeAnalysis;

namespace RubeesEat.Model.EqualityComparer;

public class EntryLinePropertyEqualityComparer : IEqualityComparer<EntryLine>
{
    private PersonPropertyEqualityComparer _personPropertyComparer = new();
    public bool Equals(EntryLine? x, EntryLine? y)
    {
        if (ReferenceEquals(x, y))
            return true;
        if (x == null || y == null)
            return false;
        return _personPropertyComparer.Equals(x.Person, y.Person) &&
               x.Amount == y.Amount;
    }

    public int GetHashCode([DisallowNull] EntryLine entryLine)
    {
        ArgumentNullException.ThrowIfNull(entryLine);
        var hash = new HashCode();
        hash.Add(entryLine.Person);
        hash.Add(entryLine.Amount);

        return hash.ToHashCode();
    }
}
