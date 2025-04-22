using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Nqey.DAL.Migrations
{
    /// <inheritdoc />
    public partial class FixedProfileImageRelations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProfileImage_Clients_UserId",
                table: "ProfileImage");

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

            migrationBuilder.AddForeignKey(
                name: "FK_ProfileImage_Users_UserId",
                table: "ProfileImage",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Clients_ProfileImage_ProfilePictureProfileImageId",
                table: "Clients");

            migrationBuilder.DropForeignKey(
                name: "FK_ProfileImage_Users_UserId",
                table: "ProfileImage");

            migrationBuilder.DropIndex(
                name: "IX_Clients_ProfilePictureProfileImageId",
                table: "Clients");

            migrationBuilder.DropColumn(
                name: "ProfilePictureProfileImageId",
                table: "Clients");

            migrationBuilder.AddForeignKey(
                name: "FK_ProfileImage_Clients_UserId",
                table: "ProfileImage",
                column: "UserId",
                principalTable: "Clients",
                principalColumn: "ClientId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
