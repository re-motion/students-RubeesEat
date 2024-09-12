using System.Collections.Immutable;

namespace RubeesEat.Model;

public class BillBuilder
{
    public Guid Id { get; set; }
    public DateTime Date { get; set; }
    public string Description { get; set; } = string.Empty;
    public List<EntryLine> EntryLines { get; } = new List<EntryLine>();

    public BillBuilder()
    {
    }

    public Bill Build()
    {
        return new Bill(Id, Date, Description, EntryLines.ToImmutableArray());
    }
    
}
