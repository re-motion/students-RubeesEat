using System.Data;
using System.Data.Common;
namespace RubeesEat.Model.DB;

public static class DbConnectionExtensions
{
    public static DbCommand CreateCommand(this DbConnection connection, string sql)
    {
        var command = connection.CreateCommand();
        command.CommandText = sql;
        command.CommandType = CommandType.Text;
        return command;
    }
}
