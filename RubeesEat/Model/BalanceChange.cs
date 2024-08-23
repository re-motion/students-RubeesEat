using System.Runtime.InteropServices.JavaScript;

namespace RubeesEat.Model;

public class BalanceChange
{
    public decimal Amount { get; }

    public DateTime Date { get; }

    public string Description { get; }

    public BalanceChange(decimal amount, DateTime date, string description)
    {
        ArgumentNullException.ThrowIfNull(description);
        Amount = amount;
        Date = date;
        Description = description;
    }
}
