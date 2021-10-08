using MongoDB.Driver;
using Suma.Authen.Entities;
using Suma.Authen.Repositories.Base;

namespace Suma.Authen.Repositories
{
    public interface IRefreshTokenRepository : IRepository<RefreshToken>
    {

    }

    public class RefreshTokenRepository : MongoDbBaseRepository<RefreshToken>, IRefreshTokenRepository
    {
        public RefreshTokenRepository(IMongoDatabase database) : base(database) 
        {

        }
    }
}