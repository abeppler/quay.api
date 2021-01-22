using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Wms.ProductionLine.Infra.Migrations
{
    public partial class AddConfigurationHistory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ProductionLineConfigurationHistory",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    UserId = table.Column<Guid>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    ProductionLineConfigurationId = table.Column<Guid>(nullable: false),
                    HistoryType = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductionLineConfigurationHistory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductionLineConfigurationHistory_ProductionLineConfiguration_ProductionLineConfigurationId",
                        column: x => x.ProductionLineConfigurationId,
                        principalTable: "ProductionLineConfiguration",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProductionLineConfigurationHistory_ProductionLineConfigurationId",
                table: "ProductionLineConfigurationHistory",
                column: "ProductionLineConfigurationId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductionLineConfigurationHistory");
        }
    }
}
