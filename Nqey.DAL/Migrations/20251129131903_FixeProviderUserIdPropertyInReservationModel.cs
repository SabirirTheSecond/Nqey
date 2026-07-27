using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Nqey.DAL.Migrations
{
    /// <inheritdoc />
    public partial class FixeProviderUserIdPropertyInReservationModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reservations_Clients_ClientUserId1",
                table: "Reservations");

            migrationBuilder.DropForeignKey(
                name: "FK_Reservations_Providers_ProviderUserId1",
                table: "Reservations");

            migrationBuilder.DropIndex(
                name: "IX_Reservations_ClientUserId1",
                table: "Reservations");

            migrationBuilder.DropIndex(
                name: "IX_Reservations_ProviderUserId1",
                table: "Reservations");

            migrationBuilder.DropColumn(
                name: "ClientUserId1",
                table: "Reservations");

            migrationBuilder.DropColumn(
                name: "ProviderUserId1",
                table: "Reservations");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ClientUserId1",
                table: "Reservations",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ProviderUserId1",
                table: "Reservations",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Reservations_ClientUserId1",
                table: "Reservations",
                column: "ClientUserId1");

            migrationBuilder.CreateIndex(
                name: "IX_Reservations_ProviderUserId1",
                table: "Reservations",
                column: "ProviderUserId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Reservations_Clients_ClientUserId1",
                table: "Reservations",
                column: "ClientUserId1",
                principalTable: "Clients",
                principalColumn: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Reservations_Providers_ProviderUserId1",
                table: "Reservations",
                column: "ProviderUserId1",
                principalTable: "Providers",
                principalColumn: "UserId");
        }
    }
}
