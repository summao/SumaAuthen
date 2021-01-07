using Microsoft.EntityFrameworkCore;
using SumaAuthen.Entities;

namespace SumaAuthen.Databases
{
    public class MysqlDataContext : DbContext
    {
        public MysqlDataContext(DbContextOptions<MysqlDataContext> options) : base(options) { }

        public DbSet<Account> Accounts { get; set; }
    }
}