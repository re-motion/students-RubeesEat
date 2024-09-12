using System.Data.Common;

namespace RubeesEat.Model.DB;

public static class DbCommandExtensions
{

    public static DbParameter AddParameter(this DbCommand command, string paramName, Guid paramValue)
    {
        var idParam = command.CreateParameter();
        idParam.ParameterName = paramName;
        idParam.Value = paramValue;
        command.Parameters.Add(idParam);
        return idParam;
    }

    public static DbParameter AddParameter(this DbCommand command, string paramName, decimal paramValue)
    {
        var amountParam = command.CreateParameter();
        amountParam.ParameterName = paramName;
        amountParam.Value = paramValue;
        command.Parameters.Add(amountParam);
        return amountParam;
    }

    public static DbParameter AddParameter(this DbCommand command, string paramName, DateTime paramValue)
    {
        var dateParam = command.CreateParameter();
        dateParam.ParameterName = paramName;
        dateParam.Value = paramValue;
        command.Parameters.Add(dateParam);
        return dateParam;
    }

    public static DbParameter AddParameter(this DbCommand command, string paramName, string paramValue)
    {
        var nameParam = command.CreateParameter();
        nameParam.ParameterName = paramName;
        nameParam.Value = paramValue;
        command.Parameters.Add(nameParam);
        return nameParam;
    }
}
