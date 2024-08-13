using System.Text.RegularExpressions;

namespace RubeesEat.Model;

public class Person
{
    public static Person Create(string firstName, string lastName) => new(Guid.NewGuid(), firstName, lastName);
    public Guid Id { get; }
    public string FirstName { get; }
    public string LastName { get; }

    public Person(Guid id, string firstName, string lastName)
    {
        ArgumentNullException.ThrowIfNull(firstName);
        ArgumentNullException.ThrowIfNull(lastName);
        if (!IsValidName(firstName, lastName))
        {
            throw new ArgumentException("Name must only consist of letters.");
        }

        Id = id;
        FirstName = firstName;
        LastName = lastName;
    }

    private bool IsValidName(params string[] words)
    {
        return words.All(s => Regex.IsMatch(s, @"^[a-zA-Z]+$"));
    }
}
