using Microsoft.EntityFrameworkCore;
using Suma.Authen.Entities;

namespace Suma.Authen.Databases
{
    public class MysqlDataContext : DbContext
    {
        public MysqlDataContext(DbContextOptions<MysqlDataContext> options) : base(options) { }

        public DbSet<Account> Accounts { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Account>()
                .HasIndex(a => a.Email)
                .IsUnique();

            modelBuilder.Entity<RefreshToken>()
                .HasIndex(a => a.Token)
                .IsUnique();
                
            base.OnModelCreating(modelBuilder);
        }
    }
}