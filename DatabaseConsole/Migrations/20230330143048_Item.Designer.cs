﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DatabaseConsole.Migrations
{
    [DbContext(typeof(Context))]
    [Migration("20230330143048_Item")]
    partial class Item
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.32")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("Shared.Models.Database.AccountCharacterItemModel", b =>
                {
                    b.Property<long>("Id")
                        .HasColumnType("bigint");

                    b.Property<long>("CharacterId")
                        .HasColumnType("bigint");

                    b.Property<bool>("Deleted")
                        .HasColumnType("tinyint(1)");

                    b.Property<bool>("Equipped")
                        .HasColumnType("tinyint(1)");

                    b.Property<long>("ItemId")
                        .HasColumnType("bigint");

                    b.Property<int>("Quantity")
                        .HasColumnType("int");

                    b.Property<int>("Slot")
                        .HasColumnType("int");

                    b.Property<DateTime>("Time")
                        .HasColumnType("datetime(6)");

                    b.HasKey("Id", "CharacterId");

                    b.HasIndex("CharacterId");

                    b.ToTable("account_character_item");
                });

            modelBuilder.Entity("Shared.Models.Database.AccountCharacterModel", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    b.Property<long>("AccountId")
                        .HasColumnType("bigint");

                    b.Property<int>("Armor")
                        .HasColumnType("int");

                    b.Property<int>("BankBalance")
                        .HasColumnType("int");

                    b.Property<DateTime>("DateCreated")
                        .HasColumnType("datetime(6)");

                    b.Property<int>("EyeColorId")
                        .HasColumnType("int");

                    b.Property<float>("Heading")
                        .HasColumnType("float");

                    b.Property<int>("Health")
                        .HasColumnType("int");

                    b.Property<string>("Model")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<int>("MoneyBalance")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<int>("Slot")
                        .HasColumnType("int");

                    b.Property<string>("Surname")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.HasKey("Id");

                    b.HasIndex("AccountId");

                    b.HasIndex("Id", "AccountId", "Slot")
                        .IsUnique();

                    b.ToTable("account_character");
                });

            modelBuilder.Entity("Shared.Models.Database.AccountCharacterPedComponentModel", b =>
                {
                    b.Property<long>("CharacterId")
                        .HasColumnType("bigint");

                    b.Property<int>("ComponentId")
                        .HasColumnType("int");

                    b.Property<int>("DrawableId")
                        .HasColumnType("int");

                    b.Property<int>("PalleteId")
                        .HasColumnType("int");

                    b.Property<int>("TextureId")
                        .HasColumnType("int");

                    b.HasKey("CharacterId", "ComponentId");

                    b.ToTable("account_character_ped_component");
                });

            modelBuilder.Entity("Shared.Models.Database.AccountCharacterPedFaceModel", b =>
                {
                    b.Property<long>("CharacterId")
                        .HasColumnType("bigint");

                    b.Property<int>("Index")
                        .HasColumnType("int");

                    b.Property<float>("Scale")
                        .HasColumnType("float");

                    b.HasKey("CharacterId", "Index");

                    b.ToTable("account_character_ped_face");
                });

            modelBuilder.Entity("Shared.Models.Database.AccountCharacterPedHeadDataModel", b =>
                {
                    b.Property<long>("CharacterId")
                        .HasColumnType("bigint");

                    b.Property<bool>("IsParent")
                        .HasColumnType("tinyint(1)");

                    b.Property<int>("ShapeFirstID")
                        .HasColumnType("int");

                    b.Property<float>("ShapeMix")
                        .HasColumnType("float");

                    b.Property<int>("ShapeSecondID")
                        .HasColumnType("int");

                    b.Property<int>("ShapeThirdID")
                        .HasColumnType("int");

                    b.Property<int>("SkinFirstID")
                        .HasColumnType("int");

                    b.Property<float>("SkinMix")
                        .HasColumnType("float");

                    b.Property<int>("SkinSecondID")
                        .HasColumnType("int");

                    b.Property<int>("SkinThirdID")
                        .HasColumnType("int");

                    b.Property<float>("ThirdMix")
                        .HasColumnType("float");

                    b.HasKey("CharacterId");

                    b.ToTable("account_character_ped_head_data");
                });

            modelBuilder.Entity("Shared.Models.Database.AccountCharacterPedHeadModel", b =>
                {
                    b.Property<long>("CharacterId")
                        .HasColumnType("bigint");

                    b.Property<int>("HairColorId")
                        .HasColumnType("int");

                    b.Property<int>("HairHighlightColor")
                        .HasColumnType("int");

                    b.HasKey("CharacterId");

                    b.ToTable("account_character_ped_head");
                });

            modelBuilder.Entity("Shared.Models.Database.AccountCharacterPedHeadOverlayColorModel", b =>
                {
                    b.Property<long>("CharacterId")
                        .HasColumnType("bigint");

                    b.Property<int>("OverlayId")
                        .HasColumnType("int");

                    b.Property<int>("ColorId")
                        .HasColumnType("int");

                    b.Property<int>("ColorType")
                        .HasColumnType("int");

                    b.Property<int>("SecondColorId")
                        .HasColumnType("int");

                    b.HasKey("CharacterId", "OverlayId");

                    b.ToTable("account_character_ped_head_overlay_color");
                });

            modelBuilder.Entity("Shared.Models.Database.AccountCharacterPedHeadOverlayModel", b =>
                {
                    b.Property<long>("CharacterId")
                        .HasColumnType("bigint");

                    b.Property<int>("OverlayId")
                        .HasColumnType("int");

                    b.Property<int>("Index")
                        .HasColumnType("int");

                    b.Property<float>("Opacity")
                        .HasColumnType("float");

                    b.HasKey("CharacterId", "OverlayId");

                    b.ToTable("account_character_ped_head_overlay");
                });

            modelBuilder.Entity("Shared.Models.Database.AccountCharacterPedPropModel", b =>
                {
                    b.Property<long>("CharacterId")
                        .HasColumnType("bigint");

                    b.Property<int>("PropId")
                        .HasColumnType("int");

                    b.Property<bool>("Attach")
                        .HasColumnType("tinyint(1)");

                    b.Property<int>("ComponentId")
                        .HasColumnType("int");

                    b.Property<int>("DrawableId")
                        .HasColumnType("int");

                    b.Property<int>("TextureId")
                        .HasColumnType("int");

                    b.HasKey("CharacterId", "PropId");

                    b.ToTable("account_character_ped_prop");
                });

            modelBuilder.Entity("Shared.Models.Database.AccountCharacterPositionModel", b =>
                {
                    b.Property<long>("CharacterId")
                        .HasColumnType("bigint");

                    b.Property<float>("X")
                        .HasColumnType("float");

                    b.Property<float>("Y")
                        .HasColumnType("float");

                    b.Property<float>("Z")
                        .HasColumnType("float");

                    b.HasKey("CharacterId");

                    b.ToTable("account_character_position");
                });

            modelBuilder.Entity("Shared.Models.Database.AccountCharacterRotationModel", b =>
                {
                    b.Property<long>("CharacterId")
                        .HasColumnType("bigint");

                    b.Property<float>("X")
                        .HasColumnType("float");

                    b.Property<float>("Y")
                        .HasColumnType("float");

                    b.Property<float>("Z")
                        .HasColumnType("float");

                    b.HasKey("CharacterId");

                    b.ToTable("account_character_rotation");
                });

            modelBuilder.Entity("Shared.Models.Database.AccountModel", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    b.Property<DateTime>("Created")
                        .HasColumnType("datetime(6)");

                    b.Property<long>("CurrentCharacter")
                        .HasColumnType("bigint");

                    b.Property<string>("LastAddress")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("License")
                        .HasColumnType("varchar(255) CHARACTER SET utf8mb4");

                    b.Property<bool>("WhiteListed")
                        .HasColumnType("tinyint(1)")
                        .HasMaxLength(1);

                    b.HasKey("Id");

                    b.HasIndex("Id", "License")
                        .IsUnique();

                    b.ToTable("account");
                });

            modelBuilder.Entity("Shared.Models.Database.BlipModel", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    b.Property<int>("BlipId")
                        .HasColumnType("int");

                    b.Property<int>("Color")
                        .HasColumnType("int");

                    b.Property<int>("DisplayId")
                        .HasColumnType("int");

                    b.Property<float>("Scale")
                        .HasColumnType("float");

                    b.Property<bool>("ShortRange")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("Title")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<float>("X")
                        .HasColumnType("float");

                    b.Property<float>("Y")
                        .HasColumnType("float");

                    b.Property<float>("Z")
                        .HasColumnType("float");

                    b.HasKey("Id");

                    b.ToTable("blips");

                    b.HasData(
                        new
                        {
                            Id = 1L,
                            BlipId = 198,
                            Color = 70,
                            DisplayId = 4,
                            Scale = 0.9f,
                            ShortRange = true,
                            Title = "Terminal de Taxi",
                            X = -1033.49f,
                            Y = -2727.02f,
                            Z = 13.75f
                        },
                        new
                        {
                            Id = 2L,
                            BlipId = 50,
                            Color = 0,
                            DisplayId = 4,
                            Scale = 0.9f,
                            ShortRange = true,
                            Title = "Estacionamento",
                            X = 98.95f,
                            Y = -1067.59f,
                            Z = 29.29f
                        },
                        new
                        {
                            Id = 3L,
                            BlipId = 50,
                            Color = 0,
                            DisplayId = 4,
                            Scale = 0.9f,
                            ShortRange = true,
                            Title = "Estacionamento",
                            X = 44.37f,
                            Y = -865.29f,
                            Z = 30.53f
                        },
                        new
                        {
                            Id = 4L,
                            BlipId = 50,
                            Color = 0,
                            DisplayId = 4,
                            Scale = 0.9f,
                            ShortRange = true,
                            Title = "Estacionamento",
                            X = 208.75f,
                            Y = -808.06f,
                            Z = 30.88f
                        },
                        new
                        {
                            Id = 5L,
                            BlipId = 50,
                            Color = 0,
                            DisplayId = 4,
                            Scale = 0.9f,
                            ShortRange = true,
                            Title = "Estacionamento",
                            X = 606.64f,
                            Y = 73.82f,
                            Z = 91.93f
                        },
                        new
                        {
                            Id = 6L,
                            BlipId = 50,
                            Color = 0,
                            DisplayId = 4,
                            Scale = 0.9f,
                            ShortRange = true,
                            Title = "Estacionamento",
                            X = -1179.2f,
                            Y = -1507.13f,
                            Z = 4.37f
                        },
                        new
                        {
                            Id = 7L,
                            BlipId = 50,
                            Color = 0,
                            DisplayId = 4,
                            Scale = 0.9f,
                            ShortRange = true,
                            Title = "Estacionamento",
                            X = -1160.81f,
                            Y = -726.49f,
                            Z = 20.57f
                        },
                        new
                        {
                            Id = 8L,
                            BlipId = 50,
                            Color = 0,
                            DisplayId = 4,
                            Scale = 0.9f,
                            ShortRange = true,
                            Title = "Estacionamento",
                            X = -335.82f,
                            Y = 264.83f,
                            Z = 85.89f
                        },
                        new
                        {
                            Id = 9L,
                            BlipId = 50,
                            Color = 0,
                            DisplayId = 4,
                            Scale = 0.9f,
                            ShortRange = true,
                            Title = "Estacionamento",
                            X = 58.3f,
                            Y = -624.23f,
                            Z = 31.66f
                        },
                        new
                        {
                            Id = 10L,
                            BlipId = 408,
                            Color = 26,
                            DisplayId = 4,
                            Scale = 0.9f,
                            ShortRange = true,
                            Title = "Dominos Pizza",
                            X = 536.19f,
                            Y = 98.79f,
                            Z = 96.44f
                        },
                        new
                        {
                            Id = 11L,
                            BlipId = 826,
                            Color = 26,
                            DisplayId = 4,
                            Scale = 0.9f,
                            ShortRange = true,
                            Title = "Agência de Taxi",
                            X = 913.76f,
                            Y = -179.71f,
                            Z = 74.16f
                        },
                        new
                        {
                            Id = 12L,
                            BlipId = 410,
                            Color = 46,
                            DisplayId = 4,
                            Scale = 0.9f,
                            ShortRange = true,
                            Title = "Terminal Marítimo",
                            X = 1299.2f,
                            Y = 4217.9f,
                            Z = 33.9f
                        },
                        new
                        {
                            Id = 13L,
                            BlipId = 679,
                            Color = 0,
                            DisplayId = 4,
                            Scale = 0.9f,
                            ShortRange = true,
                            Title = "Cassino",
                            X = 917.37f,
                            Y = 50.76f,
                            Z = 80.76f
                        });
                });

            modelBuilder.Entity("Shared.Models.Database.ServerVehicleService", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    b.Property<float>("DriveToX")
                        .HasColumnType("float");

                    b.Property<float>("DriveToY")
                        .HasColumnType("float");

                    b.Property<float>("DriveToZ")
                        .HasColumnType("float");

                    b.Property<uint>("Driver")
                        .HasColumnType("int unsigned");

                    b.Property<int>("Key")
                        .HasColumnType("int");

                    b.Property<float>("MarkX")
                        .HasColumnType("float");

                    b.Property<float>("MarkY")
                        .HasColumnType("float");

                    b.Property<float>("MarkZ")
                        .HasColumnType("float");

                    b.Property<uint>("Model")
                        .HasColumnType("int unsigned");

                    b.Property<float>("SpawnHeading")
                        .HasColumnType("float");

                    b.Property<float>("SpawnX")
                        .HasColumnType("float");

                    b.Property<float>("SpawnY")
                        .HasColumnType("float");

                    b.Property<float>("SpawnZ")
                        .HasColumnType("float");

                    b.Property<string>("Title")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.HasKey("Id");

                    b.ToTable("server_vehicle_service");

                    b.HasData(
                        new
                        {
                            Id = 1L,
                            DriveToX = 134.954f,
                            DriveToY = -1023.76f,
                            DriveToZ = 28.8165f,
                            Driver = 1885233650u,
                            Key = 38,
                            MarkX = -1049.649f,
                            MarkY = -2719.027f,
                            MarkZ = 13.7566f,
                            Model = 3338918751u,
                            SpawnHeading = 240.2623f,
                            SpawnX = -1051.63f,
                            SpawnY = -2712.7f,
                            SpawnZ = 14f,
                            Title = "TAXI PRAÇA"
                        },
                        new
                        {
                            Id = 2L,
                            DriveToX = 918.015f,
                            DriveToY = 50.655f,
                            DriveToZ = 80.247f,
                            Driver = 1885233650u,
                            Key = 38,
                            MarkX = -1041.9746f,
                            MarkY = -2721.6182f,
                            MarkZ = 13.7566f,
                            Model = 3338918751u,
                            SpawnHeading = 240.2623f,
                            SpawnX = -1040.689f,
                            SpawnY = -2719.0955f,
                            SpawnZ = 13.28f,
                            Title = "Taxi Casino"
                        },
                        new
                        {
                            Id = 3L,
                            DriveToX = -1596.211f,
                            DriveToY = -1044.552f,
                            DriveToZ = 12.533f,
                            Driver = 1885233650u,
                            Key = 38,
                            MarkX = -1026.4174f,
                            MarkY = -2730.4631f,
                            MarkZ = 13.7566f,
                            Model = 3338918751u,
                            SpawnHeading = 238.933f,
                            SpawnX = -1024.145f,
                            SpawnY = -2728.84f,
                            SpawnZ = 13.272f,
                            Title = "Taxi Del Perro"
                        },
                        new
                        {
                            Id = 4L,
                            DriveToX = 0f,
                            DriveToY = 0f,
                            DriveToZ = 0f,
                            Driver = 1885233650u,
                            Key = 38,
                            MarkX = -1014.7446f,
                            MarkY = -2737.0579f,
                            MarkZ = 13.7566f,
                            Model = 3338918751u,
                            SpawnHeading = 0f,
                            SpawnX = 0f,
                            SpawnY = 0f,
                            SpawnZ = 0f,
                            Title = "Chamar Taxi"
                        });
                });

            modelBuilder.Entity("Shared.Models.Database.VehicleModModel", b =>
                {
                    b.Property<long>("VehicleId")
                        .HasColumnType("bigint");

                    b.Property<int>("Index")
                        .HasColumnType("int");

                    b.Property<int>("Type")
                        .HasColumnType("int");

                    b.HasKey("VehicleId");

                    b.HasIndex("VehicleId")
                        .IsUnique();

                    b.ToTable("vehicle_mod");
                });

            modelBuilder.Entity("Shared.Models.Database.VehicleModel", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    b.Property<float>("BodyHealth")
                        .HasColumnType("float");

                    b.Property<long>("CharacterId")
                        .HasColumnType("bigint");

                    b.Property<int>("CustomPrimaryColourB")
                        .HasColumnType("int");

                    b.Property<int>("CustomPrimaryColourG")
                        .HasColumnType("int");

                    b.Property<int>("CustomPrimaryColourR")
                        .HasColumnType("int");

                    b.Property<int>("CustomSecondaryColourB")
                        .HasColumnType("int");

                    b.Property<int>("CustomSecondaryColourG")
                        .HasColumnType("int");

                    b.Property<int>("CustomSecondaryColourR")
                        .HasColumnType("int");

                    b.Property<int>("DashboardColor")
                        .HasColumnType("int");

                    b.Property<float>("DirtLevel")
                        .HasColumnType("float");

                    b.Property<int>("DoorLockStatus")
                        .HasColumnType("int");

                    b.Property<int>("DoorStatus")
                        .HasColumnType("int");

                    b.Property<int>("DoorsLockedForPlayer")
                        .HasColumnType("int");

                    b.Property<float>("EngineHealth")
                        .HasColumnType("float");

                    b.Property<bool>("Handbrake")
                        .HasColumnType("tinyint(1)");

                    b.Property<int>("HeadlightsColour")
                        .HasColumnType("int");

                    b.Property<bool>("HighbeamsOn")
                        .HasColumnType("tinyint(1)");

                    b.Property<int>("HomingLockonState")
                        .HasColumnType("int");

                    b.Property<int>("InteriorColor")
                        .HasColumnType("int");

                    b.Property<bool>("LightsOn")
                        .HasColumnType("tinyint(1)");

                    b.Property<int>("Livery")
                        .HasColumnType("int");

                    b.Property<uint>("Model")
                        .HasColumnType("int unsigned");

                    b.Property<string>("NumberPlateText")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<int>("NumberPlateTextIndex")
                        .HasColumnType("int");

                    b.Property<int>("PearlColour")
                        .HasColumnType("int");

                    b.Property<float>("PetrolTankHealth")
                        .HasColumnType("float");

                    b.Property<int>("PrimaryColour")
                        .HasColumnType("int");

                    b.Property<int>("RoofLivery")
                        .HasColumnType("int");

                    b.Property<int>("SecondaryColour")
                        .HasColumnType("int");

                    b.Property<int>("TyreSmokeColorB")
                        .HasColumnType("int");

                    b.Property<int>("TyreSmokeColorG")
                        .HasColumnType("int");

                    b.Property<int>("TyreSmokeColorR")
                        .HasColumnType("int");

                    b.Property<int>("WheelColour")
                        .HasColumnType("int");

                    b.Property<int>("WheelType")
                        .HasColumnType("int");

                    b.Property<int>("WindowTint")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("Id", "CharacterId")
                        .IsUnique();

                    b.ToTable("vehicle");
                });

            modelBuilder.Entity("Shared.Models.Database.AccountCharacterItemModel", b =>
                {
                    b.HasOne("Shared.Models.Database.AccountCharacterModel", null)
                        .WithMany("Items")
                        .HasForeignKey("CharacterId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Shared.Models.Database.AccountCharacterModel", b =>
                {
                    b.HasOne("Shared.Models.Database.AccountModel", null)
                        .WithMany("Character")
                        .HasForeignKey("AccountId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Shared.Models.Database.AccountCharacterPedComponentModel", b =>
                {
                    b.HasOne("Shared.Models.Database.AccountCharacterModel", null)
                        .WithMany("PedComponent")
                        .HasForeignKey("CharacterId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Shared.Models.Database.AccountCharacterPedFaceModel", b =>
                {
                    b.HasOne("Shared.Models.Database.AccountCharacterModel", null)
                        .WithMany("PedFace")
                        .HasForeignKey("CharacterId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Shared.Models.Database.AccountCharacterPedHeadDataModel", b =>
                {
                    b.HasOne("Shared.Models.Database.AccountCharacterModel", null)
                        .WithOne("PedHeadData")
                        .HasForeignKey("Shared.Models.Database.AccountCharacterPedHeadDataModel", "CharacterId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Shared.Models.Database.AccountCharacterPedHeadModel", b =>
                {
                    b.HasOne("Shared.Models.Database.AccountCharacterModel", null)
                        .WithOne("PedHead")
                        .HasForeignKey("Shared.Models.Database.AccountCharacterPedHeadModel", "CharacterId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Shared.Models.Database.AccountCharacterPedHeadOverlayColorModel", b =>
                {
                    b.HasOne("Shared.Models.Database.AccountCharacterModel", null)
                        .WithMany("PedHeadOverlayColor")
                        .HasForeignKey("CharacterId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Shared.Models.Database.AccountCharacterPedHeadOverlayModel", b =>
                {
                    b.HasOne("Shared.Models.Database.AccountCharacterModel", null)
                        .WithMany("PedHeadOverlay")
                        .HasForeignKey("CharacterId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Shared.Models.Database.AccountCharacterPedPropModel", b =>
                {
                    b.HasOne("Shared.Models.Database.AccountCharacterModel", null)
                        .WithMany("PedProp")
                        .HasForeignKey("CharacterId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Shared.Models.Database.AccountCharacterPositionModel", b =>
                {
                    b.HasOne("Shared.Models.Database.AccountCharacterModel", null)
                        .WithOne("Position")
                        .HasForeignKey("Shared.Models.Database.AccountCharacterPositionModel", "CharacterId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Shared.Models.Database.AccountCharacterRotationModel", b =>
                {
                    b.HasOne("Shared.Models.Database.AccountCharacterModel", null)
                        .WithOne("Rotation")
                        .HasForeignKey("Shared.Models.Database.AccountCharacterRotationModel", "CharacterId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Shared.Models.Database.VehicleModModel", b =>
                {
                    b.HasOne("Shared.Models.Database.VehicleModel", null)
                        .WithMany("Mods")
                        .HasForeignKey("VehicleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
