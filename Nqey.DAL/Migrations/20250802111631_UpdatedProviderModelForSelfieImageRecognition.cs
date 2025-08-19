using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Nqey.DAL.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedProviderModelForSelfieImageRecognition : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IdentityPiece",
                table: "Providers");

            migrationBuilder.AddColumn<int>(
                name: "IdentityId",
                table: "Providers",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SelfieId",
                table: "Providers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Providers_IdentityId",
                table: "Providers",
                column: "IdentityId",
                unique: true,
                filter: "[IdentityId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Providers_SelfieId",
                table: "Providers",
                column: "SelfieId",
                unique: true,
                filter: "[SelfieId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Providers_Image_IdentityId",
                table: "Providers",
                column: "IdentityId",
                principalTable: "Image",
                principalColumn: "imageId");

            migrationBuilder.AddForeignKey(
                name: "FK_Providers_Image_SelfieId",
                table: "Providers",
                column: "SelfieId",
                principalTable: "Image",
                principalColumn: "imageId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Providers_Image_IdentityId",
                table: "Providers");

            migrationBuilder.DropForeignKey(
                name: "FK_Providers_Image_SelfieId",
                table: "Providers");

            migrationBuilder.DropIndex(
                name: "IX_Providers_IdentityId",
                table: "Providers");

            migrationBuilder.DropIndex(
                name: "IX_Providers_SelfieId",
                table: "Providers");

            migrationBuilder.DropColumn(
                name: "IdentityId",
                table: "Providers");

            migrationBuilder.DropColumn(
                name: "SelfieId",
                table: "Providers");

            migrationBuilder.AddColumn<string>(
                name: "IdentityPiece",
                table: "Providers",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
