using System.Linq;
using System.Threading.Tasks;
using AutoMapper.QueryableExtensions;
using TaskManager.DataAccess.UnitOfWork;
using TasksManager.DataAccess.Interfaces.Tags;
using TasksManager.ViewModel;
using TasksManager.ViewModel.Tags;

namespace TasksManager.DataAccess.DbImplementation.Tags
{
    public class TagsListQuery : ITagsListQuery
    {
        private readonly IUnitOfWork _uow;
        private readonly IAsyncQueryableFactory _factory;

        public TagsListQuery(IUnitOfWork uow, IAsyncQueryableFactory factory)
        {
            _uow = uow;
            _factory = factory;
        }

        public async Task<ListResponse<TagResponse>> RunAsync(TagFilter filter, ListOptions options)
        {
            var tags = _uow.Tags.Query(t => t.Tasks);
            foreach(var tag in tags)
            {
                tag.Tasks = _uow.TagsInTasks.Query(t => t.Task).Where(t => t.TagId == tag.Id).ToList();
            }
            
            await _factory.CreateAsyncQueryable(tags).LoadAsync();
            var query = tags.ProjectTo<TagResponse>();

            query = ApplyFilter(query, filter);
            int totalCount = await _factory.CreateAsyncQueryable(query).CountAsync();

            if (options.Sort == null)
            {
                options.Sort = "Name";
            }
           
            query = options.ApplySort(query);
           
            query = options.ApplyPaging(query);
            
            return new ListResponse<TagResponse>
            {
                Items = await _factory.CreateAsyncQueryable(query).ToListAsync(),
                Page = options.Page,
                PageSize = options.PageSize ?? totalCount,
                Sort = options.Sort,
                TotalItemsCount = totalCount
            };
        }

        private IQueryable<TagResponse> ApplyFilter(IQueryable<TagResponse> query, TagFilter filter)
        {
            if (filter.Name != null)
            {
                query = query.Where(t => t.Name.StartsWith(filter.Name));
            }
            return query;
        }
    }
}

