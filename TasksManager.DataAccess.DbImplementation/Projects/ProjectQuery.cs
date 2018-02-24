using System.Threading.Tasks;
using AutoMapper;
using TaskManager.DataAccess.UnitOfWork;
using TasksManager.DataAccess.Interfaces.Projects;
using TasksManager.Entities;
using TasksManager.ViewModel.Projects;

namespace TasksManager.DataAccess.DbImplementation.Projects
{
    internal class ProjectQuery : IProjectQuery
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _uow;
        private readonly IAsyncQueryableFactory _factory;

        public ProjectQuery(IUnitOfWork uow, IMapper mapper, IAsyncQueryableFactory factory)
        {
            _uow = uow;
            _mapper = mapper;
            _factory = factory;
        }

        public async Task<ProjectResponse> RunAsync(int projectId)
        {
            var project = await _factory.CreateAsyncQueryable(_uow.Projects.Query(t => t.Tasks, t => t.User))
                .FirstOrDefaultAsync(p => p.Id == projectId);

            return _mapper.Map<Project, ProjectResponse>(project);
        }
    }
}
