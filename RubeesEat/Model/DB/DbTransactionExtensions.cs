using System.Data;
using System.Data.Common;

namespace RubeesEat.Model.DB;

public static class DbTransactionExtensions
{
    public static DbCommand? CreateCommand(this DbTransaction transaction, string sql)
    {
        var command = transaction.Connection!.CreateCommand();
        command.CommandText = sql;
        command.CommandType = CommandType.Text;
        command.Connection = transaction.Connection;
        command.Transaction = transaction;
        return command;
    }
}
