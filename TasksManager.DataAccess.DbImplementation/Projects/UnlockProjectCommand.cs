using System.Threading.Tasks;
using TaskManager.DataAccess.UnitOfWork;
using TasksManager.DataAccess.Interfaces.Projects;

namespace TasksManager.DataAccess.DbImplementation.Projects
{
    public class UnlockProjectCommand : IUnlockProjectCommand
    {
        private readonly IUnitOfWork _uow;
        private readonly IAsyncQueryableFactory _factory;

        public UnlockProjectCommand(IUnitOfWork uow, IAsyncQueryableFactory factory)
        {
            _uow = uow;
            _factory = factory;
        }

        public async Task<bool> ExecuteAsync(int projectId, string user)
        {
           var projectLock = await _factory
                .CreateAsyncQueryable(_uow.ProjectLock.Query())
                .FirstOrDefaultAsync(p => p.ProjectId == projectId && p.Username == user);

            if (projectLock == null)
            {
                return true;
            }

            _uow.ProjectLock.Remove(projectLock);

            await _uow.CommitAsync();

            return true;
        }
    }
}
