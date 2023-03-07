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
        public DbSet<BlipModel> Blip { get; set; }
        public DbSet<ServerVehicleService> ServerVehicleService { get; set; }
        public DbSet<VehicleModel> Vehicles { get; set; }

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

                e.HasIndex(m => new { m.Id, m.License }).IsUnique();

                e.Property(m => m.WhiteListed)
                    .IsRequired()
                    .HasMaxLength(1);

                e.HasMany(m => m.Character).WithOne().HasPrincipalKey(m => m.Id).HasForeignKey(m => m.AccountId).IsRequired().OnDelete(DeleteBehavior.Cascade);

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

                e.HasKey(m => m.Id);

                e.HasIndex(m => new { m.Id, m.AccountId, m.Slot }).IsUnique();

                e.HasOne(m => m.Position).WithOne().HasForeignKey<AccountCharacterPositionModel>(m => m.CharacterId).OnDelete(DeleteBehavior.Cascade);
                e.HasOne(m => m.Rotation).WithOne().HasForeignKey<AccountCharacterRotationModel>(m => m.CharacterId).OnDelete(DeleteBehavior.Cascade);
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
                e.HasKey(m => m.CharacterId);

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
                e.HasKey(m => m.CharacterId);

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
                e.HasKey(m => m.Id);

                e.HasData(new[]
                {
                    new ServerVehicleService
                    {
                        Id = 1,
                        // VehicleHash.Bus
                        Model = 3338918751u,
                        // PedHash
                        Driver = 1885233650u,
                        // Control.Pickup
                        Key = 38,
                        Title = "TAXI PRAÇA",
                        DriveToX = 134.954f,
                        DriveToY = -1023.76f,
                        DriveToZ = 28.8165f,
                        SpawnX = -1051.63f,
                        SpawnY = -2712.7f,
                        SpawnZ = 14f,
                        SpawnHeading = 240.2623f,
                        MarkX = -1049.6491f,
                        MarkY = -2719.0270f,
                        MarkZ = 13.7566f,
                    },
                    new ServerVehicleService
                    {
                        Id = 2,
                        // VehicleHash.Bus
                        Model = 3338918751u,
                        // PedHash
                        Driver = 1885233650u,
                        // Control.Pickup
                        Key = 38,
                        Title = "Taxi Casino",
                        DriveToX = 918.015f,
                        DriveToY = 50.655f,
                        DriveToZ = 80.247f,
                        SpawnX = -1040.689f,
                        SpawnY = -2719.0954f,
                        SpawnZ = 13.280f,
                        SpawnHeading = 240.2623f,
                        MarkX = -1041.9746f,
                        MarkY = -2721.6181f,
                        MarkZ = 13.7566f,
                    },
                    new ServerVehicleService
                    {
                        Id = 3,
                        // VehicleHash.Bus
                        Model = 3338918751u,
                        // PedHash
                        Driver = 1885233650u,
                        // Control.Pickup
                        Key = 38,
                        Title = "Taxi Del Perro",
                        DriveToX = -1596.211f,
                        DriveToY = -1044.552f,
                        DriveToZ = 12.533f,
                        SpawnX = -1024.145f,
                        SpawnY = -2728.840f,
                        SpawnZ = 13.272f,
                        SpawnHeading = 238.933f,
                        MarkX = -1026.4173f,
                        MarkY = -2730.4631f,
                        MarkZ = 13.7566f,
                    },
                    new ServerVehicleService
                    {
                        Id = 4,
                        // VehicleHash.Bus
                        Model = 3338918751u,
                        // PedHash
                        Driver = 1885233650u,
                        // Control.Pickup
                        Key = 38,
                        Title = "Chamar Taxi",
                        MarkX = -1014.7446f,
                        MarkY = -2737.0578f,
                        MarkZ = 13.7566f,
                    }
                });
            });

            modelBuilder.Entity<BlipModel>(e =>
            {
                e.ToTable("blips");
                e.HasKey(m => m.Id);

                e.HasData(new[]
                {
                    new BlipModel
                    {
                        Id = 1,
                        BlipId = 198,
                        DisplayId = 4,
                        Title = "Terminal de Taxi",
                        Color = 70,
                        Scale = 0.9f,
                        ShortRange = true,
                        X = -1033.49f,
                        Y = -2727.02f,
                        Z = 13.75f
                    },
                    new BlipModel
                    {
                        Id = 2,
                        BlipId = 50,
                        DisplayId = 4,
                        Title = "Estacionamento",
                        Color = 0,
                        Scale = 0.9f,
                        ShortRange = true,
                        X = 98.95f,
                        Y = -1067.59f,
                        Z = 29.29f
                    },
                    new BlipModel
                    {
                        Id = 3,
                        BlipId = 50,
                        DisplayId = 4,
                        Title = "Estacionamento",
                        Color = 0,
                        Scale = 0.9f,
                        ShortRange = true,
                        X = 44.37f,
                        Y = -865.29f,
                        Z = 30.53f
                    },
                    new BlipModel
                    {
                        Id = 4,
                        BlipId = 50,
                        DisplayId = 4,
                        Title = "Estacionamento",
                        Color = 0,
                        Scale = 0.9f,
                        ShortRange = true,
                        X = 208.75f,
                        Y = -808.06f,
                        Z = 30.88f
                    },
                    new BlipModel
                    {
                        Id = 5,
                        BlipId = 50,
                        DisplayId = 4,
                        Title = "Estacionamento",
                        Color = 0,
                        Scale = 0.9f,
                        ShortRange = true,
                        X = 606.64f,
                        Y = 73.82f,
                        Z = 91.93f
                    },
                    new BlipModel
                    {
                        Id = 6,
                        BlipId = 50,
                        DisplayId = 4,
                        Title = "Estacionamento",
                        Color = 0,
                        Scale = 0.9f,
                        ShortRange = true,
                        X = -1179.20f,
                        Y = -1507.13f,
                        Z = 4.37f
                    },
                    new BlipModel
                    {
                        Id = 7,
                        BlipId = 50,
                        DisplayId = 4,
                        Title = "Estacionamento",
                        Color = 0,
                        Scale = 0.9f,
                        ShortRange = true,
                        X = -1160.81f,
                        Y = -726.49f,
                        Z = 20.57f
                    },
                    new BlipModel
                    {
                        Id = 8,
                        BlipId = 50,
                        DisplayId = 4,
                        Title = "Estacionamento",
                        Color = 0,
                        Scale = 0.9f,
                        ShortRange = true,
                        X = -335.82f,
                        Y = 264.83f,
                        Z = 85.89f
                    },
                    new BlipModel
                    {
                        Id = 9,
                        BlipId = 50,
                        DisplayId = 4,
                        Title = "Estacionamento",
                        Color = 0,
                        Scale = 0.9f,
                        ShortRange = true,
                        X = 58.30f,
                        Y = -624.23f,
                        Z = 31.66f
                    },
                    new BlipModel
                    {
                        Id = 10,
                        BlipId = 408,
                        DisplayId = 4,
                        Title = "Dominos Pizza",
                        Color = 26,
                        Scale = 0.9f,
                        ShortRange = true,
                        X = 536.19f,
                        Y = 98.79f,
                        Z = 96.44f
                    },
                    new BlipModel
                    {
                        Id = 11,
                        BlipId = 826,
                        DisplayId = 4,
                        Title = "Agência de Taxi",
                        Color = 26,
                        Scale = 0.9f,
                        ShortRange = true,
                        X = 913.76f,
                        Y = -179.71f,
                        Z = 74.16f
                    },
                    new BlipModel
                    {
                        Id = 12,
                        BlipId = 410,
                        DisplayId = 4,
                        Title = "Terminal Marítimo",
                        Color = 46,
                        Scale = 0.9f,
                        ShortRange = true,
                        X = 1299.20f,
                        Y = 4217.90f,
                        Z = 33.90f
                    },
                    new BlipModel
                    {
                        Id = 13,
                        BlipId = 679,
                        DisplayId = 4,
                        Title = "Cassino",
                        Color = 0,
                        Scale = 0.9f,
                        ShortRange = true,
                        X = 917.37f,
                        Y = 50.76f,
                        Z = 80.76f
                    }
                });
            });

            modelBuilder.Entity<VehicleModel>(e =>
            {
                e.ToTable("vehicle");

                e.HasKey(m => m.Id);

                e.HasIndex(m => new { m.Id, m.CharacterId }).IsUnique();

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
        }
    }
}
