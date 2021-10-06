using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Suma.Authen.Databases;

namespace Suma.Authen.Repositories
{
    public class MySqlBaseRepository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        private readonly MysqlDataContext _context;
        private readonly DbSet<TEntity> _dbSet;
        public MySqlBaseRepository(MysqlDataContext context)
        {
            _context = context;
            _dbSet = _context.Set<TEntity>();
        }

        public virtual void DeleteAsync(TEntity entityToDelete)
        {
            if (_context.Entry(entityToDelete).State == EntityState.Detached)
            {
                _dbSet.Attach(entityToDelete);
            }
            _dbSet.Remove(entityToDelete);
        }

        public void DeleteAsync(object id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<TEntity>> GetAsync(Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, string includeProperties = "")
        {
            // IQueryable<TEntity> query = _dbSet;

            // if (filter != null)
            // {
            //     query = query.Where(filter);
            // }

            // if (includeProperties != null)
            // {
            //     foreach (var includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            //     {
            //         query = query.Include(includeProperty);
            //     }
            // }

            // if (orderBy != null)
            // {
            //     return orderBy(query).ToList();
            // }
            // else
            // {
            //     return query.ToList();
            // }
            throw new NotImplementedException();
        }

        public virtual async Task<TEntity> GetOneAsync(Expression<Func<TEntity, bool>> filter)
        {
            return await _dbSet.Where(filter).SingleOrDefaultAsync();
        }

        public TEntity GetByIdAsync(object id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<TEntity> GetWithRawSqlAsync(string query, params object[] parameters)
        {
            throw new NotImplementedException();
        }

        public virtual async Task<TEntity> InsertAsync(TEntity entity)
        {
            await _dbSet.AddAsync(entity);
            return entity;
        }

        public virtual void Update(TEntity entityToUpdate)
        {
            _dbSet.Update(entityToUpdate);
        }
    }
}