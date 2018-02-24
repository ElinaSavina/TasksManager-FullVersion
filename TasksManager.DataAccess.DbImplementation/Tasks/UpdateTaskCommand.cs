using System.Threading.Tasks;
using AutoMapper;
using TaskManager.DataAccess.UnitOfWork;
using TasksManager.DataAccess.Interfaces.Tasks;
using TasksManager.ViewModel.Tasks;
using System.Linq;

namespace TasksManager.DataAccess.DbImplementation.Tasks
{
    public class UpdateTaskCommand : IUpdateTaskCommand
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;
        private readonly IAsyncQueryableFactory _factory;

        public UpdateTaskCommand(IUnitOfWork uow, IMapper mapper, IAsyncQueryableFactory factory)
        {
            _uow = uow;
            _mapper = mapper;
            _factory = factory;
        }

        public async Task<TaskResponse> ExecuteAsync(int taskId, UpdateTaskRequest request)
        {
            Entities.Task task = await _factory
                .CreateAsyncQueryable(_uow.Tasks.Query())
                .FirstOrDefaultAsync(tk => tk.Id == taskId);
            if (task == null)
                return null;
            task.Tags = _uow.TagsInTasks.Query(t => t.Tag).Where(t => t.TaskId == task.Id).ToList();

            task = _mapper.Map(request, task);

            foreach (var tagsInTask in task.Tags)
            {
                var tag = await _factory
                    .CreateAsyncQueryable(_uow.Tags.Query())
                    .FirstOrDefaultAsync(t => t.Name == tagsInTask.Tag.Name);
                if (tag != null) tagsInTask.Tag = tag;
            }

            await _uow.CommitAsync();

            task.Project = await _factory.CreateAsyncQueryable(_uow.Projects.Query(p => p.Tasks, p => p.User))
                .FirstOrDefaultAsync(p => p.Id == task.ProjectId);

            return _mapper.Map<Entities.Task, TaskResponse>(task);
        }
    }
}
