using System.Threading.Tasks;
using SumaAuthen.Databases;
using SumaAuthen.Entities;

namespace SumaAuthen.Repositories
{
    public interface IUnitOfWork
    {
        IRepository<Account> Accounts { get; }

        Task CommitAsync();
    }

    public class UnitOfWork : IUnitOfWork
    {
        private readonly MysqlDataContext _context;

        private BaseRepository<Account> _accounts;

        public UnitOfWork(MysqlDataContext context)
        {
            _context = context;
        }

        public IRepository<Account> Accounts => _accounts ?? (_accounts = new BaseRepository<Account>(_context));

        public async Task CommitAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}