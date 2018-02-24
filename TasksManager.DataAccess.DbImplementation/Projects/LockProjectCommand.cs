using System;
using System.Threading.Tasks;
using TaskManager.DataAccess.UnitOfWork;
using TasksManager.DataAccess.Interfaces.Projects;
using TasksManager.Entities;

namespace TasksManager.DataAccess.DbImplementation.Projects
{
    public class LockProjectCommand : ILockProjectCommand
    {
        private readonly IUnitOfWork _uow;
        private readonly IAsyncQueryableFactory _factory;

        public LockProjectCommand(IUnitOfWork uow, IAsyncQueryableFactory factory)
        {
            _uow = uow;
            _factory = factory;
        }

        public async Task<bool> ExecuteAsync(int projectId, string user)
        {
            var project = await _factory
                .CreateAsyncQueryable(_uow.Projects.Query(p => p.Lock))
                .FirstOrDefaultAsync(p => p.Id == projectId);

            if (project == null)
                return false;

            if (project.Lock != null)
                throw new ProjectLockedException();

            var projectLock = new ProjectLock {
                ProjectId = projectId,
                Username = user,
                LockDate = DateTime.Now};
            _uow.ProjectLock.Add(projectLock);

            await _uow.CommitAsync();

            return true;
        }
    }
}
