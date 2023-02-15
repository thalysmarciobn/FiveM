using Microsoft.EntityFrameworkCore;
using Shared.Models.Database;
using Shared.Enumerations;
using System;
using System.Collections.Generic;
using CitizenFX.Core;

namespace FiveM.Server.Database
{
    public class FiveMContext : DbContext
    {
        private readonly string _connectionString;
        public DbSet<AccountModel> Account { get; set; }
        public DbSet<AccountCharacterModel> AccountCharacter { get; set; }
        public DbSet<AccountCharacterPositionModel> AccountCharactersPosition { get; set; }
        public DbSet<AccountCharacterRotationModel> AccountCharacterRotationModel { get; set; }
        public DbSet<AccountCharacterPedHeadDataModel> AccountCharacterHeritage { get; set; }
        public DbSet<AccountCharacterPedFaceModel> AccountCharactersFaceShape { get; set; }
        public DbSet<AccountCharacterPedComponentModel> AccountCharacterPedComponent { get; set; }
        public DbSet<AccountCharacterPedPropModel> AccountCharacterPedProp { get; set; }
        public DbSet<AccountCharacterPedHeadModel> AccountCharacterAppearance { get; set; }
        public DbSet<AccountCharacterPedHeadOverlayModel> AccountCharacterPedHeadOverlay { get; set; }
        public DbSet<AccountCharacterPedHeadOverlayColorModel> AccountCharacterPedHeadOverlayColor { get; set; }
        public DbSet<ServerVehicleService> ServerVehicleService { get; set; }

        public FiveMContext(string connectionString) =>
            _connectionString = connectionString;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) =>
            optionsBuilder.UseMySql(_connectionString);

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<AccountModel>(e =>
            {
                e.ToTable("account");

                e.HasKey(m => m.Id);

                e.HasIndex(m => new { m.Id, m.License });

                e.Property(m => m.WhiteListed)
                    .IsRequired()
                    .HasMaxLength(1);

                e.HasMany(m => m.Character).WithOne().HasForeignKey(m => m.AccountId).IsRequired().OnDelete(DeleteBehavior.Cascade);

                // e.HasData(new AccountModel
                // {
                //     Id = 1,
                //     License = "07041d870811cccd5a93a5a012970b341d168b9a",
                //     Created = DateTime.Now,
                //     WhiteListed = true
                // });
            });

            modelBuilder.Entity<AccountCharacterModel>(e =>
            {
                e.ToTable("account_character");

                e.HasKey(m => new { m.Id, m.Slot });

                e.HasIndex(m => new { m.Id, m.AccountId });

                e.HasOne(m => m.Position).WithOne().HasForeignKey<AccountCharacterPositionModel>(m => m.ChatacterId).OnDelete(DeleteBehavior.Cascade);
                e.HasOne(m => m.Rotation).WithOne().HasForeignKey<AccountCharacterRotationModel>(m => m.ChatacterId).OnDelete(DeleteBehavior.Cascade);
                e.HasOne(m => m.PedHeadData).WithOne().HasForeignKey<AccountCharacterPedHeadDataModel>(m => m.CharacterId).OnDelete(DeleteBehavior.Cascade);
                e.HasOne(m => m.PedHead).WithOne().HasForeignKey<AccountCharacterPedHeadModel>(m => m.CharacterId).OnDelete(DeleteBehavior.Cascade);
                e.HasMany(m => m.PedFace).WithOne().HasForeignKey(m => m.CharacterId).OnDelete(DeleteBehavior.Cascade);
                e.HasMany(m => m.PedComponent).WithOne().HasForeignKey(m => m.CharacterId).OnDelete(DeleteBehavior.Cascade);
                e.HasMany(m => m.PedProp).WithOne().HasForeignKey(m => m.CharacterId).OnDelete(DeleteBehavior.Cascade);
                e.HasMany(m => m.PedHeadOverlay).WithOne().HasForeignKey(m => m.CharacterId).OnDelete(DeleteBehavior.Cascade);
                e.HasMany(m => m.PedHeadOverlayColor).WithOne().HasForeignKey(m => m.CharacterId).OnDelete(DeleteBehavior.Cascade);

                // e.HasData(new AccountCharacterModel
                // {
                //     Id = 1,
                //     AccountId = 1,
                //     Name = "Admin",
                //     Surname = "Thalys",
                //     DateCreated = DateTime.Now,
                //     Gender = 0,
                //     Armor = 0,
                //     Model = "mp_m_freemode_01"
                // });
            });

            modelBuilder.Entity<AccountCharacterPositionModel>(e =>
            {
                e.ToTable("account_character_position");
                e.HasKey(m => m.ChatacterId);

                // e.HasData(new AccountCharacterPositionModel
                // {
                //     ChatacterId = 1,
                //     X = 0,
                //     Y = 0,
                //     Z = 0
                // });
            });

