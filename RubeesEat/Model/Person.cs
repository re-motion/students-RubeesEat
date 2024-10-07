using System.Text.RegularExpressions;

namespace RubeesEat.Model;

public class Person
{
    public static Person Create(string firstName, string lastName) => new(Guid.NewGuid(), firstName, lastName, true);
    public Guid Id { get; }
    public string FirstName { get; }
    public string LastName { get; }
    public bool IsActive { get; }

    public Person(Guid id, string firstName, string lastName, bool isActive = true)
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
        IsActive = isActive;
    }

    private static bool IsValidName(params string[] words)
    {
        return words.All(s => Regex.IsMatch(s, @"^[a-zA-Z]+$"));
    }
    
    public override string ToString()
    {
        return $"{FirstName} {LastName} (ID: {Id})";
    }
}
