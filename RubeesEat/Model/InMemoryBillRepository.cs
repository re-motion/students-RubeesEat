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
}
