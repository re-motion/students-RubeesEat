using System.Diagnostics.CodeAnalysis;
using System.Net.Mail;
using System.Text.RegularExpressions;

namespace RubeesEat.Model;

public class Person
{
    public static Person Create(string firstName, string lastName, string loginName, string email) => new(Guid.NewGuid(), firstName, lastName, loginName, email);
    public Guid Id { get; }
    public string FirstName { get; }
    public string LastName { get; }
    [MemberNotNullWhen(true, nameof(LoginName))]
    public bool IsActive { get; }
    public string? LoginName { get; }
    public string? Email { get; }

    public Person(Guid id, string firstName, string lastName, string? loginName, string? email, bool isActive = true)
    {
        ArgumentNullException.ThrowIfNull(firstName);
        ArgumentNullException.ThrowIfNull(lastName);
        if (email != null && !MailAddress.TryCreate(email, out _))
        {
            throw new ArgumentException("Invalid email address.");
        }

        Id = id;
        FirstName = firstName;
        LastName = lastName;
        IsActive = isActive;
        LoginName = loginName;
        Email = email;
    }

    private static bool IsValidName(params string[] words)
    {
        return words.All(s => Regex.IsMatch(s, @"^[a-zA-Z]+$"));
    }
    
    public override string ToString()
    {
        return $"{FirstName} {LastName} (ID: {Id}; Logon: '{LoginName}'; Email: {Email})";
    }
}
