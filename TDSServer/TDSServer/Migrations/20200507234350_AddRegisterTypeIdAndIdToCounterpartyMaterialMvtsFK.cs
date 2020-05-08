using Microsoft.EntityFrameworkCore.Migrations;

namespace TDSServer.Migrations
{
    public partial class AddRegisterTypeIdAndIdToCounterpartyMaterialMvtsPK : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"ALTER TABLE `tds`.`counterpartymaterialmvts` 
                                   DROP PRIMARY KEY,
                                   ADD PRIMARY KEY (`CounterpartyId`, `MaterialId`, `RegistratorTypeId`, `RegistratorId`);");

            /*migrationBuilder.DropPrimaryKey(
                name: "PK_CounterpartyMaterialMvts",
                table: "CounterpartyMaterialMvts");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CounterpartyMaterialMvts",
                table: "CounterpartyMaterialMvts",
                columns: new[] { "RegistratorTypeId", "RegistratorId", "CounterpartyId", "MaterialId" });*/

            migrationBuilder.CreateIndex(
                name: "IX_CounterpartyMaterialMvts_CounterpartyId",
                table: "CounterpartyMaterialMvts",
                column: "CounterpartyId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropPrimaryKey(
            //    name: "PK_CounterpartyMaterialMvts",
            //    table: "CounterpartyMaterialMvts");
            migrationBuilder.Sql(@"ALTER TABLE `tds`.`counterpartymaterialmvts` 
                                   DROP PRIMARY KEY,
                                   ADD PRIMARY KEY (`CounterpartyId`, `MaterialId`);");

            migrationBuilder.DropIndex(
                name: "IX_CounterpartyMaterialMvts_CounterpartyId",
                table: "CounterpartyMaterialMvts");

            //migrationBuilder.AddPrimaryKey(
            //    name: "PK_CounterpartyMaterialMvts",
            //    table: "CounterpartyMaterialMvts",
            //    columns: new[] { "CounterpartyId", "MaterialId" });
        }
    }
}
