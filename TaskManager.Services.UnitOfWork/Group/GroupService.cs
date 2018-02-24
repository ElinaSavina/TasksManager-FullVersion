using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using TaskManager.DataAccess.UnitOfWork;
using TaskManager.Services.Groups;
using TasksManager.ViewModel.Group;
using Task = System.Threading.Tasks.Task;
namespace TaskManager.Services.UnitOfWork.Group
{
    public class GroupService : IGroupService
    {
        private IUnitOfWork Uow { get; }
        private IAsyncQueryableFactory Factory { get; }
        private IMapper Mapper { get; }

        public GroupService(IUnitOfWork uow, IAsyncQueryableFactory factory, IMapper mapper)
        {
            Uow = uow;
            Factory = factory;
            Mapper = mapper;
        }

        public async Task<GroupResponse> AddGroup(CreateGroupRequest request)
        {
            var group = Mapper.Map<CreateGroupRequest, TasksManager.Entities.Group>(request);
            await Uow.Groups.AddAsync(group);
            await Uow.CommitAsync();
            return Mapper.Map<TasksManager.Entities.Group, GroupResponse>(group);
        }

        public async Task<GroupResponse> GetGroup(int id)
        {
            var group = await Factory
                .CreateAsyncQueryable(Uow.Groups.Query(s => s.Students))
                .FirstOrDefaultAsync(g => g.Id == id);
            return Mapper.Map<TasksManager.Entities.Group, GroupResponse>(group);
        }

        public async Task<GroupResponse> UpdateGroup(int id, UpdateGroupRequest request)
        {
            var originalGroup = await Factory
                .CreateAsyncQueryable(Uow.Groups.Query(s=> s.Students))
                .FirstOrDefaultAsync(g => g.Id == id);
            var group = Mapper.Map<UpdateGroupRequest, TasksManager.Entities.Group>(request);
            if (originalGroup.Name != group.Name)
                originalGroup.Name = group.Name;

            var addedStudents = group.Students.Where(n => originalGroup.Students.All(t => t.Name != n.Name)).ToList();
            foreach (var st in addedStudents)
                originalGroup.Students.Add(st);

            var removedStudents = originalGroup.Students.Where(n => group.Students.All(t => t.Name != n.Name)).ToList();
            foreach (var rs in removedStudents)
                originalGroup.Students.Remove(rs);
            
            await Uow.CommitAsync();
            return Mapper.Map<TasksManager.Entities.Group, GroupResponse>(group);
        }

        public async Task RemoveGroup(int id)
        {
            var group = await Factory.CreateAsyncQueryable(Uow.Groups.Query()).FirstOrDefaultAsync(g => g.Id == id);
            Uow.Groups.Remove(group);
            await Uow.CommitAsync();
        }
    }
}
