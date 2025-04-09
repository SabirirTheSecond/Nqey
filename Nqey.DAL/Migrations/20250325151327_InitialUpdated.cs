using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Nqey.DAL.Migrations
{
    /// <inheritdoc />
    public partial class InitialUpdated : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ServiceDesrciption",
                table: "Providers",
                newName: "ServiceDescription");

            migrationBuilder.AlterColumn<string>(
                name: "PasswordHash",
                table: "Providers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "Message",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SenderId = table.Column<int>(type: "int", nullable: false),
                    RecieverId = table.Column<int>(type: "int", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TimeStamp = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsRead = table.Column<bool>(type: "bit", nullable: false),
                    ClientId = table.Column<int>(type: "int", nullable: true),
                    ClientId1 = table.Column<int>(type: "int", nullable: true),
                    ProviderId = table.Column<int>(type: "int", nullable: true),
                    ProviderId1 = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Message", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Message_Clients_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Clients",
                        principalColumn: "ClientId");
                    table.ForeignKey(
                        name: "FK_Message_Clients_ClientId1",
                        column: x => x.ClientId1,
                        principalTable: "Clients",
                        principalColumn: "ClientId");
                    table.ForeignKey(
                        name: "FK_Message_Providers_ProviderId",
                        column: x => x.ProviderId,
                        principalTable: "Providers",
                        principalColumn: "ProviderId");
                    table.ForeignKey(
                        name: "FK_Message_Providers_ProviderId1",
                        column: x => x.ProviderId1,
                        principalTable: "Providers",
                        principalColumn: "ProviderId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Message_ClientId",
                table: "Message",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_Message_ClientId1",
                table: "Message",
                column: "ClientId1");

            migrationBuilder.CreateIndex(
                name: "IX_Message_ProviderId",
                table: "Message",
                column: "ProviderId");

            migrationBuilder.CreateIndex(
                name: "IX_Message_ProviderId1",
                table: "Message",
                column: "ProviderId1");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Message");

            migrationBuilder.RenameColumn(
                name: "ServiceDescription",
                table: "Providers",
                newName: "ServiceDesrciption");

            migrationBuilder.AlterColumn<string>(
                name: "PasswordHash",
                table: "Providers",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }
    }
}
