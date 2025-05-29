using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Nqey.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddedUserRoleToClientModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "UserRole",
                table: "Providers",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "UserRole",
                table: "Clients",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserRole",
                table: "Clients");

            migrationBuilder.AlterColumn<int>(
                name: "UserRole",
                table: "Providers",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }
    }
}
