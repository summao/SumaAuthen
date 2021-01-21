using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SumaAuthen.Databases;

namespace SumaAuthen.Repositories
{
    public interface IRepository<TEntity> where TEntity : class
    {
        void DeleteAsync(TEntity entityToDelete);
        void DeleteAsync(object id);
        IEnumerable<TEntity> GetAsync(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = ""
        );
        TEntity GetByIdAsync(object id);
        IEnumerable<TEntity> GetWithRawSqlAsync(string query, params object[] parameters);
        Task InsertAsync(TEntity entity);
        void UpdateAsync(TEntity entityToUpdate);
    }

    public class BaseRepository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        private readonly MysqlDataContext _context;
        private readonly DbSet<TEntity> _dbSet;
        public BaseRepository(MysqlDataContext context)
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

        public IEnumerable<TEntity> GetAsync(Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, string includeProperties = "")
        {
            IQueryable<TEntity> query = _dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (includeProperties != null)
            {
                foreach (var includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProperty);
                }
            }

            if (orderBy != null)
            {
                return orderBy(query).ToList();
            }
            else
            {
                return query.ToList();
            }
        }

        public TEntity GetByIdAsync(object id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<TEntity> GetWithRawSqlAsync(string query, params object[] parameters)
        {
            throw new NotImplementedException();
        }

        public virtual async Task InsertAsync(TEntity entity)
        {
            await _dbSet.AddAsync(entity);
        }

        public void UpdateAsync(TEntity entityToUpdate)
        {
            throw new NotImplementedException();
        }
    }
}