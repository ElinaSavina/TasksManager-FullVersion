using System.Linq;
using System.Threading.Tasks;
using AutoMapper.QueryableExtensions;
using TaskManager.DataAccess.UnitOfWork;
using TasksManager.DataAccess.Interfaces.Projects;
using TasksManager.ViewModel;
using TasksManager.ViewModel.Projects;

namespace TasksManager.DataAccess.DbImplementation.Projects
{
    internal class ProjectsListQuery : IProjectsListQuery
    {
        private readonly IUnitOfWork _uow;
        private readonly IAsyncQueryableFactory _factory;

        public ProjectsListQuery(IUnitOfWork uow, IAsyncQueryableFactory factory)
        {
            _uow = uow;
            _factory = factory;
        }

        public async Task<ListResponse<ProjectResponse>> RunAsync(ProjectFilter filter, ListOptions options)
        {
            var q = _uow.Projects.Query(t => t.Tasks, t => t.User);
            await _factory.CreateAsyncQueryable(q).LoadAsync();
            var query = q.ProjectTo<ProjectResponse>();            
            
            query = ApplyFilter(query, filter);
            int totalCount = await _factory.CreateAsyncQueryable(query).CountAsync();

            if (options.Sort == null)
            {
                options.Sort = "Id";
            }

            query = options.ApplySort(query);
            query = options.ApplyPaging(query);

            return new ListResponse<ProjectResponse>
            {
                Items = await _factory.CreateAsyncQueryable(query).ToListAsync(),
                Page = options.Page,
                PageSize = options.PageSize ?? totalCount,
                Sort = options.Sort,
                TotalItemsCount = totalCount
            };

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
