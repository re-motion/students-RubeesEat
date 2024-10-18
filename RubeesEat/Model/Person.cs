using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;

namespace RubeesEat.Model;

public class Person
{
    public static Person Create(string firstName, string lastName, string loginName) => new(Guid.NewGuid(), firstName, lastName, loginName);
    public Guid Id { get; }
    public string FirstName { get; }
    public string LastName { get; }
    [MemberNotNullWhen(true, nameof(LoginName))]
    public bool IsActive { get; }
    public string? LoginName { get; }

    public Person(Guid id, string firstName, string lastName, string? loginName, bool isActive = true)
    {
        ArgumentNullException.ThrowIfNull(firstName);
        ArgumentNullException.ThrowIfNull(lastName);
        if (!IsValidName(firstName, lastName))
        {
            throw new ArgumentException("Name must only consist of letters.");
        }

        if (isActive == false && loginName != null)
        {
            throw new ArgumentException("Inactive users should have no login name");
        }

        if (isActive && loginName == null)
        {
            throw new ArgumentException("Active users should have a login name");
        }

        Id = id;
        FirstName = firstName;
        LastName = lastName;
        IsActive = isActive;
        LoginName = loginName;
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
