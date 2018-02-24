using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TasksManager.Db;
using TasksManager.Entities;
using Task = TasksManager.Entities.Task;

namespace TaskManager.DataAccess.UnitOfWork.Implementation
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly TasksContext _context;
        private IRepository<Task> _tasks;
        private IRepository<Tag> _tags;
        private IRepository<TagsInTask> _tagsInTasks;
        private IRepository<Project> _project;
        private IRepository<Group> _group;
        private IRepository<GeoObject> _geoObjects;
        private IRepository<ProjectLock> _projectLock;
        private IRepository<User> _users;
        private IRepository<Role> _roles;

        public UnitOfWork(TasksContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context)); ;
        }
        public IRepository<Role> Roles => _roles ??
                                          (_roles = new EfCoreRepository<Role>(_context.Roles));       

        public IRepository<User> Users => _users ??
                                         (_users = new EfCoreRepository<User>(_context.Users));

        public IRepository<ProjectLock> ProjectLock => _projectLock ??
                                                    (_projectLock = new EfCoreRepository<ProjectLock>(_context.ProjectLock));

        public IRepository<GeoObject> GeoObjects => _geoObjects ??
                                                      (_geoObjects = new EfCoreRepository<GeoObject>(_context.GeoObjects));

        public IRepository<TagsInTask> TagsInTasks => _tagsInTasks ??
            (_tagsInTasks = new EfCoreRepository<TagsInTask>(_context.TagsInTasks));

        public IRepository<Task> Tasks => _tasks ??
            (_tasks = new EfCoreRepository<Task>(_context.Tasks));

        public IRepository<Tag> Tags => _tags ??
            (_tags = new EfCoreRepository<Tag>(_context.Tags));
        public IRepository<Project> Projects => _project ??
            (_project = new EfCoreRepository<Project>(_context.Projects));
        public IRepository<Group> Groups => _group ??
            (_group = new EfCoreRepository<Group>(_context.Groups));
        #region Sync

        public int Commit()
        {
            return _context.SaveChanges();
        }
        #endregion
        public void Migrate()
        {
            _context.Database.Migrate();
        }
        #region Async
        public Task<int> CommitAsync()
        {
            return _context.SaveChangesAsync();
        }
        #endregion

        #region Disposable implementation

        private bool disposed = false;

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
                return;

            if (disposing)
            {
                _context.Dispose();
            }

            disposed = true;
        }
        #endregion
    }
}
