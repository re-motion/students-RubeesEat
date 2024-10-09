using System.Security.Claims;

namespace RubeesEat.Model.DB;

public class DbPersonRepository: IPersonRepository
{
    private readonly IHttpContextAccessor? _httpContextAccessor;
    private readonly IDbConnectionFactory _connectionFactory;
    public DbPersonRepository(IHttpContextAccessor httpContextAccessor, IDbConnectionFactory connectionFactory)
    {
        _httpContextAccessor = httpContextAccessor;
        _connectionFactory = connectionFactory;
    }
    public DbPersonRepository(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }
    public IReadOnlyList<Person> GetAll()
    {
        using var connection = _connectionFactory.CreateDbConnection();
        using var command = connection.CreateCommand("SELECT PersonID, FirstName, LastName FROM Persons ORDER BY LastName ASC;");
        connection.Open();

        using var reader = command.ExecuteReader();
        
        var persons = new List<Person>();

        while (reader.Read())
        {
            persons.Add(new Person
            (
                reader.GetGuid(0),
                reader.GetString(1),
                reader.GetString(2)
            ));
        }

        return persons.AsReadOnly();
    }

    public void Add(Person person)
    {
        using var connection = _connectionFactory.CreateDbConnection();
        using var command = connection.CreateCommand("INSERT INTO Persons (PersonID, FirstName, LastName, LoginName) VALUES (@PersonID, @FirstName, @LastName, @LoginName);");
        command.AddParameter("@PersonID", person.Id);
        command.AddParameter("@FirstName", person.FirstName);
        command.AddParameter("@LastName", person.LastName);
        command.AddParameter("@LoginName", person.LoginName);
        
        connection.Open();
        command.ExecuteNonQuery();
    }
    
    public Person GetCurrentUser()
    {
        var httpContext = _httpContextAccessor.HttpContext;
        if (httpContext == null)
            throw new NullReferenceException("HttpContext is null");
        var loginName = httpContext.User.FindFirst("name")?.Value;
        var nickName = httpContext.User.FindFirst("nickname")?.Value;
        var splitNickName = nickName.Split(".");
        var firstName = splitNickName[0];
        var lastName = splitNickName[1];
        if (loginName == null)
            throw new NullReferenceException("User is inactive");

        using var connection = _connectionFactory.CreateDbConnection();
        using var command = connection.CreateCommand("SELECT PersonID, FirstName, LastName FROM Persons WHERE LoginName = @LoginName;");
        command.AddParameter("@LoginName", loginName);

        connection.Open();
        using var reader = command.ExecuteReader();
        if (reader.Read())
        {
            return new Person
            (
                reader.GetGuid(0),
                reader.GetString(1),
                reader.GetString(2)
            );
        }
        var newPerson = new Person(Guid.NewGuid(), firstName, lastName, loginName);
        Add(newPerson);
        return newPerson;
    }

    public Person? GetById(Guid id)
    {
        using var connection = _connectionFactory.CreateDbConnection();
        using var command = connection.CreateCommand("SELECT PersonID, FirstName, LastName FROM Persons WHERE PersonID = @PersonID;");
        command.AddParameter("@PersonID", id);

        connection.Open();
        
        using var reader = command.ExecuteReader();
        if (!reader.Read())
        {
            return null;
        }

        return new Person
        (
            reader.GetGuid(0),
            reader.GetString(1),
            reader.GetString(2)
        );
    }
}
