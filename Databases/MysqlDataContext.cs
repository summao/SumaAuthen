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
            modelBuilder.Entity<RefreshToken>()
                .HasIndex(a => a.Token)
                .IsUnique();

            modelBuilder.Entity<Account>()
                .HasIndex(a => a.MobileNumber)
                .IsUnique();

            modelBuilder.Entity<Account>()
                .HasIndex(a => a.Username)
                .IsUnique();
                
            base.OnModelCreating(modelBuilder);
        }
    }
}