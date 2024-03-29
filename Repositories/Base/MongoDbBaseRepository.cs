using System.Threading.Tasks;
using MongoDB.Driver;
using System.Linq;
using MongoDB.Driver.Linq;
using System.Threading;
using System.Linq.Expressions;
using System;

namespace Suma.Authen.Repositories.Base
{
    public class MongoDbBaseRepository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        private readonly IMongoCollection<TEntity> _collection;

        public IMongoQueryable<TEntity> Collection
        {
            get { return _collection.AsQueryable(); }
        }

        public MongoDbBaseRepository(IMongoDatabase database)
        {
            _collection = database.GetCollection<TEntity>(typeof(TEntity).Name);
        }

        public async Task<TEntity> GetOneAsync(Expression<Func<TEntity, bool>> filter, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await _collection.Find(filter).FirstOrDefaultAsync();
        }

        public async Task<TEntity> GetOneAndDeleteAsync(Expression<Func<TEntity, bool>> filter, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await _collection.FindOneAndDeleteAsync(filter);
        }

        public async Task<TEntity> InsertAsync(TEntity entity, CancellationToken cancellationToken = default(CancellationToken))
        {
            await _collection.InsertOneAsync(entity, null, cancellationToken);
            return entity;
        }

        public void Update(TEntity entityToUpdate)
        {
            throw new System.NotImplementedException();
        }

        public void DeleteAsync(TEntity entityToDelete)
        {
            throw new System.NotImplementedException();
        }

        public void DeleteAsync(object id)
        {
            throw new System.NotImplementedException();
        }
    }
}