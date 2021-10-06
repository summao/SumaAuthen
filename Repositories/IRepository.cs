using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Suma.Authen.Repositories
{
    public interface IRepository<TEntity> where TEntity : class
    {
        Task<IEnumerable<TEntity>> GetAsync(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = ""
        );
        Task<TEntity> GetOneAsync(Expression<Func<TEntity, bool>> filter);
        TEntity GetByIdAsync(object id);
        IEnumerable<TEntity> GetWithRawSqlAsync(string query, params object[] parameters);
        Task<TEntity> InsertAsync(TEntity entity);
        void Update(TEntity entityToUpdate);
        void DeleteAsync(TEntity entityToDelete);
        void DeleteAsync(object id);
    }
}