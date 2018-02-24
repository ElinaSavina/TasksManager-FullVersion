using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using TasksManager.DataAccess.Interfaces.Projects;
using TasksManager.Entities;
using TasksManager.ViewModel.Projects;
using Task = TasksManager.Entities.Task;

namespace TasksManager.DataAccess.Dapper.Projects
{
    internal class ProjectQuery : IProjectQuery
    {
        private readonly IConnectionFactory _connection;

        public ProjectQuery(IConnectionFactory connection)
        {
            _connection = connection;
        }

        public async Task<ProjectResponse> RunAsync(int projectId)
        {
            using (var con = _connection.GetConnection())
            {
                var project = new Project {Tasks = new List<Task>()};
                var cmd = "select * from Projects p left join Tasks t on t.ProjectId=p.Id Where p.Id=@Id";
                //con.QueryFirstOrDefaultAsync<>()
                var result = con.Query<Project, Task, Project>(cmd, (pr, task) =>
                {
                    if (pr == null) return null;
                    if (project.Id != pr.Id)
                    {
                        project.Id = pr.Id;
                        project.Name = pr.Name;
                        project.Description = pr.Description;
                    }
                    if (task != null)
                    {
                        project.Tasks.Add(task);
                    }
                    return project;
                }, new { Id = projectId }).FirstOrDefault();

                return result == null
                    ? null
                    : new ProjectResponse
                    {
                        Id = project.Id,
                        Name = project.Name,
                        Description = project.Description,
                        OpenTasksCount = project.OpenTasksCount
                    };
            }
        }
    }
}
