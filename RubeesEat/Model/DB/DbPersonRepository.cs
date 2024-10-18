using System.Data;
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
        using var command = connection.CreateCommand("SELECT PersonID, FirstName, LastName, LoginName, IsActive FROM Persons ORDER BY LastName ASC;");
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
                reader.GetBoolean(4)
            ));
        }

        return persons.AsReadOnly();
    }

    public IReadOnlyList<Person> GetAllActive()
    {
        using var connection = _connectionFactory.CreateDbConnection();
        using var command = connection.CreateCommand("SELECT PersonID, FirstName, LastName, LoginName, IsActive FROM Persons WHERE IsActive = 1 ORDER BY LastName ASC;");
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
                reader.GetBoolean(4)
            ));
        }

        return persons.AsReadOnly();
    }

    public void Add(Person person)
    {
        using var connection = _connectionFactory.CreateDbConnection();
        using var command = connection.CreateCommand("INSERT INTO Persons (PersonID, FirstName, LastName, LoginName, IsActive) VALUES (@PersonID, @FirstName, @LastName, @LoginName, @IsActive);");
        command.AddParameter("@PersonID", person.Id);
        command.AddParameter("@FirstName", person.FirstName);
        command.AddParameter("@LastName", person.LastName);
        command.AddParameter("@LoginName", person.LoginName);
        command.AddParameter("@IsActive", person.IsActive);
        
        connection.Open();
        command.ExecuteNonQuery();
    }
    
    public Person GetOrCreateUser(ClaimsPrincipal user)
    {
        var loginName = user.FindFirst("name")?.Value;
        var nickName = user.FindFirst("nickname")?.Value;
        if (loginName == null)
            throw new InvalidOperationException("User's claim is missing name field");
        if (nickName == null)
            throw new InvalidOperationException("User's claim is missing nickname field");
        if (!nickName.Contains('.'))
            throw new InvalidOperationException("Nickname in User's claim has unexpected formating (does not contain '.')");
        var splitNickName = nickName.Split(".");
        var firstName = splitNickName[0];
        var lastName = splitNickName[1];
        var person = GetByLoginName(loginName);
        if (person != null)
            return person;
        person = new Person(Guid.NewGuid(), firstName, lastName, loginName);
        Add(person);
        return person;
    }

    public Person? GetById(Guid id)
    {
        using var connection = _connectionFactory.CreateDbConnection();
        using var command =
            connection.CreateCommand(
                "SELECT PersonID, FirstName, LastName, LoginName, IsActive FROM Persons WHERE PersonID = @PersonID;");
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
                "SELECT PersonID, FirstName, LastName, LoginName FROM Persons WHERE LoginName = @LoginName;");
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
            reader.IsDBNull(3) ? null : reader.GetString(3)
        );
    }
}
