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
}
