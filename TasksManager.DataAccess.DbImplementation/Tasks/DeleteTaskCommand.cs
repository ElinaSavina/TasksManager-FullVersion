using System.Threading.Tasks;
using TaskManager.DataAccess.UnitOfWork;
using TasksManager.DataAccess.Interfaces.Tasks;

namespace TasksManager.DataAccess.DbImplementation.Tasks
{
    public class DeleteTaskCommand : IDeleteTaskCommand
    {
        private readonly IUnitOfWork _uow;
        private readonly IAsyncQueryableFactory _factory;

        public DeleteTaskCommand(IUnitOfWork uow, IAsyncQueryableFactory factory)
        {
            _uow = uow;
            _factory = factory;
        }

        public async Task ExecuteAsync(int taskId)
        {
            Entities.Task task = await _factory
                .CreateAsyncQueryable(_uow.Tasks.Query())
                .FirstOrDefaultAsync(p => p.Id == taskId);
            if (task != null)
            {
                _uow.Tasks.Remove(task);
                await _uow.CommitAsync();
            }
        }
    }
}
