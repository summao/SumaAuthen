using System.Threading;
using System.Threading.Tasks;
using MongoDB.Driver.Linq;

namespace Suma.Authen.Repositories.Base
{
    public interface IRepository<TEntity> where TEntity : class
    {
        IMongoQueryable<TEntity> Collection { get; }

        Task<TEntity> InsertAsync(TEntity entity, CancellationToken cancellationToken = default(CancellationToken));
        void Update(TEntity entityToUpdate);
        void DeleteAsync(TEntity entityToDelete);
        void DeleteAsync(object id);
    }
}