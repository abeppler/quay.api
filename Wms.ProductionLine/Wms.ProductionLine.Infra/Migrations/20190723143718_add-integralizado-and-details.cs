using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Wms.ProductionLine.Infra.Migrations
{
    [ExcludeFromCodeCoverage]
    public partial class addintegralizadoanddetails : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Integralizado",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ItemId = table.Column<Guid>(nullable: false),
                    Enabled = table.Column<bool>(nullable: false),
                    WarehouseId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Integralizado", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Integralizado_Item_ItemId",
                        column: x => x.ItemId,
                        principalTable: "Item",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "IntegralizadoDetail",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ItemId = table.Column<Guid>(nullable: false),
                    Quantity = table.Column<double>(nullable: false),
                    IntegralizadoId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IntegralizadoDetail", x => x.Id);
                    table.ForeignKey(
                        name: "FK_IntegralizadoDetail_Integralizado_IntegralizadoId",
                        column: x => x.IntegralizadoId,
                        principalTable: "Integralizado",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_IntegralizadoDetail_Item_ItemId",
                        column: x => x.ItemId,
                        principalTable: "Item",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Integralizado_ItemId",
                table: "Integralizado",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_IntegralizadoDetail_IntegralizadoId",
                table: "IntegralizadoDetail",
                column: "IntegralizadoId");

            migrationBuilder.CreateIndex(
                name: "IX_IntegralizadoDetail_ItemId",
                table: "IntegralizadoDetail",
                column: "ItemId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "IntegralizadoDetail");

            migrationBuilder.DropTable(
                name: "Integralizado");
        }
    }
}
