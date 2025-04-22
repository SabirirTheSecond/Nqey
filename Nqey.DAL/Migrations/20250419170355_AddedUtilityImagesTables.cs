using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Nqey.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddedUtilityImagesTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ProfilePictureProfileImageId",
                table: "Clients",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "PortfolioImage",
                columns: table => new
                {
                    PortfolioImageId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ImagePath = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProviderId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PortfolioImage", x => x.PortfolioImageId);
                    table.ForeignKey(
                        name: "FK_PortfolioImage_Providers_ProviderId",
                        column: x => x.ProviderId,
                        principalTable: "Providers",
                        principalColumn: "ProviderId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProfileImage",
                columns: table => new
                {
                    ProfileImageId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ImagePath = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProfileImage", x => x.ProfileImageId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Clients_ProfilePictureProfileImageId",
                table: "Clients",
                column: "ProfilePictureProfileImageId");

            migrationBuilder.CreateIndex(
                name: "IX_PortfolioImage_ProviderId",
                table: "PortfolioImage",
                column: "ProviderId");

            migrationBuilder.AddForeignKey(
                name: "FK_Clients_ProfileImage_ProfilePictureProfileImageId",
                table: "Clients",
                column: "ProfilePictureProfileImageId",
                principalTable: "ProfileImage",
                principalColumn: "ProfileImageId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Clients_ProfileImage_ProfilePictureProfileImageId",
                table: "Clients");

            migrationBuilder.DropTable(
                name: "PortfolioImage");

            migrationBuilder.DropTable(
                name: "ProfileImage");

            migrationBuilder.DropIndex(
                name: "IX_Clients_ProfilePictureProfileImageId",
                table: "Clients");

            migrationBuilder.DropColumn(
                name: "ProfilePictureProfileImageId",
                table: "Clients");
        }
    }
}