            modelBuilder.Entity<AccountCharacterRotationModel>(e =>
            {
                e.ToTable("account_character_rotation");
                e.HasKey(m => m.ChatacterId);

                // e.HasData(new AccountCharacterRotationModel
                // {
                //     ChatacterId = 1,
                //     X = 0,
                //     Y = 0,
                //     Z = 0
                // });
            });

            modelBuilder.Entity<AccountCharacterPedFaceModel>(e =>
            {
                e.ToTable("account_character_ped_face");
                e.HasKey(m => new { m.CharacterId, m.Index });

                // var models = new List<AccountCharacterPedFaceModel>();
                // foreach (FaceShapeEnum item in Enum.GetValues(typeof(FaceShapeEnum)))
                // {
                //     models.Add(new AccountCharacterPedFaceModel
                //     {
                //         CharacterId = 1,
                //         Index = item,
                //         Scale = 0
                //     });
                // }
                // e.HasData(models);
            });

            modelBuilder.Entity<AccountCharacterPedHeadDataModel>(e =>
            {
                e.ToTable("account_character_ped_head_data");
                e.HasKey(m => m.CharacterId);

                // e.HasData(new AccountCharacterPedHeadDataModel
                // {
                //     CharacterId = 1,
                // });
            });

            modelBuilder.Entity<AccountCharacterPedComponentModel>(e =>
            {
                e.ToTable("account_character_ped_component");
                e.HasKey(m => new { m.CharacterId, m.ComponentId });

                // var models = new List<AccountCharacterPedComponentModel>();
                // foreach (ComponentVariationEnum item in Enum.GetValues(typeof(ComponentVariationEnum)))
                // {
                //     models.Add(new AccountCharacterPedComponentModel
                //     {
                //         CharacterId = 1,
                //         ComponentId = item,
                //         Index = 0,
                //         Texture = 0
                //     });
                // }
                // e.HasData(models);
            });

            modelBuilder.Entity<AccountCharacterPedPropModel>(e =>
            {
                e.ToTable("account_character_ped_prop");
                e.HasKey(m => new { m.CharacterId, m.PropId });

                // var models = new List<AccountCharacterPedPropModel>();
                // foreach (PropVariationEnum item in Enum.GetValues(typeof(PropVariationEnum)))
                // {
                //     models.Add(new AccountCharacterPedPropModel
                //     {
                //         CharacterId = 1,
                //         PropId = item,
                //         Index = 0,
                //         Texture = 0
                //     });
                // }
                // e.HasData(models);
            });

            modelBuilder.Entity<AccountCharacterPedHeadModel>(e =>
            {
                e.ToTable("account_character_ped_head");
                e.HasKey(m => m.CharacterId);

                // e.HasData(new AccountCharacterPedHeadModel
                // {
                //     CharacterId = 1,
                // });
            });

            modelBuilder.Entity<AccountCharacterPedHeadOverlayModel>(e =>
            {
                e.ToTable("account_character_ped_head_overlay");
                e.HasKey(m => new { m.CharacterId, m.OverlayId });

                // var models = new List<AccountCharacterPedHeadOverlayModel>();
                // foreach (OverlayEnum item in Enum.GetValues(typeof(OverlayEnum)))
                // {
                //     models.Add(new AccountCharacterPedHeadOverlayModel
                //     {
                //         CharacterId = 1,
                //         OverlayId = item
                //     });
                // }
                // e.HasData(models);
            });

            modelBuilder.Entity<AccountCharacterPedHeadOverlayColorModel>(e =>
            {
                e.ToTable("account_character_ped_head_overlay_color");
                e.HasKey(m => new { m.CharacterId, m.OverlayId });
            });

            modelBuilder.Entity<ServerVehicleService>(e =>
            {
                e.ToTable("server_vehicle_service");
                e.HasKey(m => new { m.Id });
                e.HasData(new []
                {
                    new ServerVehicleService
                    {
                        Id = 1,
                        // VehicleHash.Bus
                        Model = 3338918751u,
                        // PedHash.LesterCrest
                        Driver = 1302784073u,
                        X = -1049.6491f,
                        Y = -2719.0270f,
                        Z = 13.7566f,
                    },
                    new ServerVehicleService
                    {
                        Id = 2,
                        // VehicleHash.Bus
                        Model = 3338918751u,
                        // PedHash.LesterCrest
                        Driver = 1302784073u,
                        X = -1041.9746f,
                        Y = -2721.6181f,
                        Z = 13.7566f,
                    },
                    new ServerVehicleService
                    {
                        Id = 3,
                        // VehicleHash.Bus
                        Model = 3338918751u,
                        // PedHash.LesterCrest
                        Driver = 1302784073u,
                        X = -1026.4173f,
                        Y = -2730.4631f,
                        Z = 13.7566f,
                    },
                    new ServerVehicleService
                    {
                        Id = 4,
                        // VehicleHash.Bus
                        Model = 3338918751u,
                        // PedHash.LesterCrest
                        Driver = 1302784073u,
                        X = -1014.7446f,
                        Y = -2737.0578f,
                        Z = 13.7566f,
                    }
                });
            });
        }
    }
}
