using System.Collections.Immutable;

namespace RubeesEat.Model;

public class Bill
{
    public static Bill Create(string description, EntryLine[] entryLines) =>
        new(Guid.NewGuid(), DateTime.Now, description, [..entryLines]);

    public Guid Id { get; }
    public DateTime Date { get; }
    public string Description { get; }
    public ImmutableArray<EntryLine> EntryLines { get; }

    public Bill(Guid id, DateTime date, string description, ImmutableArray<EntryLine> entryLines)
    {
        ArgumentNullException.ThrowIfNull(description);
        if (entryLines.Length < 2)
        {
            throw new ArgumentException("Entry lines should contain at least 2.");
        }

        if (entryLines.Sum(e => e.Amount) != 0)
        {
            throw new ArgumentException("Sum of entry lines should be 0.");
        }

        Id = id;
        Date = date;
        Description = description;
        EntryLines = entryLines;
    }
    
    public override bool Equals(object? obj)
    {
        if (obj is not Bill other)
        {
            return false;
        }

        return Id == other.Id &&
               Date == other.Date &&
               Description == other.Description &&
               EntryLines.SequenceEqual(other.EntryLines);
    }
    
    public override string ToString()
    {
        return $"Bill: {Id}, Date: {Date}, Description: {Description}, EntryLines: [{string.Join(", ", EntryLines)}]";
    }
    
    public override int GetHashCode()
    {
        var hash = new HashCode();
        hash.Add(Id);
        hash.Add(Date);
        hash.Add(Description);
        foreach (var entryLine in EntryLines)
        {
            hash.Add(entryLine);
        }
        return hash.ToHashCode();
    }
    
    public static bool operator == (Bill? left, Bill? right)
    {
        if (left is null)
            return right is null;
        return left.Equals(right);
    }

    public static bool operator !=(Bill? left, Bill? right) => !(left == right);
}
