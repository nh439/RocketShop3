using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RocketShop.Migration.Migrations.Warehouse
{
    /// <inheritdoc />
    public partial class Recreate_Token_Table : Microsoft.EntityFrameworkCore.Migrations.Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Authorization_Token",
                columns: table => new
                {
                    TokenKey = table.Column<string>(type: "text", nullable: false),
                    Client = table.Column<long>(type: "bigint", nullable: false),
                    RemainingAccess = table.Column<int>(type: "integer", nullable: true),
                    IssueDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    TokenAge = table.Column<TimeSpan>(type: "interval", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Authorization_Token", x => x.TokenKey);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Authorization_Token_Client",
                table: "Authorization_Token",
                column: "Client");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Authorization_Token");
        }
    }
}
