using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RocketShop.Migration.Migrations
{
    /// <inheritdoc />
    public partial class Add_UserPayroll_Cancelled : Microsoft.EntityFrameworkCore.Migrations.Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Cancelled",
                table: "Payroll",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_Payroll_Cancelled",
                table: "Payroll",
                column: "Cancelled");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Payroll_Cancelled",
                table: "Payroll");

            migrationBuilder.DropColumn(
                name: "Cancelled",
                table: "Payroll");
        }
    }
}
