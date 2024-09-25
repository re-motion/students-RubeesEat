using System.Data;
using Microsoft.Extensions.Configuration;
using RubeesEat.Model.DB;

namespace RubeesEat.IntegrationTests;

[Explicit("does not run on teamcity")]
public abstract class DatabaseIntegrationTestBase
{
    protected SqlConnectionFactory Factory;
    private readonly string _connectionStringName = "DefaultConnectionString";
    private IConfiguration _configuration;

    [SetUp]
    public void SetUp()
    {
        var builder = new ConfigurationBuilder()
                      .SetBasePath(Directory.GetCurrentDirectory()) 
                      .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                      .AddEnvironmentVariables();

        _configuration = builder.Build();
        
        string filePath = "./CreateTestDb.sql";
        string sqlContent = File.ReadAllText(filePath);
        string[] sqlCommands = sqlContent.Split([';'], StringSplitOptions.RemoveEmptyEntries);

        for (int i = 0; i < sqlCommands.Length; i++)
        {
            sqlCommands[i] = sqlCommands[i].Trim();
        }

        var connectionString = _configuration["ConnectionStrings:TestConnectionString"];
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
