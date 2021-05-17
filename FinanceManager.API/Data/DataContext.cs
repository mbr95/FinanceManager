using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using FinanceManager.API.Domain.Models;

namespace FinanceManager.API.Data
{
    public class DataContext : IdentityDbContext
    {
        public DataContext(DbContextOptions<DataContext> options)
            : base(options) { }

        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<TransactionCategory> TransactionCategories { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }

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

            modelBuilder
                .Entity<IdentityUser>()
                .HasMany<Transaction>()
                .WithOne(e => e.User)
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder
                .Entity<IdentityUser>()
                .HasMany<RefreshToken>()
                .WithOne(e => e.User)
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder
                .Entity<RefreshToken>()
                .HasKey(e => e.Token);
            modelBuilder
                .Entity<RefreshToken>()
                .Property(e => e.Token)
                .ValueGeneratedOnAdd();


            // Value conversions and data seeding
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
