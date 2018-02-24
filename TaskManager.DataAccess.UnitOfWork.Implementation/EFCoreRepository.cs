using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Query;
using TasksManager.Entities;
using Task = System.Threading.Tasks.Task;

namespace TaskManager.DataAccess.UnitOfWork.Implementation
{
    public class EfCoreRepository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        protected DbSet<TEntity> DbSet { get; }
        public EfCoreRepository(DbSet<TEntity> dbset)
        {
            DbSet = dbset ?? throw new ArgumentNullException(nameof(dbset)); 
        }

        public IQueryable<TEntity> Query(params Expression<Func<TEntity, object>>[] includes)
        {
            return ApplyIncludes(includes, DbSet);
        }
        private static IQueryable<TEntity> ApplyIncludes(Expression<Func<TEntity, object>>[] includes, IQueryable<TEntity> query)
        {
            foreach (var include in includes)
            {
                query = query.Include(include);
            }
            return query;
        }

        public IQueryable<TEntity> NoTrackingQuery(params Expression<Func<TEntity, object>>[] includes)
        {
            return ApplyIncludes(includes, DbSet.AsNoTracking());
        }

        public void Add(TEntity entity)
        {
            DbSet.Add(entity);
        }

        public Task<EntityEntry<TEntity>> AddAsync(TEntity entity)
        {
            return DbSet.AddAsync(entity);
        }

        public void Remove(TEntity entity)
        {
            DbSet.Remove(entity);
        } 
    }
}
