using System.Linq;
using Dapper;
using TasksManager.DataAccess.Interfaces.Projects;
using Task = System.Threading.Tasks.Task;

namespace TasksManager.DataAccess.Dapper.Projects
{
    internal class DeleteProjectCommand : IDeleteProjectCommand
    {
        private readonly IConnectionFactory _connection;

        public DeleteProjectCommand(IConnectionFactory connection)
        {
            _connection = connection;
        }

        public async Task ExecuteAsync(int projectId)
        {
            using (var con = _connection.GetConnection())
            {
                var cmd1 = "DELETE from Projects where Id=@Id";
                var cmd = "SELECT * from Tasks where ProjectId=@pId";
                using (var tr = con.BeginTransaction())
                {
                    var task = con.Query<Task>(cmd, new {pId = projectId}, tr).FirstOrDefault();
                    if (task != null) throw new CannotDeleteProjectWithTasksException();
                    con.Execute(cmd1, new {Id = projectId}, tr);
                  
                    tr.Commit();
                }
            }
        }
    }
}
