using Microsoft.EntityFrameworkCore;
using TasksManager.Entities;
using Task = System.Threading.Tasks.Task;
using TaskManager.DataAccess.UnitOfWork;
using TasksManager.DataAccess.Interfaces.Projects;

namespace TasksManager.DataAccess.DbImplementation.Projects
{
    internal class DeleteProjectCommand : IDeleteProjectCommand
    {
        private readonly IUnitOfWork _uow;
        private readonly IAsyncQueryableFactory _factory;

        public DeleteProjectCommand(IUnitOfWork uow, IAsyncQueryableFactory factory)
        {
            _uow = uow;
            _factory = factory;
        }

        public async Task ExecuteAsync(int projectId, string user)
        {
            Project project = await _factory
                .CreateAsyncQueryable(_uow.Projects.Query(p => p.Tasks, p => p.Lock))
                .FirstOrDefaultAsync(p => p.Id == projectId);
                   
                
            if (project != null)
            {
                if (project.Tasks.Count != 0)
                    throw new CannotDeleteProjectWithTasksException();

                if (project.Lock != null && project.Lock.Username != user)
                        throw new ProjectLockedException();

                _uow.Projects.Remove(project);
                await _uow.CommitAsync();
            }
        }
    }
}
