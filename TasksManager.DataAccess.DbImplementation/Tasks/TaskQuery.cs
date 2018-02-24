using System.Threading.Tasks;
using AutoMapper;
using TaskManager.DataAccess.UnitOfWork;
using TasksManager.DataAccess.Interfaces.Tasks;
using TasksManager.ViewModel.Tasks;
using System.Linq;

namespace TasksManager.DataAccess.DbImplementation.Tasks
{
    public class TaskQuery : ITaskQuery
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;
        private readonly IAsyncQueryableFactory _factory;

        public TaskQuery(IUnitOfWork uow, IMapper mapper, IAsyncQueryableFactory factory)
        {
            _uow = uow;
            _mapper = mapper;
            _factory = factory;
        }

        public async Task<TaskResponse> RunAsync(int taskId)
       {    
            Entities.Task task = await _factory
                .CreateAsyncQueryable(_uow.Tasks.Query(t => t.Project.Tasks, t => t.Project.User))
                .FirstOrDefaultAsync(tk => tk.Id == taskId);
            if (task == null)
                return null;
            task.Tags = _uow.TagsInTasks.Query(t => t.Tag).Where(t => t.TaskId == task.Id).ToList();
           
            return _mapper.Map<Entities.Task, TaskResponse>(task); 
        }
    }
}

