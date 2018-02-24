using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using TasksManager.DataAccess.Interfaces.Projects;
using TasksManager.Entities;
using TasksManager.ViewModel.Projects;

namespace TasksManager.DataAccess.Dapper.Projects
{
    internal class UpdateProjectCommand : IUpdateProjectCommand
    {
        private readonly IConnectionFactory _connection;

        public UpdateProjectCommand(IConnectionFactory connection)
        {
            _connection = connection;
        }
       
        public async Task<ProjectResponse> ExecuteAsync(int projectId, UpdateProjectRequest request)
        {

            using (var con = _connection.GetConnection())
            {
                var cmd = "UPDATE Projects set Name=@Name,Description=@Description where Id=@Id";
                using (var tr = con.BeginTransaction())
                {
                    con.Execute(cmd, new{request.Name, request.Description, Id = projectId}, tr);
                    tr.Commit();
                }

                var project = new Project { Tasks = new List<Entities.Task>() };
                var cmd1 = "select * from Projects p left join Tasks t on t.ProjectId=p.Id Where p.Id=@Id";

                var result = con.Query<Project, Entities.Task, Project>(cmd1, (pr, task) =>
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
