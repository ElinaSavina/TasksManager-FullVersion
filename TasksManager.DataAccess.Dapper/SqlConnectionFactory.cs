using System.Data;
using System.Data.SqlClient;

namespace TasksManager.DataAccess.Dapper
{
    public class SqlConnectionFactory : IConnectionFactory
    {
        private readonly string _connection;
        public SqlConnectionFactory(string connectionString)
        {
            _connection = connectionString;
        }

        public IDbConnection GetConnection()
        {
            var connection = new SqlConnection(_connection);
            connection.Open();
            return connection;
        }
    }
}
