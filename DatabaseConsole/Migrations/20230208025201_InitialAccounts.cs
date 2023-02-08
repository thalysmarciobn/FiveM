using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DatabaseConsole.Migrations
{
    public partial class InitialAccounts : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "accounts",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    License = table.Column<string>(nullable: true),
                    Created = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    WhiteListed = table.Column<bool>(maxLength: 1, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_accounts", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_accounts_License",
                table: "accounts",
                column: "License",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_accounts_Id_License",
                table: "accounts",
                columns: new[] { "Id", "License" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "accounts");
        }
    }
}
