using System.Threading.Tasks;
using TasksManager.Entities;
using Task = TasksManager.Entities.Task;

namespace TaskManager.DataAccess.UnitOfWork
{
    public interface IUnitOfWork 
    {
        IRepository<Project> Projects { get; }
        IRepository<Task> Tasks { get; }
        IRepository<Tag> Tags { get; }
        IRepository<TagsInTask> TagsInTasks { get; }
        IRepository<Group> Groups { get; }
        IRepository<GeoObject> GeoObjects { get; }
        IRepository<ProjectLock> ProjectLock { get; }
        IRepository<User> Users { get; }
        IRepository<Role> Roles { get; }

        void Migrate();

        int Commit();

        Task<int> CommitAsync();
    }
}
