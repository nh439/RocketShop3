using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RocketShop.Migration.Migrations.Warehouse
{
    /// <inheritdoc />
    public partial class AddClientHistories : Microsoft.EntityFrameworkCore.Migrations.Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Authorization_Client_History",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    Key = table.Column<string>(type: "text", nullable: false),
                    ClientId = table.Column<long>(type: "bigint", nullable: false),
                    Date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IpAddress = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Authorization_Client_History", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Authorization_Client_History_ClientId",
                table: "Authorization_Client_History",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_Authorization_Client_History_Key",
                table: "Authorization_Client_History",
                column: "Key");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Authorization_Client_History");
        }
    }
}
