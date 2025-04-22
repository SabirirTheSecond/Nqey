using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Nqey.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddedProfilePictureToProvider : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Clients_ProfileImage_ProfilePictureProfileImageId",
                table: "Clients");

            migrationBuilder.DropIndex(
                name: "IX_Clients_ProfilePictureProfileImageId",
                table: "Clients");

            migrationBuilder.DropColumn(
                name: "ProfilePictureProfileImageId",
                table: "Clients");

            migrationBuilder.AddColumn<int>(
                name: "ProfilePictureimageId",
                table: "Providers",
                type: "int",
                nullable: true);

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
                name: "IX_Providers_ProfilePictureimageId",
                table: "Providers",
                column: "ProfilePictureimageId");

            migrationBuilder.CreateIndex(
                name: "IX_ProfileImage_UserId",
                table: "ProfileImage",
                column: "UserId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ProfileImage_Clients_UserId",
                table: "ProfileImage",
                column: "UserId",
                principalTable: "Clients",
                principalColumn: "ClientId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Providers_Image_ProfilePictureimageId",
                table: "Providers",
                column: "ProfilePictureimageId",
                principalTable: "Image",
                principalColumn: "imageId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProfileImage_Clients_UserId",
                table: "ProfileImage");

            migrationBuilder.DropForeignKey(
                name: "FK_Providers_Image_ProfilePictureimageId",
                table: "Providers");

            migrationBuilder.DropTable(
                name: "Image");

            migrationBuilder.DropIndex(
                name: "IX_Providers_ProfilePictureimageId",
                table: "Providers");

            migrationBuilder.DropIndex(
                name: "IX_ProfileImage_UserId",
                table: "ProfileImage");

            migrationBuilder.DropColumn(
                name: "ProfilePictureimageId",
                table: "Providers");

            migrationBuilder.AddColumn<int>(
                name: "ProfilePictureProfileImageId",
                table: "Clients",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Clients_ProfilePictureProfileImageId",
                table: "Clients",
                column: "ProfilePictureProfileImageId");

            migrationBuilder.AddForeignKey(
                name: "FK_Clients_ProfileImage_ProfilePictureProfileImageId",
                table: "Clients",
                column: "ProfilePictureProfileImageId",
                principalTable: "ProfileImage",
                principalColumn: "ProfileImageId");
        }
    }
}
