using System.Data.Common;

namespace RubeesEat.Model.DB;

public static class DbCommandExtensions
{

    public static DbParameter AddParameter(this DbCommand command, string name, Guid value)
    {
        return AddParameterHelper(command, name, value);
    }

    public static DbParameter AddParameter(this DbCommand command, string name, decimal value)
    {
        return AddParameterHelper(command, name, value);
    }

    public static DbParameter AddParameter(this DbCommand command, string name, DateTime value)
    {
        return AddParameterHelper(command, name, value);
    }

    public static DbParameter AddParameter(this DbCommand command, string name, string value)
    {
        return AddParameterHelper(command, name, value);
    }
    
    public static DbParameter AddParameter(this DbCommand command, string name, int value)
    {
        return AddParameterHelper(command, name, value);
    }

    public static DbParameter AddParameter(this DbCommand command, string name, bool isActive)
    {
        return AddParameterHelper(command, name, isActive);
    }

    private static DbParameter AddParameterHelper(this DbCommand command, string name, object value)
    {
        var parameter = command.CreateParameter();
        parameter.ParameterName = name;
        parameter.Value = value;
        command.Parameters.Add(parameter);
        return parameter;
    }
}
