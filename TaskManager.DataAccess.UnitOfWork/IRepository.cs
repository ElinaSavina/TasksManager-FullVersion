using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using TasksManager.Entities;
using Task = System.Threading.Tasks.Task;

namespace TaskManager.DataAccess.UnitOfWork
{
    public interface IRepository<TEntity> where TEntity : class
    {
        void Add(TEntity entity);

        void Remove(TEntity entity);

        Task<EntityEntry<TEntity>> AddAsync(TEntity entity);

        IQueryable<TEntity> Query(params Expression<Func<TEntity, object>>[] includes);
    }
}
