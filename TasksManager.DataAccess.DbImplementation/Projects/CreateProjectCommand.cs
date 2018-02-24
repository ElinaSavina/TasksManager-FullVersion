using System.Threading.Tasks;
using TasksManager.Entities;
using TasksManager.ViewModel.Projects;
using AutoMapper;
using TaskManager.DataAccess.UnitOfWork;
using TasksManager.DataAccess.Interfaces.Projects;

namespace TasksManager.DataAccess.DbImplementation.Projects
{
    internal class CreateProjectCommand : ICreateProjectCommand
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;
        private readonly IAsyncQueryableFactory _factory;

        public CreateProjectCommand(IUnitOfWork uow, IMapper mapper, IAsyncQueryableFactory factory)
        {
            _uow = uow;
            _mapper = mapper;
            _factory = factory;
        }

        public async Task<ProjectResponse> ExecuteAsync(CreateProjectRequest request, string username)
        {
            var project = _mapper.Map<CreateProjectRequest, Project>(request);
            var user = await _factory
                .CreateAsyncQueryable(_uow.Users.Query())
                .FirstOrDefaultAsync(u => u.Login == username);
            project.User = user;
            _uow.Projects.Add(project);
            await _uow.CommitAsync();
            return _mapper.Map<Project, ProjectResponse>(project);
        }
    }
}
