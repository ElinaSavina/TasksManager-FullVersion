using System.Threading.Tasks;
using AutoMapper;
using TaskManager.DataAccess.UnitOfWork;
using TasksManager.DataAccess.Interfaces.Projects;
using TasksManager.Entities;
using TasksManager.ViewModel.Projects;
namespace TasksManager.DataAccess.DbImplementation.Projects
{
    internal class UpdateProjectCommand : IUpdateProjectCommand
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;
        private readonly IAsyncQueryableFactory _factory;

        public UpdateProjectCommand(IUnitOfWork uow, IMapper mapper, IAsyncQueryableFactory factory)
        {
            _uow = uow; 
            _mapper = mapper;
            _factory = factory;
        }
       
        public async Task<ProjectResponse> ExecuteAsync(int projectId, UpdateProjectRequest request, string user)
        {
            Project project = await _factory
                .CreateAsyncQueryable(_uow.Projects.Query(p => p.Lock, p => p.User))
                .FirstOrDefaultAsync(p => p.Id == projectId);

            if (project == null)
                return null;

            if (project.Lock != null && project.Lock.Username != user)
                throw new ProjectLockedException();
           
            project = _mapper.Map(request, project);

            await _uow.CommitAsync();

            return _mapper.Map<Project, ProjectResponse>(project);
        }
    }
}
