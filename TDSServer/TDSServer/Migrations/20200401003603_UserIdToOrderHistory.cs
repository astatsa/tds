using Microsoft.EntityFrameworkCore.Migrations;

namespace TDSServer.Migrations
{
    public partial class UserIdToOrderHistory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderStateHistories_Users_UserId",
                table: "OrderStateHistories");

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Users",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Roles",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Refuels",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Positions",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Permissions",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "OrderStates",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "OrderStateHistories",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "OrderStateHistories",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Orders",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Measures",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Materials",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "GasStations",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Employees",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "CounterpartyTypes",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Counterparties",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderStateHistories_Users_UserId",
                table: "OrderStateHistories",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderStateHistories_Users_UserId",
                table: "OrderStateHistories");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Refuels");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Positions");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Permissions");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "OrderStates");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "OrderStateHistories");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Measures");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Materials");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "GasStations");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "CounterpartyTypes");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Counterparties");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "OrderStateHistories",
                type: "int",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddForeignKey(
                name: "FK_OrderStateHistories_Users_UserId",
                table: "OrderStateHistories",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
