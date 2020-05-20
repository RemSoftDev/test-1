using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Test2.Data.Models;

namespace Test2.Data.Repositories
{
    public interface IRepository<TEntity, TKey>
    {
        IQueryable<TEntity> Query { get; }
        TEntity Add(TEntity entity);
        TEntity Update(TEntity entity);
        Task<IEnumerable<TKey>> CheckExists(IEnumerable<TKey> keys);
    }
}
