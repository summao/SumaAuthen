using Microsoft.EntityFrameworkCore;
using Suma.Authen.Entities;

namespace Suma.Authen.Databases
{
    public class MysqlDataContext : DbContext
    {
        public MysqlDataContext(DbContextOptions<MysqlDataContext> options) : base(options) { }

        public DbSet<Account> Accounts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Account>()
                .HasIndex(a => a.Email)
                .IsUnique();
                
            base.OnModelCreating(modelBuilder);
        }
    }
}