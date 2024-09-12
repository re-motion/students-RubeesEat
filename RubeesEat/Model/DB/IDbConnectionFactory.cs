using System;
using System.Data.Common;

namespace RubeesEat.Model.DB;

public interface IDbConnectionFactory
{
    DbConnection CreateDbConnection();
}
