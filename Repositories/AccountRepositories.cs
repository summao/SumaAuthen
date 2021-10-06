using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Suma.Authen.Databases;
using Suma.Authen.Entities;

namespace Suma.Authen.Repositories
{
    public interface IAccountRepositories
    {
        // Task<Account> GetByEmailAsync([EmailAddress] string email);
    }

    public class AccountRepositories : IAccountRepositories
    {
        // private readonly MysqlDataContext _context;

        // public AccountRepositories(MysqlDataContext context)
        // {
        //     _context = context;
        // }

        // public async Task<Account> GetByEmailAsync([EmailAddress] string email)
        // {
        //     return await _context.Accounts.SingleOrDefaultAsync<Account>(a => a.Email == email);
        // }
    }
}