using System.Diagnostics.CodeAnalysis;

namespace RubeesEat.Model.EqualityComparer;

public class BillPropertyEqualityComparer : IEqualityComparer<Bill>
{
    private EntryLinePropertyEqualityComparer _entryLineComparer = new();
    public bool Equals(Bill? x, Bill? y)
    {
        if (ReferenceEquals(x, y))
            return true;
        if (x == null || y == null)
            return false;
        return x.Id == y.Id &&
               x.Date == y.Date &&
               x.Description == y.Description &&
               x.EntryLines.SequenceEqual(y.EntryLines, _entryLineComparer);
    }

    public int GetHashCode([DisallowNull] Bill bill)
    {
        ArgumentNullException.ThrowIfNull(bill);
        var hash = new HashCode();
        hash.Add(bill.Id);
        hash.Add(bill.Date);
        hash.Add(bill.Description);
        foreach (var entryLine in bill.EntryLines)
        {
            hash.Add(entryLine);
        }

        return hash.ToHashCode();
    }
}
