using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using TasksManager.DataAccess.Interfaces.Projects;
using TasksManager.Entities;
using TasksManager.ViewModel;
using TasksManager.ViewModel.Projects;
using Task = TasksManager.Entities.Task;

namespace TasksManager.DataAccess.Dapper.Projects
{
    internal class ProjectsListQuery : IProjectsListQuery
    {
        private readonly IConnectionFactory _connection;

        public ProjectsListQuery(IConnectionFactory connection)
        {
            _connection = connection;
        }

        public async Task<ListResponse<ProjectResponse>> RunAsync(ProjectFilter filter, ListOptions options)
        {
            using (var con = _connection.GetConnection())
            {
                var projectsDictionary = new Dictionary<int, Project>();

                var cmd = "select * from Projects p left join Tasks t on p.Id=t.ProjectId";
                con.Query<Project, Task, Project>(cmd, (pr, task) =>
                {
                    Project project;
                    if (!projectsDictionary.TryGetValue(pr.Id, out project))
                    {
                        project = pr;
                        project.Tasks = new List<Task>();
                        projectsDictionary.Add(project.Id, project);
                    }

                    project = projectsDictionary[pr.Id];
                    if (task != null)
                        project.Tasks.Add(task);

                    return project;
                });

                var projects = projectsDictionary.Values.ToList();
                var prs = new List<ProjectResponse>();
                foreach (var pr in projects)
                {
                    prs.Add(new ProjectResponse
                    {
                        Id = pr.Id,
                        Name = pr.Name,
                        Description = pr.Description,
                        OpenTasksCount = pr.OpenTasksCount
                    });
                }
                IQueryable<ProjectResponse> query = prs.AsQueryable();

                query = ApplyFilter(query, filter);

                int totalCount = query.Count();

                if (options.Sort == null)
                {
                    options.Sort = "Id";
                }

                query = options.ApplySort(query);
                query = options.ApplyPaging(query);

                return new ListResponse<ProjectResponse>
                {
                    Items = query.ToList(),
                    Page = options.Page,
                    PageSize = options.PageSize ?? totalCount,
                    Sort = options.Sort,
                    TotalItemsCount = totalCount
                };
            }
        }

        private IQueryable<ProjectResponse> ApplyFilter(IQueryable<ProjectResponse> query, ProjectFilter filter)
        {
            if (filter.Id != null)
            {
                query = query.Where(pr => pr.Id == filter.Id);
            }

            if (filter.Name != null)
            {
                query = query.Where(pr => pr.Name.StartsWith(filter.Name));
            }

            if (filter.OpenTasksCount != null)
            {
                if (filter.OpenTasksCount.From != null)
                {
                    query = query.Where(pr => pr.OpenTasksCount >= filter.OpenTasksCount.From);
                }

                if (filter.OpenTasksCount.To != null)
                {
                    query = query.Where(pr => pr.OpenTasksCount <= filter.OpenTasksCount.To);
                }
            }
            return query;
        }
    }
}
