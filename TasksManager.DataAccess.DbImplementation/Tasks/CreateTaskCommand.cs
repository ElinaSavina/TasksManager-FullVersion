using System;
using System.Threading.Tasks;
using AutoMapper;
using TaskManager.DataAccess.UnitOfWork;
using TasksManager.DataAccess.Interfaces.Tasks;
using TasksManager.ViewModel.Tasks;

namespace TasksManager.DataAccess.DbImplementation.Tasks
{
    public class CreateTaskCommand : ICreateTaskCommand
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;
        private readonly IAsyncQueryableFactory _factory;

        public CreateTaskCommand(IUnitOfWork uow, IMapper mapper, IAsyncQueryableFactory factory)
        {
            _uow = uow;
            _mapper = mapper;
            _factory = factory;
        }

        public async Task<TaskResponse> ExecuteAsync(CreateTaskRequest request)
        {
            var project = await _factory
                .CreateAsyncQueryable(_uow.Projects.Query(p => p.User))
                .FirstOrDefaultAsync(p => p.Id == request.ProjectId);
            if (project == null) 
                throw new ArgumentException("ProjectId incorrect", "projectId");

            Entities.Task task = _mapper.Map<CreateTaskRequest, Entities.Task>(request);
            foreach (var tagsInTask in task.Tags)
            {
                //проверка на существование тега
                var tag = await _factory
                    .CreateAsyncQueryable(_uow.Tags.Query())
                    .FirstOrDefaultAsync(t => t.Name == tagsInTask.Tag.Name);
                if (tag != null) tagsInTask.Tag = tag;
            }

            _uow.Tasks.Add(task);
            await _uow.CommitAsync();

            task = await _factory
                .CreateAsyncQueryable(_uow.Tasks.Query(t => t.Tags))
                .FirstOrDefaultAsync(tk => tk.Id == task.Id);

            task.Project = await _factory
                .CreateAsyncQueryable(_uow.Projects.Query(t => t.Tasks, t => t.User))
                .FirstOrDefaultAsync(p => p.Id == task.ProjectId);
                  
            return _mapper.Map<Entities.Task, TaskResponse>(task);
        }
    }
}
