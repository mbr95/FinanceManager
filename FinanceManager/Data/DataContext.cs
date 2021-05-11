using System;
using System.Linq;
using FinanceManager.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace FinanceManager.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options)
            : base(options) { }

        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<TransactionCategory> TransactionCategories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder
                .Entity<Transaction>()
                .Property(e => e.Id)
                .UseIdentityColumn();
            modelBuilder
                .Entity<Transaction>()
                .Property(e => e.Description)
                .IsRequired()
                .HasColumnType("Nvarchar(25)");
            modelBuilder
                .Entity<Transaction>()
                .Property(e => e.Amount)
                .IsRequired()
                .HasColumnType("Decimal(14,2)");
            modelBuilder
                .Entity<Transaction>()
                .Property(e => e.Date)
                .IsRequired()
                .HasColumnType("Date");

            
            modelBuilder
                .Entity<TransactionCategory>()
                .Property(e => e.Name)
                .IsRequired()
                .HasColumnType("Nvarchar(15)");
                

            // Value conversions and data seeding for TransactionCategories
            modelBuilder
                .Entity<Transaction>()
                .Property(e => e.CategoryId)
                .HasConversion<int>();

            modelBuilder
                .Entity<TransactionCategory>()
                .Property(e => e.Id)
                .HasConversion<int>();

            modelBuilder
                .Entity<TransactionCategory>()
                .HasData(
                    Enum.GetValues(typeof(TransactionCategoryId))
                        .Cast<TransactionCategoryId>()
                        .Select(e => new TransactionCategory()
                        {
                            Id = e,
                            Name = e.ToString()
                        })
                );

        }
    }
}
