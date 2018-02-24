using System.Linq;
using System.Threading.Tasks;
using AutoMapper.QueryableExtensions;
using TaskManager.DataAccess.UnitOfWork;
using TasksManager.DataAccess.Interfaces.Tasks;
using TasksManager.ViewModel;
using TasksManager.ViewModel.Tasks;

namespace TasksManager.DataAccess.DbImplementation.Tasks
{
    public class TaskListQuery : ITaskListQuery
    {
        private readonly IUnitOfWork _uow;
        private readonly IAsyncQueryableFactory _factory;

        public TaskListQuery(IUnitOfWork uow, IAsyncQueryableFactory factory)
        {
            _uow = uow;
            _factory = factory;
        }

        public async Task<ListResponse<TaskResponse>> RunAsync(TaskFilter filter, ListOptions options)
        {
            int totalCount = 0;
            var tasks = _uow.Tasks.Query(t => t.Tags, t => t.Project.Tasks, t => t.Project.User);
            foreach (var task in tasks)
            {
                task.Tags = _uow.TagsInTasks.Query(t => t.Tag).Where(t => t.TaskId == task.Id).ToList();
            }

            await _factory.CreateAsyncQueryable(tasks).LoadAsync();
            var query = tasks.ProjectTo<TaskResponse>();

            query = ApplyFilter(query, filter);
            totalCount = query.Count();
            //после фильтрации по тегу происходит какая-то магия и ассинхронные функции перестают работать
            //totalCount = await query.CountAsync();


            if (options.Sort == null)
            {
                options.Sort = "Id";
            }

            query = options.ApplySort(query);
            query = options.ApplyPaging(query);


            return new ListResponse<TaskResponse>
            {
                Items = query.ToList(),
                Page = options.Page,
                PageSize = options.PageSize ?? totalCount,
                Sort = options.Sort,
                TotalItemsCount = totalCount
            };
        }

        private IQueryable<TaskResponse> ApplyFilter(IQueryable<TaskResponse> query, TaskFilter filter)
        {
            if (filter.Name != null)
            {
                query = query.Where(t => t.Name.StartsWith(filter.Name));
            }

            if (filter.CompleteDate != null)
            {
                if (filter.CompleteDate.From != null)
                {
                    query = query.Where(t => t.CompleteDate >= filter.CompleteDate.From);
                }

                if (filter.CompleteDate.To != null)
                {
                    query = query.Where(t => t.CompleteDate <= filter.CompleteDate.To);
                }
            }

            if (filter.CreateDate != null)
            {
                if (filter.CreateDate.From != null)
                {
                    query = query.Where(t => t.CreateDate >= filter.CreateDate.From);
                }

                if (filter.CreateDate.To != null)
                {
                    query = query.Where(t => t.CreateDate <= filter.CreateDate.To);
                }
            }

            if (filter.DueDate != null)
            {
                if (filter.DueDate.From != null)
                {
                    query = query.Where(t => t.DueDate >= filter.DueDate.From);
                }

                if (filter.DueDate.To != null)
                {
                    query = query.Where(t => t.DueDate <= filter.DueDate.To);
                }
            }

            if (filter.Status != null)
            {
                query = query.Where(t => t.Status == filter.Status);
            }

            if (filter.ProjectId != null)
            {
                query = query.Where(t => t.Project.Id == filter.ProjectId);
            }

            if (filter.Tag != null)
            {
                query = query.Where(t => t.Tags.Contains(filter.Tag));
            }

            if (filter.HasDueDate != null)
            {
                query = query.Where(t => t.DueDate != null);
            }

            return query;
        }
    }
}
