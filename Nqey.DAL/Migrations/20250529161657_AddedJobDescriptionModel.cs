using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Nqey.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddedJobDescriptionModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "JobDescriptionId",
                table: "Image",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "JobDescriptions",
                columns: table => new
                {
                    JobDescriptionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ReservationId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobDescriptions", x => x.JobDescriptionId);
                    table.ForeignKey(
                        name: "FK_JobDescriptions_Reservations_ReservationId",
                        column: x => x.ReservationId,
                        principalTable: "Reservations",
                        principalColumn: "ReservationId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Image_JobDescriptionId",
                table: "Image",
                column: "JobDescriptionId");

            migrationBuilder.CreateIndex(
                name: "IX_JobDescriptions_ReservationId",
                table: "JobDescriptions",
                column: "ReservationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Image_JobDescriptions_JobDescriptionId",
                table: "Image",
                column: "JobDescriptionId",
                principalTable: "JobDescriptions",
                principalColumn: "JobDescriptionId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Image_JobDescriptions_JobDescriptionId",
                table: "Image");

            migrationBuilder.DropTable(
                name: "JobDescriptions");

            migrationBuilder.DropIndex(
                name: "IX_Image_JobDescriptionId",
                table: "Image");

            migrationBuilder.DropColumn(
                name: "JobDescriptionId",
                table: "Image");
        }
    }
}
