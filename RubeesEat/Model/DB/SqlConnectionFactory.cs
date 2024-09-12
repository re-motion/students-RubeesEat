using System.Data.Common;
using Microsoft.Data.SqlClient;

namespace RubeesEat.Model.DB;

public class SqlConnectionFactory : IDbConnectionFactory
{
    private readonly string _connectionStringName;
    private readonly IConfiguration _configuration;
    private string? _connectionString; 
    private string ConnectionString => _connectionString ??= _configuration.GetConnectionString(_connectionStringName);


    public SqlConnectionFactory(IConfiguration configuration, string connectionStringName)
    {
        _connectionStringName = connectionStringName;
        _configuration = configuration;
    }
    
    public SqlConnectionFactory(string connectionString)
    {
        _connectionString = connectionString;
    }
    
    public DbConnection CreateDbConnection()
    {
        return new SqlConnection(ConnectionString);
    }
}
