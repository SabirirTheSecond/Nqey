using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Nqey.DAL.Migrations
{
    /// <inheritdoc />
    public partial class ReservationModelChanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ClientPhone",
                table: "Reservations");

            migrationBuilder.DropColumn(
                name: "ProviderPhone",
                table: "Reservations");

            migrationBuilder.AddColumn<DateTime>(
                name: "createdAt",
                table: "Reservations",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "createdAt",
                table: "Reservations");

            migrationBuilder.AddColumn<int>(
                name: "ClientPhone",
                table: "Reservations",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ProviderPhone",
                table: "Reservations",
                type: "int",
                nullable: true);
        }
    }
}
