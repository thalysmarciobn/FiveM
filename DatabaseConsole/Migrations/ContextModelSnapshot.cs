﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DatabaseConsole.Migrations
{
    [DbContext(typeof(Context))]
    partial class ContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.32")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("Shared.Models.Database.AccountCharacterModel", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    b.Property<long>("AccountId")
                        .HasColumnType("bigint");

                    b.Property<int>("Armor")
                        .HasColumnType("int");

                    b.Property<DateTime>("DateCreated")
                        .HasColumnType("datetime(6)");

                    b.Property<int>("Gender")
                        .HasColumnType("int");

                    b.Property<float>("Heading")
                        .HasColumnType("float");

                    b.Property<string>("Model")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<int>("Money")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<int>("Slot")
                        .HasColumnType("int");

                    b.Property<string>("Surname")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.HasKey("Id");

                    b.HasIndex("AccountId");

                    b.HasIndex("Id", "AccountId");

                    b.ToTable("account_character");
                });

            modelBuilder.Entity("Shared.Models.Database.AccountCharacterPedComponentModel", b =>
                {
                    b.Property<long>("CharacterId")
                        .HasColumnType("bigint");

                    b.Property<int>("ComponentId")
                        .HasColumnType("int");

                    b.Property<int>("Index")
                        .HasColumnType("int");

                    b.Property<int>("Texture")
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

                    b.Property<int>("EyeColorId")
                        .HasColumnType("int");

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

                    b.Property<int>("ColorType")
                        .HasColumnType("int");

                    b.Property<int>("ColortId")
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

                    b.Property<int>("Index")
                        .HasColumnType("int");

                    b.Property<int>("Texture")
                        .HasColumnType("int");

                    b.HasKey("CharacterId", "PropId");

                    b.ToTable("account_character_ped_prop");
                });

            modelBuilder.Entity("Shared.Models.Database.AccountCharacterPositionModel", b =>
                {
                    b.Property<long>("ChatacterId")
                        .HasColumnType("bigint");

                    b.Property<float>("X")
                        .HasColumnType("float");

                    b.Property<float>("Y")
                        .HasColumnType("float");

                    b.Property<float>("Z")
                        .HasColumnType("float");

                    b.HasKey("ChatacterId");

                    b.ToTable("account_character_position");
                });

            modelBuilder.Entity("Shared.Models.Database.AccountCharacterRotationModel", b =>
                {
                    b.Property<long>("ChatacterId")
                        .HasColumnType("bigint");

                    b.Property<float>("X")
                        .HasColumnType("float");

                    b.Property<float>("Y")
                        .HasColumnType("float");

                    b.Property<float>("Z")
                        .HasColumnType("float");

                    b.HasKey("ChatacterId");

                    b.ToTable("account_character_rotation");
                });

            modelBuilder.Entity("Shared.Models.Database.AccountModel", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    b.Property<DateTime>("Created")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("License")
                        .HasColumnType("varchar(255) CHARACTER SET utf8mb4");

                    b.Property<bool>("WhiteListed")
                        .HasColumnType("tinyint(1)")
                        .HasMaxLength(1);

                    b.HasKey("Id");

                    b.HasIndex("Id", "License");

                    b.ToTable("account");
                });

            modelBuilder.Entity("Shared.Models.Database.ServerVehicleService", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    b.Property<uint>("Driver")
                        .HasColumnType("int unsigned");

                    b.Property<uint>("Model")
                        .HasColumnType("int unsigned");

                    b.Property<float>("X")
                        .HasColumnType("float");

                    b.Property<float>("Y")
                        .HasColumnType("float");

                    b.Property<float>("Z")
                        .HasColumnType("float");

                    b.HasKey("Id");

                    b.ToTable("server_vehicle_service");

                    b.HasData(
                        new
                        {
                            Id = 1L,
                            Driver = 1302784073u,
                            Model = 3338918751u,
                            X = -1049.649f,
                            Y = -2719.027f,
                            Z = 13.7566f
                        },
                        new
                        {
                            Id = 2L,
                            Driver = 1302784073u,
                            Model = 3338918751u,
                            X = -1041.9746f,
                            Y = -2721.6182f,
                            Z = 13.7566f
                        },
                        new
                        {
                            Id = 3L,
                            Driver = 1302784073u,
                            Model = 3338918751u,
                            X = -1026.4174f,
                            Y = -2730.4631f,
                            Z = 13.7566f
                        },
                        new
                        {
                            Id = 4L,
                            Driver = 1302784073u,
                            Model = 3338918751u,
                            X = -1014.7446f,
                            Y = -2737.0579f,
                            Z = 13.7566f
                        });
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
                        .HasForeignKey("Shared.Models.Database.AccountCharacterPositionModel", "ChatacterId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Shared.Models.Database.AccountCharacterRotationModel", b =>
                {
                    b.HasOne("Shared.Models.Database.AccountCharacterModel", null)
                        .WithOne("Rotation")
                        .HasForeignKey("Shared.Models.Database.AccountCharacterRotationModel", "ChatacterId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
