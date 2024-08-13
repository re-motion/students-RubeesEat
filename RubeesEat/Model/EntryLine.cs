namespace RubeesEat.Model;

public class EntryLine
{
    public Person Person { get; }
    public decimal Amount { get; }

    public EntryLine(Person person, decimal amount)
    {
        ArgumentNullException.ThrowIfNull(person);
        Person = person;
        Amount = amount;
    }
}
