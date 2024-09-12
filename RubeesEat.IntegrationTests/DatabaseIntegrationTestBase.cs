using System.Data;
using RubeesEat.Model.DB;

namespace RubeesEat.IntegrationTests;

[Explicit("does not run on teamcity")]
public abstract class DatabaseIntegrationTestBase
{
    protected SqlConnectionFactory Factory;
    
    [SetUp]
    public void SetUp()
    {
        string filePath = "./CreateTestDb.sql";
        string sqlContent = File.ReadAllText(filePath);
        string[] sqlCommands = sqlContent.Split([';'], StringSplitOptions.RemoveEmptyEntries);

        // Trimmt leere Zeilen oder unnötige Leerzeichen von jedem Befehl
        for (int i = 0; i < sqlCommands.Length; i++)
        {
            sqlCommands[i] = sqlCommands[i].Trim();
        }

        const string connectionString = "Data Source=localhost;TrustServerCertificate=true;Database=TestDB;Integrated Security=sspi;Connection Timeout=30;";
        Factory = new SqlConnectionFactory(connectionString);
        using var connection = Factory.CreateDbConnection();
        connection.Open();
        foreach (var commandText in sqlCommands)
        {
            using var command = connection.CreateCommand();
            command.CommandText = commandText;
            command.CommandType = CommandType.Text;
            command.ExecuteNonQuery();
        }
    }

    [TearDown]
    public void TearDown()
    {
    }
}
