namespace RubeesEat.Model;

public interface IBillRepository
{
    IReadOnlyList<Bill> GetAll();
    void Add(Bill bill);
}
