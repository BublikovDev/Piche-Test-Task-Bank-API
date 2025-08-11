using Microsoft.EntityFrameworkCore;
using Server.Entities;

namespace Server.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Account> Accounts { get; set; } = null!;
        public DbSet<Transaction> Transactions { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Account>(b =>
            {
                b.HasKey(a => a.AccountNumber);
                b.Property(a => a.Owner).IsRequired().HasMaxLength(200);
                b.Property(a => a.Balance).IsRequired().HasPrecision(18, 2);
            });

            modelBuilder.Entity<Transaction>(b =>
            {
                b.HasKey(t => t.Id);
                b.Property(t => t.Amount).IsRequired().HasPrecision(18, 2);
                b.Property(t => t.Type).IsRequired().HasMaxLength(50);
                b.HasOne(t => t.Account)
                 .WithMany(a => a.Transactions)
                 .HasForeignKey(t => t.AccountNumber)
                 .OnDelete(DeleteBehavior.Cascade);
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}
