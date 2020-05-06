using Microsoft.EntityFrameworkCore.Migrations;

namespace TDSServer.Migrations
{
    public partial class RemoveCounterpartyMaterialMvtForeignKeys : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CounterpartyMaterialMvts_CounterpartyRestCorrections_Registr~",
                table: "CounterpartyMaterialMvts");

            migrationBuilder.DropForeignKey(
                name: "FK_CounterpartyMaterialMvts_Orders_RegistratorId",
                table: "CounterpartyMaterialMvts");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddForeignKey(
                name: "FK_CounterpartyMaterialMvts_CounterpartyRestCorrections_Registr~",
                table: "CounterpartyMaterialMvts",
                column: "RegistratorId",
                principalTable: "CounterpartyRestCorrections",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CounterpartyMaterialMvts_Orders_RegistratorId",
                table: "CounterpartyMaterialMvts",
                column: "RegistratorId",
                principalTable: "Orders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
