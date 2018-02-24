using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using TaskManager.DataAccess.UnitOfWork;
using TasksManager.DataAccess.Interfaces.Tasks;
using TasksManager.Entities;
using TasksManager.ViewModel.Tasks;

namespace TasksManager.DataAccess.DbImplementation.Tasks
{
    public class AddTagToTaskCommand : IAddTagToTaskCommand
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;
        private readonly IAsyncQueryableFactory _factory;

        public AddTagToTaskCommand(IUnitOfWork uow, IMapper mapper, IAsyncQueryableFactory factory)
        {
            _uow = uow;
            _mapper = mapper;
            _factory = factory;
        }

        public async Task<TaskResponse> ExecuteAsync(int taskId, string tagName)
        {
            var task = await _factory
                .CreateAsyncQueryable(_uow.Tasks.Query(p => p.Project.User, t => t.Tags))
                .FirstOrDefaultAsync(tk => tk.Id == taskId);

            task.Tags = _uow.TagsInTasks.Query(t => t.Tag).Where(t => t.TaskId == task.Id).ToList();

            if (task == null)
                return null;

            var tag = await _factory
                .CreateAsyncQueryable(_uow.Tags.Query())
                .FirstOrDefaultAsync(t => t.Name == tagName);

            if (tag != null)
            {
                var tt = _uow.TagsInTasks.Query().Where(t => t.TaskId == task.Id && t.TagId == tag.Id);
                if (!tt.Any())
                    task.Tags.Add(new TagsInTask {Tag = tag, TagId = tag.Id, Task = task, TaskId = task.Id});
            }
            else
            {
                tag = new Tag {Name = tagName};
                task.Tags.Add(new TagsInTask {Tag = tag, TagId = tag.Id, Task = task, TaskId = task.Id});
            }

            await _uow.CommitAsync();
           
            return _mapper.Map<Entities.Task, TaskResponse>(task);
        }
    }
}

 

