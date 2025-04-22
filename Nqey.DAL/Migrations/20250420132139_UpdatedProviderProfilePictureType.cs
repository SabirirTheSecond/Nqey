using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Nqey.DAL.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedProviderProfilePictureType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Providers_Image_ProfilePictureimageId",
                table: "Providers");

            migrationBuilder.DropTable(
                name: "Image");

            migrationBuilder.RenameColumn(
                name: "ProfilePictureimageId",
                table: "Providers",
                newName: "ProfilePictureProfileImageId");

            migrationBuilder.RenameIndex(
                name: "IX_Providers_ProfilePictureimageId",
                table: "Providers",
                newName: "IX_Providers_ProfilePictureProfileImageId");

            migrationBuilder.AddForeignKey(
                name: "FK_Providers_ProfileImage_ProfilePictureProfileImageId",
                table: "Providers",
                column: "ProfilePictureProfileImageId",
                principalTable: "ProfileImage",
                principalColumn: "ProfileImageId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Providers_ProfileImage_ProfilePictureProfileImageId",
                table: "Providers");

            migrationBuilder.RenameColumn(
                name: "ProfilePictureProfileImageId",
                table: "Providers",
                newName: "ProfilePictureimageId");

            migrationBuilder.RenameIndex(
                name: "IX_Providers_ProfilePictureProfileImageId",
                table: "Providers",
                newName: "IX_Providers_ProfilePictureimageId");

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

            migrationBuilder.AddForeignKey(
                name: "FK_Providers_Image_ProfilePictureimageId",
                table: "Providers",
                column: "ProfilePictureimageId",
                principalTable: "Image",
                principalColumn: "imageId");
        }
    }
}
