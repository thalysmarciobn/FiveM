using Microsoft.EntityFrameworkCore;
using Models.Database;
using System;

namespace Database
{
    public class FiveMContext : DbContext
    {
        public DbSet<AccountModel> Accounts { get; set; }
        public DbSet<AccountCharacterModel> AccountCharacters { get; set; }
        public DbSet<AccountCharacterPositionModel> AccountCharactersPosition { get; set; }
        public DbSet<AccountCharacterFaceShapeModel> AccountCharactersFaceShape { get; set; }

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

                e.HasIndex(m => m.License).IsUnique();
                e.HasIndex(m => new { m.Id, m.License });

                e.Property(m => m.WhiteListed)
                    .IsRequired()
                    .HasMaxLength(1);

                e.Property(m => m.Created)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");
            });

            modelBuilder.Entity<AccountCharacterModel>(e =>
            {
                e.ToTable("account_character");

                e.HasKey(m => m.Id);

                e.HasIndex(m => new { m.Id, m.AccountId });

                e.HasOne(m => m.Position).WithOne().HasForeignKey<AccountCharacterPositionModel>(m => m.ChatacterId).IsRequired().OnDelete(DeleteBehavior.Cascade);
                e.HasOne(m => m.FaceShape).WithOne().HasForeignKey<AccountCharacterFaceShapeModel>(m => m.CharacterId).IsRequired().OnDelete(DeleteBehavior.Cascade);

                e.HasData(new AccountCharacterModel
                {
                    Id = 1,
                    AccountId = 1,
                    Name = "Admin",
                    Surname = "Thalys",
                    DateCreated = DateTime.Now,
                    Gender = 0,
                    Armor = 0,
                    Model = "mp_m_freemode_01"
                });
            });

            modelBuilder.Entity<AccountCharacterPositionModel>(e =>
            {
                e.ToTable("account_character_position");
                e.HasKey(m => m.ChatacterId);

                e.HasData(new AccountCharacterPositionModel
                {
                    ChatacterId = 1,
                    X = 0,
                    Y = 0,
                    Z = 0
                });
            });

            modelBuilder.Entity<AccountCharacterFaceShapeModel>(e =>
            {
                e.ToTable("account_character_faceshape");
                e.HasKey(m => m.CharacterId);
            
                e.HasData(new AccountCharacterFaceShapeModel
                {
                    CharacterId = 1,
                });
            });
        }
    }
}
