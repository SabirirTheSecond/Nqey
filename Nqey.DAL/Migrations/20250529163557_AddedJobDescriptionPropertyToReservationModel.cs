using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Nqey.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddedJobDescriptionPropertyToReservationModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_JobDescriptions_ReservationId",
                table: "JobDescriptions");

            migrationBuilder.DropColumn(
                name: "JobDescription",
                table: "Reservations");

            migrationBuilder.CreateIndex(
                name: "IX_JobDescriptions_ReservationId",
                table: "JobDescriptions",
                column: "ReservationId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_JobDescriptions_ReservationId",
                table: "JobDescriptions");

            migrationBuilder.AddColumn<string>(
                name: "JobDescription",
                table: "Reservations",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_JobDescriptions_ReservationId",
                table: "JobDescriptions",
                column: "ReservationId");
        }
    }
}
