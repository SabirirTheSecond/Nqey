using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Nqey.DAL.Migrations
{
    /// <inheritdoc />
    public partial class DeletedJobDescriptionIdFromReservation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reservations_JobDescriptions_JobDescriptionId",
                table: "Reservations");

            migrationBuilder.DropIndex(
                name: "IX_Reservations_JobDescriptionId",
                table: "Reservations");

            migrationBuilder.DropColumn(
                name: "JobDescriptionId",
                table: "Reservations");

            migrationBuilder.CreateIndex(
                name: "IX_JobDescriptions_ReservationId",
                table: "JobDescriptions",
                column: "ReservationId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_JobDescriptions_Reservations_ReservationId",
                table: "JobDescriptions",
                column: "ReservationId",
                principalTable: "Reservations",
                principalColumn: "ReservationId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_JobDescriptions_Reservations_ReservationId",
                table: "JobDescriptions");

            migrationBuilder.DropIndex(
                name: "IX_JobDescriptions_ReservationId",
                table: "JobDescriptions");

            migrationBuilder.AddColumn<int>(
                name: "JobDescriptionId",
                table: "Reservations",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Reservations_JobDescriptionId",
                table: "Reservations",
                column: "JobDescriptionId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Reservations_JobDescriptions_JobDescriptionId",
                table: "Reservations",
                column: "JobDescriptionId",
                principalTable: "JobDescriptions",
                principalColumn: "JobDescriptionId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
