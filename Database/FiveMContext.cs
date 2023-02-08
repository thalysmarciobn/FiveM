using Microsoft.EntityFrameworkCore;
using Models.Database;
using System;

namespace Database
{
    public class FiveMContext : DbContext
    {
        public DbSet<AccountModel> Accounts { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var connectionString = @"server=localhost;database=fivem;uid=root;password=3251thaX@;";
            optionsBuilder.UseMySql(connectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<AccountModel>(e =>
            {
                e.ToTable("accounts");

                e.HasKey(m => m.Id);

                e.HasIndex(m => m.License)
                    .IsUnique();
                e.HasIndex(m => new { m.Id, m.License });

                e.Property(m => m.WhiteListed)
                    .IsRequired()
                    .HasMaxLength(1);

                e.Property(m => m.Created)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");
            });
        }
    }
}
