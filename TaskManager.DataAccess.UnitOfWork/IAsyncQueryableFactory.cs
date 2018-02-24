using System.Linq;

namespace TaskManager.DataAccess.UnitOfWork
{
    public interface IAsyncQueryableFactory
    {
        IAsyncQueryable<T> CreateAsyncQueryable<T>(IQueryable<T> query);
    }
}
