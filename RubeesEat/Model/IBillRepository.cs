namespace RubeesEat.Model;

public interface IBillRepository
{
    IReadOnlyList<Bill> GetAll();
    void Add(Bill bill);
    IReadOnlyList<Bill> GetAllForUser(Person user);
    IReadOnlyList<BalanceChange> GetRecentBalanceChanges(Person user, int amount);
    decimal GetBalance(Person user);
    Bill? GetById(Guid guid);
}
