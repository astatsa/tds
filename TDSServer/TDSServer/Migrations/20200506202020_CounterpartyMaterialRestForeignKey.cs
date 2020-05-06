using Microsoft.EntityFrameworkCore.Migrations;

namespace TDSServer.Migrations
{
    public partial class CounterpartyMaterialRestForeignKey : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_CounterpartyMaterialRests_MaterialId",
                table: "CounterpartyMaterialRests",
                column: "MaterialId");

            migrationBuilder.AddForeignKey(
                name: "FK_CounterpartyMaterialRests_Counterparties_CounterpartyId",
                table: "CounterpartyMaterialRests",
                column: "CounterpartyId",
                principalTable: "Counterparties",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CounterpartyMaterialRests_Materials_MaterialId",
                table: "CounterpartyMaterialRests",
                column: "MaterialId",
                principalTable: "Materials",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CounterpartyMaterialRests_Counterparties_CounterpartyId",
                table: "CounterpartyMaterialRests");

            migrationBuilder.DropForeignKey(
                name: "FK_CounterpartyMaterialRests_Materials_MaterialId",
                table: "CounterpartyMaterialRests");

            migrationBuilder.DropIndex(
                name: "IX_CounterpartyMaterialRests_MaterialId",
                table: "CounterpartyMaterialRests");
        }
    }
}
