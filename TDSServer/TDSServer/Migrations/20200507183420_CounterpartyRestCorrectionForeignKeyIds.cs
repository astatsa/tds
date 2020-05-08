using Microsoft.EntityFrameworkCore.Migrations;

namespace TDSServer.Migrations
{
    public partial class CounterpartyRestCorrectionForeignKeyIds : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CounterpartyRestCorrections_Users_UserId",
                table: "CounterpartyRestCorrections");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "CounterpartyRestCorrections",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_CounterpartyRestCorrections_Users_UserId",
                table: "CounterpartyRestCorrections",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CounterpartyRestCorrections_Users_UserId",
                table: "CounterpartyRestCorrections");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "CounterpartyRestCorrections",
                type: "int",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddForeignKey(
                name: "FK_CounterpartyRestCorrections_Users_UserId",
                table: "CounterpartyRestCorrections",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
