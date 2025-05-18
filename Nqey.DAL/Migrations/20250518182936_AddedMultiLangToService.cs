using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Nqey.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddedMultiLangToService : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Services",
                newName: "NameFr");

            migrationBuilder.AddColumn<string>(
                name: "NameAr",
                table: "Services",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "NameEn",
                table: "Services",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NameAr",
                table: "Services");

            migrationBuilder.DropColumn(
                name: "NameEn",
                table: "Services");

            migrationBuilder.RenameColumn(
                name: "NameFr",
                table: "Services",
                newName: "Name");
        }
    }
}
