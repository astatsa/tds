using Microsoft.EntityFrameworkCore.Migrations;

namespace TDSServer.Migrations
{
    public partial class RolePermissionsForDispatcherAndDriver : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Refuels_Employees_DriverId",
                table: "Refuels");

            migrationBuilder.DropForeignKey(
                name: "FK_Refuels_GasStations_GasStationId",
                table: "Refuels");

            migrationBuilder.AlterColumn<int>(
                name: "GasStationId",
                table: "Refuels",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "DriverId",
                table: "Refuels",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.InsertData(
                table: "RolePermissions",
                columns: new[] { "PermissionId", "RoleId" },
                values: new object[] { 1, 2 });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "Description", "FullName", "IsDeleted", "Name" },
                values: new object[] { 3, null, "Диспетчер", false, "Dispatcher" });

            migrationBuilder.InsertData(
                table: "RolePermissions",
                columns: new[] { "PermissionId", "RoleId" },
                values: new object[,]
                {
                    { 2, 3 },
                    { 3, 3 },
                    { 4, 3 },
                    { 5, 3 },
                    { 6, 3 }
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Refuels_Employees_DriverId",
                table: "Refuels",
                column: "DriverId",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Refuels_GasStations_GasStationId",
                table: "Refuels",
                column: "GasStationId",
                principalTable: "GasStations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Refuels_Employees_DriverId",
                table: "Refuels");

            migrationBuilder.DropForeignKey(
                name: "FK_Refuels_GasStations_GasStationId",
                table: "Refuels");

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { 1, 2 });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { 2, 3 });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { 3, 3 });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { 4, 3 });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { 5, 3 });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { 6, 3 });

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.AlterColumn<int>(
                name: "GasStationId",
                table: "Refuels",
                type: "int",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<int>(
                name: "DriverId",
                table: "Refuels",
                type: "int",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddForeignKey(
                name: "FK_Refuels_Employees_DriverId",
                table: "Refuels",
                column: "DriverId",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Refuels_GasStations_GasStationId",
                table: "Refuels",
                column: "GasStationId",
                principalTable: "GasStations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
