namespace RubeesEat.Model.DB;

public class DbPersonRepository(IDbConnectionFactory connectionFactory) : IPersonRepository
{
    public IReadOnlyList<Person> GetAll()
    {
        using var connection = connectionFactory.CreateDbConnection();
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
        using var connection = connectionFactory.CreateDbConnection();
        using var command = connection.CreateCommand("INSERT INTO Persons (PersonID, FirstName, LastName) VALUES (@PersonID, @FirstName, @LastName);");
        command.AddParameter("@PersonID", person.Id);
        command.AddParameter("@FirstName", person.FirstName);
        command.AddParameter("@LastName", person.LastName);
        
        connection.Open();
        command.ExecuteNonQuery();
    }
    
    public Person GetCurrentUser()
    {
        return GetAll()[^1];
    }

    public Person? GetById(Guid id)
    {
        using var connection = connectionFactory.CreateDbConnection();
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
