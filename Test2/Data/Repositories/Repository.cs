using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Test2.Data.Models;

namespace Test2.Data.Repositories
{
    public class Repository<TEntity, TKey> : IRepository<TEntity, TKey> where TEntity : class, IEntity<TKey>
    {
        private readonly DbSet<TEntity> _entities;

        public IQueryable<TEntity> Query => _entities.AsQueryable();

        public Repository(DataContext context)
        {
            _entities = context.Set<TEntity>();
        }

        public TEntity Add(TEntity entity)
        {
            _entities.Add(entity);
            return entity;
        }

        public TEntity Update(TEntity entity)
        {
            _entities.Attach(entity);
            _entities.Update(entity);
            return entity;
        }

        public async Task<IEnumerable<TKey>> CheckExists(IEnumerable<TKey> keys)
        {
            return await _entities.Where(e => keys.Contains(e.Id)).Select(e => e.Id).ToListAsync();
        }
    }
}
