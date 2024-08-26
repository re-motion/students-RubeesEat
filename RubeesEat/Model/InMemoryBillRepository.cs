namespace RubeesEat.Model;

public class InMemoryBillRepository : IBillRepository
{
    private readonly IDictionary<Guid, Bill> _bills;

    public InMemoryBillRepository()
    {
        _bills = new Dictionary<Guid, Bill>();
    }

    public InMemoryBillRepository(IDictionary<Guid, Bill> storage)
    {
        _bills = storage;
    }

    public IReadOnlyList<Bill> GetAll()
    {
        return _bills.Values.ToList();
    }

    public Bill? GetById(Guid guid)
    {
        return _bills[guid];
    }

    public void Add(Bill bill)
    {
        ArgumentNullException.ThrowIfNull(bill);
        _bills.Add(bill.Id, bill);
    }

    public IReadOnlyList<Bill> GetAllForUser(Person user)
    {
        return _bills.Values
                     .Where(e => e.EntryLines.Any(f => f.Person == user))
                     .ToList();
    }

    public IReadOnlyList<BalanceChange> GetRecentBalanceChanges(Person user, int amount)
    {
        var billsWithUser = GetAllForUser(user).OrderByDescending(bill => bill.Date);
        var balanceChanges = new List<BalanceChange>();
        int counter = 0;
        foreach (var bill in billsWithUser)
        {
            if (counter >= amount)
                break;
            var sum = bill.EntryLines
                          .Where(entryLine => entryLine.Person.Id == user.Id)
                          .Sum(entryLine => entryLine.Amount);

            balanceChanges.Add(new BalanceChange(sum, bill.Date, bill.Description));
            counter++;
        }

        return balanceChanges;
    }

    public decimal GetBalance(Person user)
    {
        return _bills.Values
                     .SelectMany(e => e.EntryLines)
                     .Where(e => e.Person == user)
                     .Sum(e => e.Amount);
    }
}
