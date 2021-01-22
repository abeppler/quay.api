using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Wms.ProductionLine.Infra.Migrations
{
    [ExcludeFromCodeCoverage]
    public partial class initialmigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Address",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    WarehouseId = table.Column<Guid>(nullable: false),
                    Code = table.Column<string>(unicode: false, maxLength: 30, nullable: true),
                    Description = table.Column<string>(unicode: false, maxLength: 250, nullable: true),
                    Enabled = table.Column<bool>(nullable: false),
                    Sequence = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Address", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    WarehouseId = table.Column<Guid>(nullable: true),
                    Name = table.Column<string>(unicode: false, maxLength: 100, nullable: false),
                    Login = table.Column<string>(unicode: false, maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Address");

            migrationBuilder.DropTable(
                name: "User");
        }
    }
}
