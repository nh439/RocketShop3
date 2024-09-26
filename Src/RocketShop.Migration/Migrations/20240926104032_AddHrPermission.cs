using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RocketShop.Migration.Migrations
{
    /// <inheritdoc />
    public partial class AddHrPermission : Microsoft.EntityFrameworkCore.Migrations.Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "HRAuditLog",
                table: "Roles",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "HRFinancial",
                table: "Roles",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "HRAuditLog", "HRFinancial" },
                values: new object[] { true, true });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HRAuditLog",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "HRFinancial",
                table: "Roles");
        }
    }
}
