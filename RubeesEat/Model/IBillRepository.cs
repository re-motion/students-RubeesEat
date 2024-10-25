namespace RubeesEat.Model;

public interface IBillRepository
{
    IReadOnlyList<Bill> GetAll();
    void Add(Bill bill);
    IReadOnlyList<Bill> GetAllForUser(Person user);
    PaginatedView<BalanceChange> GetRecentBalanceChanges(Person currentUser, int page, int pageSize);
    decimal GetBalance(Person user);
    Bill? GetById(Guid guid);
    void Update(Bill bill);
}
