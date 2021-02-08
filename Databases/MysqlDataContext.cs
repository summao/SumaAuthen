using Microsoft.EntityFrameworkCore;
using Suma.Authen.Entities;

namespace Suma.Authen.Databases
{
    public class MysqlDataContext : DbContext
    {
        public MysqlDataContext(DbContextOptions<MysqlDataContext> options) : base(options) { }

        public DbSet<Account> Accounts { get; set; }
    }
}