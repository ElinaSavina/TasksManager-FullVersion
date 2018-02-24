using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace TaskManager.DataAccess.UnitOfWork.Implementation
{
    internal class AsyncQueryable<T> : IAsyncQueryable<T>
    {
        private readonly IQueryable<T> _query;
        public AsyncQueryable(IQueryable<T> query)
        {
            _query = query;
        }
        public Task<T> FirstOrDefaultAsync()
        {
            return _query.FirstOrDefaultAsync();
        }

        public Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate)
        {
            return _query.FirstOrDefaultAsync(predicate);
        }

        public Task<T> FirstAsync()
        {
            return _query.FirstAsync();
        }

        public Task<T> FirstAsync(Expression<Func<T, bool>> predicate)
        {
            return _query.FirstAsync(predicate);
        }

        public Task<List<T>> ToListAsync()
        {
            return _query.ToListAsync();
        }

        public Task<T[]> ToArrayAsync()
        {
            return _query.ToArrayAsync();
        }

        public Task<bool> AnyAsync()
        {
            return _query.AnyAsync();
        }

        public Task<bool> AnyAsync(Expression<Func<T, bool>> predicate)
        {
            return _query.AnyAsync(predicate);
        }

        public Task<bool> AllAsync(Expression<Func<T, bool>> predicate)
        {
            return _query.AllAsync(predicate);
        }

        public Task<int> CountAsync()
        {
            return _query.CountAsync();
        }

        public Task<int> CountAsync(Expression<Func<T, bool>> predicate)
        {
            return _query.CountAsync(predicate);
        }

        public Task<long> LongCountAsync()
        {
            return _query.LongCountAsync();
        }

        public Task<long> LongCountAsync(Expression<Func<T, bool>> predicate)
        {
            return _query.LongCountAsync(predicate);
        }

        public Task<T> LastAsync()
        {
            return _query.LastAsync();
        }

        public Task<T> LastAsync(Expression<Func<T, bool>> predicate)
        {
            return _query.LastAsync(predicate);
        }

        public Task<T> LastOrDefaultAsync()
        {
            return _query.LastOrDefaultAsync();
        }

        public Task<T> LastOrDefaultAsync(Expression<Func<T, bool>> predicate)
        {
            return _query.LastOrDefaultAsync(predicate);
        }

        public Task<T> SingleAsync()
        {
            return _query.SingleAsync();
        }

        public Task<T> SingleAsync(Expression<Func<T, bool>> predicate)
        {
            return _query.SingleAsync(predicate);
        }

        public Task<T> SingleOrDefaultAsync()
        {
            return _query.SingleOrDefaultAsync();
        }

        public Task<T> SingleOrDefaultAsync(Expression<Func<T, bool>> predicate)
        {
            return _query.SingleOrDefaultAsync(predicate);
        }

        public Task<T> MinAsync()
        {
            return _query.MinAsync();
        }

        public Task<TResult> MinAsync<TResult>(Expression<Func<T, TResult>> selector)
        {
            return _query.MinAsync(selector);
        }

        public Task<T> MaxAsync()
        {
            return _query.MaxAsync();
        }

        public Task<TResult> MaxAsync<TResult>(Expression<Func<T, TResult>> selector)
        {
            return _query.MinAsync(selector);
        }

        public Task<decimal> SumAsync(Expression<Func<T, decimal>> selector)
        {
            return _query.SumAsync(selector);
        }

        public Task<decimal?> SumAsync(Expression<Func<T, decimal?>> selector)
        {
            return _query.SumAsync(selector);
        }
        

        public Task<decimal> AverageAsync(Expression<Func<T, decimal>> selector)
        {
            return _query.AverageAsync(selector);
        }

        public Task<decimal?> AverageAsync(Expression<Func<T, decimal?>> selector)
        {
            return _query.AverageAsync(selector);
        }

        public Task<bool> ContainsAsync(T item)
        {
            return _query.ContainsAsync(item);
        }

        public Task LoadAsync()
        {
            return _query.LoadAsync();
        }

        public Task<Dictionary<TKey, T>> ToDictionaryAsync<TKey>(Func<T, TKey> keySelector)
        {
            return _query.ToDictionaryAsync(keySelector);
        }

        public Task<Dictionary<TKey, T>> ToDictionaryAsync<TKey>(Func<T, TKey> keySelector, IEqualityComparer<TKey> comparer)
        {
            return _query.ToDictionaryAsync(keySelector, comparer);
        }

        public Task<Dictionary<TKey, TElement>> ToDictionaryAsync<TKey, TElement>(Func<T, TKey> keySelector, Func<T, TElement> elementSelector)
        {
            return _query.ToDictionaryAsync(keySelector, elementSelector);
        }

        public Task<Dictionary<TKey, TElement>> ToDictionaryAsync<TKey, TElement>(Func<T, TKey> keySelector, Func<T, TElement> elementSelector, IEqualityComparer<TKey> comparer)
        {
            return _query.ToDictionaryAsync(keySelector, elementSelector, comparer);
        }

        public Task ForEachAsync(Action<T> action)
        {
            return _query.ForEachAsync(action);
        }
    }
}
