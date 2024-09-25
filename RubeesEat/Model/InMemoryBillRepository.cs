using System.Collections.Immutable;
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
        return _bills.TryGetValue(guid, out var bill)
            ? bill
            : null;
    }

    public void Add(Bill bill)
    {
        ArgumentNullException.ThrowIfNull(bill);
        _bills.Add(bill.Id, bill);
    }
        
    public PaginatedView<BalanceChange> GetRecentBalanceChanges(Person currentUser, int page, int pageSize)
    {
        var skip = (page - 1) * pageSize;

        var billsOfUser = _bills.Values
                                .Where(b => b.EntryLines.Any(e => e.Person.Id == currentUser.Id))
                                .ToList();

        int totalPages = (int)Math.Ceiling(billsOfUser.Count / (double)pageSize);

        var balanceChanges = billsOfUser
                             .OrderByDescending(b => b.Date)
                             .Skip(skip)
                             .Take(pageSize)
                             .Select(b => GetBalanceChangesForUser(b, currentUser))
                             .ToImmutableArray();
        
        var billsPaginated = new PaginatedView<BalanceChange>(
            balanceChanges,
            page,
            totalPages
        );

        return billsPaginated;
        
        static BalanceChange GetBalanceChangesForUser(Bill bill, Person currentUser)
        {
            var sum = bill.EntryLines
                          .Where(entryLine => entryLine.Person.Id == currentUser.Id)
                          .Sum(entryLine => entryLine.Amount);
            return new BalanceChange(sum, bill.Date, bill.Description, bill.Id);
        }
    }

    public IReadOnlyList<Bill> GetAllForUser(Person user)
    {
        return _bills.Values
                     .Where(e => e.EntryLines.Any(f => f.Person == user))
                     .ToList();
    }

    public decimal GetBalance(Person user)
    {
        return _bills.Values
                     .SelectMany(e => e.EntryLines)
                     .Where(e => e.Person == user)
                     .Sum(e => e.Amount);
    }
}
