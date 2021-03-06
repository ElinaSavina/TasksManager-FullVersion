﻿using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace TaskManager.DataAccess.UnitOfWork
{
    public interface IAsyncQueryable<TSource>
    {
        Task<TSource> FirstOrDefaultAsync();
        Task<TSource> FirstOrDefaultAsync(Expression<Func<TSource, bool>> predicate);
        Task<TSource> FirstAsync();
        Task<TSource> FirstAsync(Expression<Func<TSource, bool>> predicate);
       

        Task<List<TSource>> ToListAsync();
        Task<TSource[]> ToArrayAsync();

        Task<bool> AnyAsync();
        Task<bool> AnyAsync(Expression<Func<TSource, bool>> predicate);

        Task<bool> AllAsync(Expression<Func<TSource, bool>> predicate);

        Task<int> CountAsync();
        Task<int> CountAsync(Expression<Func<TSource, bool>> predicate);
        Task<long> LongCountAsync();
        Task<long> LongCountAsync(Expression<Func<TSource, bool>> predicate);

        Task<TSource> LastAsync();
        Task<TSource> LastAsync(Expression<Func<TSource, bool>> predicate);
        Task<TSource> LastOrDefaultAsync();
        Task<TSource> LastOrDefaultAsync(Expression<Func<TSource, bool>> predicate);

        Task<TSource> SingleAsync();
        Task<TSource> SingleAsync(Expression<Func<TSource, bool>> predicate);
        Task<TSource> SingleOrDefaultAsync();
        Task<TSource> SingleOrDefaultAsync(Expression<Func<TSource, bool>> predicate);

        Task<TSource> MinAsync();
        Task<TResult> MinAsync<TResult>(Expression<Func<TSource, TResult>> selector);

        Task<TSource> MaxAsync();
        Task<TResult> MaxAsync<TResult>(Expression<Func<TSource, TResult>> selector);

        Task<decimal> SumAsync(Expression<Func<TSource, decimal>> selector);
        Task<decimal?> SumAsync(Expression<Func<TSource, decimal?>> selector);
        
        Task<decimal> AverageAsync(Expression<Func<TSource, decimal>> selector);
        Task<decimal?> AverageAsync(Expression<Func<TSource, decimal?>> selector);

        Task<bool> ContainsAsync(TSource item);

        Task LoadAsync();

        Task<Dictionary<TKey, TSource>> ToDictionaryAsync<TKey>(Func<TSource, TKey> keySelector);
        Task<Dictionary<TKey, TSource>> ToDictionaryAsync<TKey>(Func<TSource, TKey> keySelector,
            IEqualityComparer<TKey> comparer);
        Task<Dictionary<TKey, TElement>> ToDictionaryAsync<TKey, TElement>(Func<TSource, TKey> keySelector,
            Func<TSource, TElement> elementSelector);
        Task<Dictionary<TKey, TElement>> ToDictionaryAsync<TKey, TElement>(Func<TSource, TKey> keySelector,
            Func<TSource, TElement> elementSelector, IEqualityComparer<TKey> comparer);

        Task ForEachAsync(Action<TSource> action);
    }
}
