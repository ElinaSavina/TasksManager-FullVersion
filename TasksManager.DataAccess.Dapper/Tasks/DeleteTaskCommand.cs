using System.Threading.Tasks;
using Dapper;
using TasksManager.DataAccess.Interfaces.Tasks;

namespace TasksManager.DataAccess.Dapper.Tasks
{
    internal class DeleteTaskCommand : IDeleteTaskCommand
    {
        private readonly IConnectionFactory _connection;

        public DeleteTaskCommand(IConnectionFactory connection)
        {
            _connection = connection;
        }

        public async Task ExecuteAsync(int taskId)
        {using (var con = _connection.GetConnection())
            {
                var queryDeleteTask = "DELETE from Tasks where Id=@Id";
                var queryDeleteTagsInTask = "DELETE from TagsInTasks where TaskId=@TaskId";
                using (var tr = con.BeginTransaction())
                {
                    con.Execute(queryDeleteTask, new { Id = taskId }, tr);
                    con.Execute(queryDeleteTagsInTask, new {TaskId = taskId}, tr);
                    tr.Commit();
                }
            }
        }
    }
}
