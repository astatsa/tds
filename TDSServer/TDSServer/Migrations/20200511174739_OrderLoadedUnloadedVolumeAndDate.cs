using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TDSServer.Migrations
{
    public partial class OrderLoadedUnloadedVolumeAndDate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "LoadedDate",
                table: "Orders",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "LoadedVolume",
                table: "Orders",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<DateTime>(
                name: "UnloadedDate",
                table: "Orders",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "UnloadedVolume",
                table: "Orders",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LoadedDate",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "LoadedVolume",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "UnloadedDate",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "UnloadedVolume",
                table: "Orders");
        }
    }
}
