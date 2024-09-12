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
    
    public override bool Equals(object? obj)
    {
        if (obj is not EntryLine other)
            return false;

        return Person.Equals(other.Person) &&
               Amount == other.Amount;
    }
     
    public override int GetHashCode()
    {
        return HashCode.Combine(Person, Amount);
    }
    
    public override string ToString()
    {
        return $"EntryLine: Person = {Person}, Amount = {Amount}";
    }
    
    public static bool operator == (EntryLine? left, EntryLine? right)
    {
        if (left is null)
            return right is null;
        return left.Equals(right);
    }

    public static bool operator != (EntryLine? left, EntryLine? right) => !(left == right);
}
