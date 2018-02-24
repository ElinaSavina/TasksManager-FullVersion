using System.Linq;
using System.Threading.Tasks;
using Dapper;
using TasksManager.DataAccess.Interfaces.Projects;
using TasksManager.ViewModel.Projects;

namespace TasksManager.DataAccess.Dapper.Projects
{
    internal class CreateProjectCommand : ICreateProjectCommand
    {
        private readonly IConnectionFactory _connection;

        public CreateProjectCommand(IConnectionFactory connection)
        {
            _connection = connection;
        }
        
        public async Task<ProjectResponse> ExecuteAsync(CreateProjectRequest request)
        {
            using (var con = _connection.GetConnection())
            {
                var cmd = @"insert into Projects (Name, Description) values(@Name, @Description);
                            SELECT CAST(SCOPE_IDENTITY() as int)";
                int projectId;
                using (var tr = con.BeginTransaction())
                {
                    projectId = con.Query<int>(cmd, request, tr).Single();
                    tr.Commit();
                }
                return new ProjectResponse
                {
                    Id = projectId,
                    Name = request.Name,
                    Description = request.Description
                };
            }
        }
    }
}
