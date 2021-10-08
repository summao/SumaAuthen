using System.Threading.Tasks;
using MongoDB.Driver;

namespace Suma.Authen.Repositories.Base
{
    public class MongoDbBaseRepository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        private readonly IMongoCollection<TEntity> _collection;

        public MongoDbBaseRepository(IMongoDatabase database)
        {
            _collection = database.GetCollection<TEntity>(typeof(TEntity).Name);
        }

        public async Task<System.Collections.Generic.IEnumerable<TEntity>> GetAsync(System.Linq.Expressions.Expression<System.Func<TEntity, bool>> filter = null, System.Func<System.Linq.IQueryable<TEntity>, System.Linq.IOrderedQueryable<TEntity>> orderBy = null, string includeProperties = "")
        {
            return await _collection.Find(a => true).ToListAsync();
        }

        public TEntity GetByIdAsync(object id)
        {
            throw new System.NotImplementedException();
        }

        public async Task<TEntity> GetOneAsync(System.Linq.Expressions.Expression<System.Func<TEntity, bool>> filter)
        {
            return await _collection.Find(filter).SingleOrDefaultAsync();
        }

        public System.Collections.Generic.IEnumerable<TEntity> GetWithRawSqlAsync(string query, params object[] parameters)
        {
            throw new System.NotImplementedException();
        }

        public async Task<TEntity> InsertAsync(TEntity entity)
        {
            await _collection.InsertOneAsync(entity);
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