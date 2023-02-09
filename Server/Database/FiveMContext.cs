using Microsoft.EntityFrameworkCore;
using Shared.Models.Database;
using Shared.Enumerations;
using System;
using System.Collections.Generic;

namespace FiveM.Server.Database
{
    public class FiveMContext : DbContext
    {
        public DbSet<AccountModel> Accounts { get; set; }
        public DbSet<AccountCharacterModel> AccountCharacter { get; set; }
        public DbSet<AccountCharacterPositionModel> AccountCharactersPosition { get; set; }
        public DbSet<AccountCharacterPedHeadDataModel> AccountCharacterHeritage { get; set; }
        public DbSet<AccountCharacterPedFaceModel> AccountCharactersFaceShape { get; set; }
        public DbSet<AccountCharacterPedComponentModel> AccountCharacterPedComponent { get; set; }
        public DbSet<AccountCharacterPedPropModel> AccountCharacterPedProp { get; set; }
        public DbSet<AccountCharacterPedHeadModel> AccountCharacterAppearance { get; set; }
        public DbSet<AccountCharacterPedHeadOverlayModel> AccountCharacterPedHeadOverlay { get; set; }
        public DbSet<AccountCharacterPedHeadOverlayColorModel> AccountCharacterPedHeadOverlayColor { get; set; }

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
                e.HasOne(m => m.PedHeadData).WithOne().HasForeignKey<AccountCharacterPedHeadDataModel>(m => m.CharacterId).IsRequired().OnDelete(DeleteBehavior.Cascade);
                e.HasOne(m => m.PedHead).WithOne().HasForeignKey<AccountCharacterPedHeadModel>(m => m.CharacterId).IsRequired().OnDelete(DeleteBehavior.Cascade);
                e.HasMany(m => m.PedFace).WithOne().HasForeignKey(m => m.CharacterId).IsRequired().OnDelete(DeleteBehavior.Cascade);
                e.HasMany(m => m.PedComponent).WithOne().HasForeignKey(m => m.CharacterId).IsRequired().OnDelete(DeleteBehavior.Cascade);
                e.HasMany(m => m.PedProp).WithOne().HasForeignKey(m => m.CharacterId).IsRequired().OnDelete(DeleteBehavior.Cascade);
                e.HasMany(m => m.PedHeadOverlay).WithOne().HasForeignKey(m => m.CharacterId).IsRequired().OnDelete(DeleteBehavior.Cascade);
                e.HasMany(m => m.PedHeadOverlayColor).WithOne().HasForeignKey(m => m.CharacterId).IsRequired().OnDelete(DeleteBehavior.Cascade);

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

            modelBuilder.Entity<AccountCharacterPedFaceModel>(e =>
            {
                e.ToTable("account_character_ped_face");
                e.HasKey(m => new { m.CharacterId, m.Index });

                var models = new List<AccountCharacterPedFaceModel>();
                foreach (FaceShapeEnum item in Enum.GetValues(typeof(FaceShapeEnum)))
                {
                    models.Add(new AccountCharacterPedFaceModel
                    {
                        CharacterId = 1,
                        Index = item,
                        Scale = 0
                    });
                }
                e.HasData(models);
            });

            modelBuilder.Entity<AccountCharacterPedHeadDataModel>(e =>
            {
                e.ToTable("account_character_ped_head_data");
                e.HasKey(m => m.CharacterId);

                e.HasData(new AccountCharacterPedHeadDataModel
                {
                    CharacterId = 1,
                });
            });

            modelBuilder.Entity<AccountCharacterPedComponentModel>(e =>
            {
                e.ToTable("account_character_ped_component");
                e.HasKey(m => new { m.CharacterId, m.ComponentId });

                var models = new List<AccountCharacterPedComponentModel>();
                foreach (ComponentVariationEnum item in Enum.GetValues(typeof(ComponentVariationEnum)))
                {
                    models.Add(new AccountCharacterPedComponentModel
                    {
                        CharacterId = 1,
                        ComponentId = item,
                        Index = 0,
                        Texture = 0
                    });
                }
                e.HasData(models);
            });

            modelBuilder.Entity<AccountCharacterPedPropModel>(e =>
            {
                e.ToTable("account_character_ped_prop");
                e.HasKey(m => new { m.CharacterId, m.PropId });

                var models = new List<AccountCharacterPedPropModel>();
                foreach (PropVariationEnum item in Enum.GetValues(typeof(PropVariationEnum)))
                {
                    models.Add(new AccountCharacterPedPropModel
                    {
                        CharacterId = 1,
                        PropId = item,
                        Index = 0,
                        Texture = 0
                    });
                }
                e.HasData(models);
            });

            modelBuilder.Entity<AccountCharacterPedHeadModel>(e =>
            {
                e.ToTable("account_character_ped_head");
                e.HasKey(m => m.CharacterId);

                e.HasData(new AccountCharacterPedHeadModel
                {
                    CharacterId = 1,
                });
            });

            modelBuilder.Entity<AccountCharacterPedHeadOverlayModel>(e =>
            {
                e.ToTable("account_character_ped_head_overlay");
                e.HasKey(m => new { m.CharacterId, m.OverlayId });

                var models = new List<AccountCharacterPedHeadOverlayModel>();
                foreach (OverlayEnum item in Enum.GetValues(typeof(OverlayEnum)))
                {
                    models.Add(new AccountCharacterPedHeadOverlayModel
                    {
                        CharacterId = 1,
                        OverlayId = item
                    });
                }
                e.HasData(models);
            });

            modelBuilder.Entity<AccountCharacterPedHeadOverlayColorModel>(e =>
            {
                e.ToTable("account_character_ped_head_overlay_color");
                e.HasKey(m => new { m.CharacterId, m.OverlayId });
            });
        }
    }
}
