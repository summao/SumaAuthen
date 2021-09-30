using System.Threading.Tasks;
using Suma.Authen.Databases;
using Suma.Authen.Entities;

namespace Suma.Authen.Repositories
{
    public interface IUnitOfWork
    {
        IRepository<Account> Accounts { get; }
        IRepository<RefreshToken> RefreshTokens { get; }

        Task CommitAsync();
    }

    public class UnitOfWork : IUnitOfWork
    {
        private readonly MysqlDataContext _context;

        private MySqlBaseRepository<Account> _accounts;
        private MySqlBaseRepository<RefreshToken> _refreshTokens;

        public UnitOfWork(MysqlDataContext context)
        {
            _context = context;
        }

        public IRepository<Account> Accounts => _accounts ?? (_accounts = new MySqlBaseRepository<Account>(_context));
        public IRepository<RefreshToken> RefreshTokens => _refreshTokens ?? (_refreshTokens = new MySqlBaseRepository<RefreshToken>(_context));

        public async Task CommitAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}