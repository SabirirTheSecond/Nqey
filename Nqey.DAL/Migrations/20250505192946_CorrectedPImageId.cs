using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Nqey.DAL.Migrations
{
    /// <inheritdoc />
    public partial class CorrectedPImageId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Clients_ProfileImage_ProfileImageId",
                table: "Clients");

            migrationBuilder.DropForeignKey(
                name: "FK_Providers_ProfileImage_ProfileImageId",
                table: "Providers");

            migrationBuilder.DropIndex(
                name: "IX_Providers_ProfileImageId",
                table: "Providers");

            migrationBuilder.DropIndex(
                name: "IX_Clients_ProfileImageId",
                table: "Clients");

            migrationBuilder.RenameColumn(
                name: "ProfileImageId",
                table: "Providers",
                newName: "PImageId");

            migrationBuilder.RenameColumn(
                name: "ProfileImageId",
                table: "Clients",
                newName: "PImageId");

            migrationBuilder.CreateIndex(
                name: "IX_Providers_PImageId",
                table: "Providers",
                column: "PImageId",
                unique: true,
                filter: "[PImageId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Clients_PImageId",
                table: "Clients",
                column: "PImageId",
                unique: true,
                filter: "[PImageId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Clients_ProfileImage_PImageId",
                table: "Clients",
                column: "PImageId",
                principalTable: "ProfileImage",
                principalColumn: "ProfileImageId");

            migrationBuilder.AddForeignKey(
                name: "FK_Providers_ProfileImage_PImageId",
                table: "Providers",
                column: "PImageId",
                principalTable: "ProfileImage",
                principalColumn: "ProfileImageId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Clients_ProfileImage_PImageId",
                table: "Clients");

            migrationBuilder.DropForeignKey(
                name: "FK_Providers_ProfileImage_PImageId",
                table: "Providers");

            migrationBuilder.DropIndex(
                name: "IX_Providers_PImageId",
                table: "Providers");

            migrationBuilder.DropIndex(
                name: "IX_Clients_PImageId",
                table: "Clients");

            migrationBuilder.RenameColumn(
                name: "PImageId",
                table: "Providers",
                newName: "ProfileImageId");

            migrationBuilder.RenameColumn(
                name: "PImageId",
                table: "Clients",
                newName: "ProfileImageId");

            migrationBuilder.CreateIndex(
                name: "IX_Providers_ProfileImageId",
                table: "Providers",
                column: "ProfileImageId",
                unique: true,
                filter: "[ProfileImageId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Clients_ProfileImageId",
                table: "Clients",
                column: "ProfileImageId",
                unique: true,
                filter: "[ProfileImageId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Clients_ProfileImage_ProfileImageId",
                table: "Clients",
                column: "ProfileImageId",
                principalTable: "ProfileImage",
                principalColumn: "ProfileImageId");

            migrationBuilder.AddForeignKey(
                name: "FK_Providers_ProfileImage_ProfileImageId",
                table: "Providers",
                column: "ProfileImageId",
                principalTable: "ProfileImage",
                principalColumn: "ProfileImageId");
        }
    }
}
