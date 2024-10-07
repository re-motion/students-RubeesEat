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
    
    public EntryLine With(decimal? amount = null, Person? person = null)
    {
        return new EntryLine(
            person ?? this.Person,
            amount ?? this.Amount
        );
    }
    
    public override string ToString()
    {
        return $"EntryLine: Person = {Person}, Amount = {Amount}";
    }
}
