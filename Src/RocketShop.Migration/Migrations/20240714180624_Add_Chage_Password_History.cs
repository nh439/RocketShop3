using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RocketShop.Migration.Migrations
{
    /// <inheritdoc />
    public partial class Add_Chage_Password_History : Microsoft.EntityFrameworkCore.Migrations.Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ChangePasswordHistories",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    UserId = table.Column<string>(type: "text", nullable: false),
                    ChangeAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Reset = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChangePasswordHistories", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ChangePasswordHistories_Reset",
                table: "ChangePasswordHistories",
                column: "Reset");

            migrationBuilder.CreateIndex(
                name: "IX_ChangePasswordHistories_UserId",
                table: "ChangePasswordHistories",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ChangePasswordHistories");
        }
    }
}
