using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Nqey.DAL.Migrations
{
    /// <inheritdoc />
    public partial class ChangedImageToServiceModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Services_ProfileImage_ImageId",
                table: "Services");

            migrationBuilder.DropIndex(
                name: "IX_Services_ImageId",
                table: "Services");

            migrationBuilder.RenameColumn(
                name: "ImageId",
                table: "Services",
                newName: "ServiceImageId");

            migrationBuilder.CreateTable(
                name: "Image",
                columns: table => new
                {
                    imageId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ImagePath = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Image", x => x.imageId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Services_ServiceImageId",
                table: "Services",
                column: "ServiceImageId",
                unique: true,
                filter: "[ServiceImageId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Services_Image_ServiceImageId",
                table: "Services",
                column: "ServiceImageId",
                principalTable: "Image",
                principalColumn: "imageId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Services_Image_ServiceImageId",
                table: "Services");

            migrationBuilder.DropTable(
                name: "Image");

            migrationBuilder.DropIndex(
                name: "IX_Services_ServiceImageId",
                table: "Services");

            migrationBuilder.RenameColumn(
                name: "ServiceImageId",
                table: "Services",
                newName: "ImageId");

            migrationBuilder.CreateIndex(
                name: "IX_Services_ImageId",
                table: "Services",
                column: "ImageId",
                unique: true,
                filter: "[ImageId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Services_ProfileImage_ImageId",
                table: "Services",
                column: "ImageId",
                principalTable: "ProfileImage",
                principalColumn: "ProfileImageId");
        }
    }
}
