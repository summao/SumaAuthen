using System.Threading.Tasks;
using Suma.Authen.Databases;
using Suma.Authen.Entities;

namespace Suma.Authen.Repositories
{
    public interface ITokenRepository
    {
        
    }

    public class TokenRepository : ITokenRepository
    {
        private readonly MysqlDataContext _context;

        public TokenRepository(MysqlDataContext context)
        {
            _context = context;
        }

        public async Task InsertAsync(RefreshToken token)
        {
            await _context.RefreshTokens.AddAsync(token);
        } 
    }
}