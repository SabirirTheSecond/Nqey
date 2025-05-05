using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Nqey.DAL.Migrations
{
    /// <inheritdoc />
    public partial class CreateSubServicesTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Providers_Review_ReviewId",
                table: "Providers");

            migrationBuilder.DropIndex(
                name: "IX_Providers_ReviewId",
                table: "Providers");

            migrationBuilder.AddColumn<int>(
                name: "ProviderId",
                table: "Review",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "SubService",
                columns: table => new
                {
                    SubServiceId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Price = table.Column<double>(type: "float", nullable: false),
                    Unity = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProviderId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubService", x => x.SubServiceId);
                    table.ForeignKey(
                        name: "FK_SubService_Providers_ProviderId",
                        column: x => x.ProviderId,
                        principalTable: "Providers",
                        principalColumn: "ProviderId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Review_ProviderId",
                table: "Review",
                column: "ProviderId");

            migrationBuilder.CreateIndex(
                name: "IX_SubService_ProviderId",
                table: "SubService",
                column: "ProviderId");

            migrationBuilder.AddForeignKey(
                name: "FK_Review_Providers_ProviderId",
                table: "Review",
                column: "ProviderId",
                principalTable: "Providers",
                principalColumn: "ProviderId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Review_Providers_ProviderId",
                table: "Review");

            migrationBuilder.DropTable(
                name: "SubService");

            migrationBuilder.DropIndex(
                name: "IX_Review_ProviderId",
                table: "Review");

            migrationBuilder.DropColumn(
                name: "ProviderId",
                table: "Review");

            migrationBuilder.CreateIndex(
                name: "IX_Providers_ReviewId",
                table: "Providers",
                column: "ReviewId");

            migrationBuilder.AddForeignKey(
                name: "FK_Providers_Review_ReviewId",
                table: "Providers",
                column: "ReviewId",
                principalTable: "Review",
                principalColumn: "ReviewId");
        }
    }
}
