﻿using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DatabaseConsole.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "account",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    License = table.Column<string>(nullable: true),
                    CurrentCharacter = table.Column<long>(nullable: false),
                    LastAddress = table.Column<string>(nullable: true),
                    Created = table.Column<DateTime>(nullable: false),
                    WhiteListed = table.Column<bool>(maxLength: 1, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_account", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "server_vehicle_service",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Model = table.Column<uint>(nullable: false),
                    Driver = table.Column<uint>(nullable: false),
                    X = table.Column<float>(nullable: false),
                    Y = table.Column<float>(nullable: false),
                    Z = table.Column<float>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_server_vehicle_service", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "account_character",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    AccountId = table.Column<long>(nullable: false),
                    Slot = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Surname = table.Column<string>(nullable: true),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    Model = table.Column<string>(nullable: true),
                    Heading = table.Column<float>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_account_character", x => x.Id);
                    table.ForeignKey(
                        name: "FK_account_character_account_AccountId",
                        column: x => x.AccountId,
                        principalTable: "account",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "account_character_ped_component",
                columns: table => new
                {
                    CharacterId = table.Column<long>(nullable: false),
                    ComponentId = table.Column<int>(nullable: false),
                    Index = table.Column<int>(nullable: false),
                    Texture = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_account_character_ped_component", x => new { x.CharacterId, x.ComponentId });
                    table.ForeignKey(
                        name: "FK_account_character_ped_component_account_character_CharacterId",
                        column: x => x.CharacterId,
                        principalTable: "account_character",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "account_character_ped_face",
                columns: table => new
                {
                    CharacterId = table.Column<long>(nullable: false),
                    Index = table.Column<int>(nullable: false),
                    Scale = table.Column<float>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_account_character_ped_face", x => new { x.CharacterId, x.Index });
                    table.ForeignKey(
                        name: "FK_account_character_ped_face_account_character_CharacterId",
                        column: x => x.CharacterId,
                        principalTable: "account_character",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "account_character_ped_head",
                columns: table => new
                {
                    CharacterId = table.Column<long>(nullable: false),
                    EyeColorId = table.Column<int>(nullable: false),
                    HairColorId = table.Column<int>(nullable: false),
                    HairHighlightColor = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_account_character_ped_head", x => x.CharacterId);
                    table.ForeignKey(
                        name: "FK_account_character_ped_head_account_character_CharacterId",
                        column: x => x.CharacterId,
                        principalTable: "account_character",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "account_character_ped_head_data",
                columns: table => new
                {
                    CharacterId = table.Column<long>(nullable: false),
                    ShapeFirstID = table.Column<int>(nullable: false),
                    ShapeSecondID = table.Column<int>(nullable: false),
                    ShapeThirdID = table.Column<int>(nullable: false),
                    SkinFirstID = table.Column<int>(nullable: false),
                    SkinSecondID = table.Column<int>(nullable: false),
                    SkinThirdID = table.Column<int>(nullable: false),
                    ShapeMix = table.Column<float>(nullable: false),
                    SkinMix = table.Column<float>(nullable: false),
                    ThirdMix = table.Column<float>(nullable: false),
                    IsParent = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_account_character_ped_head_data", x => x.CharacterId);
                    table.ForeignKey(
                        name: "FK_account_character_ped_head_data_account_character_CharacterId",
                        column: x => x.CharacterId,
                        principalTable: "account_character",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "account_character_ped_head_overlay",
                columns: table => new
                {
                    CharacterId = table.Column<long>(nullable: false),
                    OverlayId = table.Column<int>(nullable: false),
                    Index = table.Column<int>(nullable: false),
                    Opacity = table.Column<float>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_account_character_ped_head_overlay", x => new { x.CharacterId, x.OverlayId });
                    table.ForeignKey(
                        name: "FK_account_character_ped_head_overlay_account_character_Charact~",
                        column: x => x.CharacterId,
                        principalTable: "account_character",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "account_character_ped_head_overlay_color",
                columns: table => new
                {
                    CharacterId = table.Column<long>(nullable: false),
                    OverlayId = table.Column<int>(nullable: false),
                    ColorType = table.Column<int>(nullable: false),
                    ColortId = table.Column<int>(nullable: false),
                    SecondColorId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_account_character_ped_head_overlay_color", x => new { x.CharacterId, x.OverlayId });
                    table.ForeignKey(
                        name: "FK_account_character_ped_head_overlay_color_account_character_C~",
                        column: x => x.CharacterId,
                        principalTable: "account_character",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "account_character_ped_prop",
                columns: table => new
                {
                    CharacterId = table.Column<long>(nullable: false),
                    PropId = table.Column<int>(nullable: false),
                    Index = table.Column<int>(nullable: false),
                    Texture = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_account_character_ped_prop", x => new { x.CharacterId, x.PropId });
                    table.ForeignKey(
                        name: "FK_account_character_ped_prop_account_character_CharacterId",
                        column: x => x.CharacterId,
                        principalTable: "account_character",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "account_character_position",
                columns: table => new
                {
                    ChatacterId = table.Column<long>(nullable: false),
                    X = table.Column<float>(nullable: false),
                    Y = table.Column<float>(nullable: false),
                    Z = table.Column<float>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_account_character_position", x => x.ChatacterId);
                    table.ForeignKey(
                        name: "FK_account_character_position_account_character_ChatacterId",
                        column: x => x.ChatacterId,
                        principalTable: "account_character",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "account_character_rotation",
                columns: table => new
                {
                    ChatacterId = table.Column<long>(nullable: false),
                    X = table.Column<float>(nullable: false),
                    Y = table.Column<float>(nullable: false),
                    Z = table.Column<float>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_account_character_rotation", x => x.ChatacterId);
                    table.ForeignKey(
                        name: "FK_account_character_rotation_account_character_ChatacterId",
                        column: x => x.ChatacterId,
                        principalTable: "account_character",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "server_vehicle_service",
                columns: new[] { "Id", "Driver", "Model", "X", "Y", "Z" },
                values: new object[,]
                {
                    { 1L, 1302784073u, 3338918751u, -1049.649f, -2719.027f, 13.7566f },
                    { 2L, 1302784073u, 3338918751u, -1041.9746f, -2721.6182f, 13.7566f },
                    { 3L, 1302784073u, 3338918751u, -1026.4174f, -2730.4631f, 13.7566f },
                    { 4L, 1302784073u, 3338918751u, -1014.7446f, -2737.0579f, 13.7566f }
                });

            migrationBuilder.CreateIndex(
                name: "IX_account_Id_License",
                table: "account",
                columns: new[] { "Id", "License" });

            migrationBuilder.CreateIndex(
                name: "IX_account_character_AccountId",
                table: "account_character",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_account_character_Id_AccountId",
                table: "account_character",
                columns: new[] { "Id", "AccountId" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "account_character_ped_component");

            migrationBuilder.DropTable(
                name: "account_character_ped_face");

            migrationBuilder.DropTable(
                name: "account_character_ped_head");

            migrationBuilder.DropTable(
                name: "account_character_ped_head_data");

            migrationBuilder.DropTable(
                name: "account_character_ped_head_overlay");

            migrationBuilder.DropTable(
                name: "account_character_ped_head_overlay_color");

            migrationBuilder.DropTable(
                name: "account_character_ped_prop");

            migrationBuilder.DropTable(
                name: "account_character_position");

            migrationBuilder.DropTable(
                name: "account_character_rotation");

            migrationBuilder.DropTable(
                name: "server_vehicle_service");

            migrationBuilder.DropTable(
                name: "account_character");

            migrationBuilder.DropTable(
                name: "account");
        }
    }
}
