using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TDSServer.Migrations
{
    public partial class CounterpartyMaterialMvtAndCorrectionDoc : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CounterpartyRestCorrections",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    IsDeleted = table.Column<bool>(nullable: false),
                    Date = table.Column<DateTime>(nullable: false),
                    CounterpartyId = table.Column<int>(nullable: false),
                    UserId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CounterpartyRestCorrections", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CounterpartyRestCorrections_Counterparties_CounterpartyId",
                        column: x => x.CounterpartyId,
                        principalTable: "Counterparties",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CounterpartyRestCorrections_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CounterpartyMaterialMvts",
                columns: table => new
                {
                    CounterpartyId = table.Column<int>(nullable: false),
                    MaterialId = table.Column<int>(nullable: false),
                    RegistratorTypeId = table.Column<int>(nullable: false),
                    RegistratorId = table.Column<int>(nullable: false),
                    Date = table.Column<DateTime>(nullable: false),
                    IsComing = table.Column<bool>(nullable: false),
                    Quantity = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CounterpartyMaterialMvts", x => new { x.CounterpartyId, x.MaterialId });
                    table.ForeignKey(
                        name: "FK_CounterpartyMaterialMvts_Counterparties_CounterpartyId",
                        column: x => x.CounterpartyId,
                        principalTable: "Counterparties",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CounterpartyMaterialMvts_Materials_MaterialId",
                        column: x => x.MaterialId,
                        principalTable: "Materials",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CounterpartyMaterialMvts_CounterpartyRestCorrections_Registr~",
                        column: x => x.RegistratorId,
                        principalTable: "CounterpartyRestCorrections",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CounterpartyMaterialMvts_Orders_RegistratorId",
                        column: x => x.RegistratorId,
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CounterpartyRestCorrectionMaterials",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    MaterialId = table.Column<int>(nullable: false),
                    Correction = table.Column<double>(nullable: false),
                    CounterpartyRestCorrectionId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CounterpartyRestCorrectionMaterials", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CounterpartyRestCorrectionMaterials_CounterpartyRestCorrecti~",
                        column: x => x.CounterpartyRestCorrectionId,
                        principalTable: "CounterpartyRestCorrections",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CounterpartyRestCorrectionMaterials_Materials_MaterialId",
                        column: x => x.MaterialId,
                        principalTable: "Materials",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CounterpartyMaterialMvts_MaterialId",
                table: "CounterpartyMaterialMvts",
                column: "MaterialId");

            migrationBuilder.CreateIndex(
                name: "IX_CounterpartyMaterialMvts_RegistratorId",
                table: "CounterpartyMaterialMvts",
                column: "RegistratorId");

            migrationBuilder.CreateIndex(
                name: "IX_CounterpartyRestCorrectionMaterials_CounterpartyRestCorrecti~",
                table: "CounterpartyRestCorrectionMaterials",
                column: "CounterpartyRestCorrectionId");

            migrationBuilder.CreateIndex(
                name: "IX_CounterpartyRestCorrectionMaterials_MaterialId",
                table: "CounterpartyRestCorrectionMaterials",
                column: "MaterialId");

            migrationBuilder.CreateIndex(
                name: "IX_CounterpartyRestCorrections_CounterpartyId",
                table: "CounterpartyRestCorrections",
                column: "CounterpartyId");

            migrationBuilder.CreateIndex(
                name: "IX_CounterpartyRestCorrections_UserId",
                table: "CounterpartyRestCorrections",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CounterpartyMaterialMvts");

            migrationBuilder.DropTable(
                name: "CounterpartyRestCorrectionMaterials");

            migrationBuilder.DropTable(
                name: "CounterpartyRestCorrections");
        }
    }
}
