using System.Data;
using System.Net.Mail;
using System.Security.Claims;

namespace RubeesEat.Model.DB;

public class DbPersonRepository: IPersonRepository
{
    private readonly IDbConnectionFactory _connectionFactory;
    
    public DbPersonRepository(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }
    
    public IReadOnlyList<Person> GetAll()
    {
        using var connection = _connectionFactory.CreateDbConnection();
        using var command = connection.CreateCommand("SELECT PersonID, FirstName, LastName, LoginName, Email, IsActive FROM Persons ORDER BY LastName ASC;");
        connection.Open();

        using var reader = command.ExecuteReader();
        
        var persons = new List<Person>();

        while (reader.Read())
        {
            persons.Add(new Person
            (
                reader.GetGuid(0),
                reader.GetString(1),
                reader.GetString(2),
                reader.IsDBNull(3) ? null : reader.GetString(3),
                reader.IsDBNull(4) ? null : reader.GetString(4),
                reader.GetBoolean(5)
            ));
        }

        return persons.AsReadOnly();
    }

    public IReadOnlyList<Person> GetAllActive()
    {
        using var connection = _connectionFactory.CreateDbConnection();
        using var command = connection.CreateCommand("SELECT PersonID, FirstName, LastName, LoginName, Email, IsActive FROM Persons WHERE IsActive = 1 ORDER BY LastName ASC;");
        connection.Open();

        using var reader = command.ExecuteReader();
        
        var persons = new List<Person>();

        while (reader.Read())
        {
            persons.Add(new Person
            (
                reader.GetGuid(0),
                reader.GetString(1),
                reader.GetString(2),
                reader.IsDBNull(3) ? null : reader.GetString(3),
                reader.IsDBNull(4) ? null : reader.GetString(4),
                reader.GetBoolean(5)
            ));
        }

        return persons.AsReadOnly();
    }

    public void Add(Person person)
    {
        using var connection = _connectionFactory.CreateDbConnection();
        using var command = connection.CreateCommand("INSERT INTO Persons (PersonID, FirstName, LastName, LoginName, Email, IsActive) VALUES (@PersonID, @FirstName, @LastName, @LoginName, @Email, @IsActive);");
        command.AddParameter("@PersonID", person.Id);
        command.AddParameter("@FirstName", person.FirstName);
        command.AddParameter("@LastName", person.LastName);
        command.AddParameter("@LoginName", person.LoginName);
        command.AddParameter("@Email", person.Email);
        command.AddParameter("@IsActive", person.IsActive);
        
        connection.Open();
        command.ExecuteNonQuery();
    }
    
    public Person GetOrCreateUser(ClaimsPrincipal user)
    {
        const string domain = "RUBICON\\";

        var loginNameWithDomain = user.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name")?.Value;
        if (loginNameWithDomain == null)
            throw new InvalidOperationException("Cannot find the required claims on the user.");
        if (!loginNameWithDomain.StartsWith(domain))
            throw new InvalidOperationException("Invalid user account. Not in domain.");

        var loginName = loginNameWithDomain[domain.Length..];
        var person = GetByLoginName(loginName);
        if (person != null)
            return person;

        var email = user.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/upn")?.Value;
        Console.WriteLine($"Person '{loginName}' does not exist. Creating with email '{email}'");

        if (email == null)
            throw new InvalidOperationException("Cannot create account. No Email found.");
        if (!MailAddress.TryCreate(email, out _))
            throw new InvalidOperationException($"Cannot create account. Invalid Email '{email}'.");

        var nameParts = email.Split('@')[0].Split('.');

        person = new Person(
            Guid.NewGuid(),
            ToPascalCase(nameParts[0]),
            nameParts.Length > 0 ? ToPascalCase(nameParts[1]) : "",
            loginName,
            email);
        Add(person);

        return person;
    }

    public Person? GetById(Guid id)
    {
        using var connection = _connectionFactory.CreateDbConnection();
        using var command =
            connection.CreateCommand(
                "SELECT PersonID, FirstName, LastName, LoginName, Email, IsActive FROM Persons WHERE PersonID = @PersonID;");
        command.AddParameter("@PersonID", id);

        connection.Open();
        using var reader = command.ExecuteReader();
        if (reader.Read())
            return MapToPerson(reader);
        return null;
    }

    private Person? GetByLoginName(string loginName)
    {
        using var connection = _connectionFactory.CreateDbConnection();
        using var command =
            connection.CreateCommand(
                "SELECT PersonID, FirstName, LastName, LoginName, Email, IsActive FROM Persons WHERE LoginName = @LoginName;");
        command.AddParameter("@LoginName", loginName);

        connection.Open();
        using var reader = command.ExecuteReader();
        if (reader.Read())
        {
            return MapToPerson(reader);
        }

        return null;
    }

    private Person MapToPerson(IDataReader reader)
    {
        return new Person
        (
            reader.GetGuid(0),
            reader.GetString(1),
            reader.GetString(2),
            reader.IsDBNull(3) ? null : reader.GetString(3),
            reader.IsDBNull(4) ? null : reader.GetString(4),
            reader.GetBoolean(5)
        );
    }

    private string ToPascalCase(string value)
    {
        if (value.Length == 0)
            return value;

        return char.IsUpper(value[0])
            ? value
            : char.ToUpper(value[0]) + value[1..];
    }
}
