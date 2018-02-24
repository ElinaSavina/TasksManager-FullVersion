using TaskManager.DataAccess.UnitOfWork;
using TasksManager.DataAccess.Interfaces.Projects;
using TasksManager.Entities;
using System.Threading.Tasks;

namespace TasksManager.DataAccess.DbImplementation.Projects
{
    internal class GetProjectQuery : IGetProjectQuery
    {
        private readonly IUnitOfWork _uow;
        private readonly IAsyncQueryableFactory _factory;

        public GetProjectQuery(IUnitOfWork uow, IAsyncQueryableFactory factory)
        {
            _uow = uow;
            _factory = factory;
        }
        public async Task<Project> RunAsync(int projectId)
        {
            return await _factory
                .CreateAsyncQueryable(_uow.Projects.Query(u => u.User))
                .FirstOrDefaultAsync(p => p.Id == projectId);            
        }
    }
}
