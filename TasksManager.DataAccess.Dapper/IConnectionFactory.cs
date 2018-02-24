using System.Data;

namespace TasksManager.DataAccess.Dapper
{
    public interface IConnectionFactory
    {
        IDbConnection GetConnection();
    }
}
