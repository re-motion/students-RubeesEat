using System.Data;
using System.Data.Common;

namespace RubeesEat.Model.DB;

public static class DbTransactionExtensions
{
    public static DbCommand? CreateCommand(this DbTransaction transaction, string sql)
    {
        //Rider says transaction.Connection could be null
        //(because we assign the connection to the transaction outside of this function)
        //should we check for null and return null in that case?
        var command = transaction.Connection?.CreateCommand();
        if (command == null) return null;
        command.CommandText = sql;
        command.CommandType = CommandType.Text;
        command.Connection = transaction.Connection;
        command.Transaction = transaction;
        return command;
    }
}
