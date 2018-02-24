using System.Linq;
using System.Threading.Tasks;
using TaskManager.DataAccess.UnitOfWork;
using TasksManager.DataAccess.Interfaces.Tasks;

namespace TasksManager.DataAccess.DbImplementation.Tasks
{
    public class DeleteTagFromTaskCommand : IDeleteTagFromTaskCommand
    {
        private readonly IUnitOfWork _uow;
        private readonly IAsyncQueryableFactory _factory;

        public DeleteTagFromTaskCommand(IUnitOfWork uow, IAsyncQueryableFactory factory)
        {
            _uow = uow;
            _factory = factory;
        }

        public async Task ExecuteAsync(int taskId, string tag)
        {
            var task = await _factory
                .CreateAsyncQueryable(_uow.TagsInTasks.Query().Where(t => t.TaskId == taskId))
                .FirstOrDefaultAsync(t => t.Tag.Name == tag);
            if (task != null)
            {
                _uow.TagsInTasks.Remove(task);
                await _uow.CommitAsync();
            }
        }
    }
}
