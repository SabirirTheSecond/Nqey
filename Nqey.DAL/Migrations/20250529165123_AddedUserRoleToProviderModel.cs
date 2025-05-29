using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Nqey.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddedUserRoleToProviderModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UserRole",
                table: "Providers",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserRole",
                table: "Providers");
        }
    }
}
