using MongoDB.Driver;
using Suma.Authen.Entities;
using Suma.Authen.Repositories.Base;

namespace Suma.Authen.Repositories
{
    public interface IAccountRepository : IRepository<Account>
    {

    }

    public class AccountRepository : MongoDbBaseRepository<Account>, IAccountRepository
    {
        public AccountRepository(IMongoDatabase database) : base(database) 
        { 
            
        }
    }
}