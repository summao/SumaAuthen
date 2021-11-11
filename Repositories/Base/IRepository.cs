using System;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using MongoDB.Driver.Linq;

namespace Suma.Authen.Repositories.Base
{
    public interface IRepository<TEntity> where TEntity : class
    {
        IMongoQueryable<TEntity> Collection { get; }
        Task<TEntity> GetOneAsync(Expression<Func<TEntity, bool>> filter, CancellationToken cancellationToken = default(CancellationToken));
        Task<TEntity> GetOneAndDeleteAsync(Expression<Func<TEntity, bool>> filter, CancellationToken cancellationToken = default(CancellationToken));
        Task<TEntity> InsertAsync(TEntity entity, CancellationToken cancellationToken = default(CancellationToken));
        void Update(TEntity entityToUpdate);
        void DeleteAsync(TEntity entityToDelete);
        void DeleteAsync(object id);
    }
}