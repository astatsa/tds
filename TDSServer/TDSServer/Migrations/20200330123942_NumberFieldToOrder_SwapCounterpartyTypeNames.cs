using Microsoft.EntityFrameworkCore.Migrations;

namespace TDSServer.Migrations
{
    public partial class NumberFieldToOrder_SwapCounterpartyTypeNames : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Number",
                table: "Orders",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "CounterpartyTypes",
                keyColumn: "Id",
                keyValue: 1,
                column: "Name",
                value: "Supplier");

            migrationBuilder.UpdateData(
                table: "CounterpartyTypes",
                keyColumn: "Id",
                keyValue: 2,
                column: "Name",
                value: "Customer");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Number",
                table: "Orders");

            migrationBuilder.UpdateData(
                table: "CounterpartyTypes",
                keyColumn: "Id",
                keyValue: 1,
                column: "Name",
                value: "Customer");

            migrationBuilder.UpdateData(
                table: "CounterpartyTypes",
                keyColumn: "Id",
                keyValue: 2,
                column: "Name",
                value: "Supplier");
        }
    }
}
