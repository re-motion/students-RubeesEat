using System.Runtime.InteropServices.JavaScript;

namespace RubeesEat.Model;

public class BalanceChange
{
    public decimal Amount { get; }

    public DateTime Date { get; }

    public string Description { get; }
    
    public Guid BillId { get; }

    public BalanceChange(decimal amount, DateTime date, string description, Guid billId)
    {
        ArgumentNullException.ThrowIfNull(description);
        Amount = amount;
        Date = date;
        Description = description;
        BillId = billId;
    }
}
