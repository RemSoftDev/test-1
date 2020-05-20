using Microsoft.EntityFrameworkCore;
using Test2.Data.Models;

namespace Test2.Data
{
    public class DataContext : DbContext
    {
        public DbSet<Transaction> Transactions { get; set; }

        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Transaction>().HasKey(t => t.Id);
            modelBuilder.Entity<Transaction>().Property(t => t.Id).HasMaxLength(50);
            modelBuilder.Entity<Transaction>().Property(t => t.Currency).HasMaxLength(3).IsRequired();
        }
    }
}
