using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RocketShop.Migration.Migrations.AuditLog
{
    /// <inheritdoc />
    public partial class Add_Log_Division : Microsoft.EntityFrameworkCore.Migrations.Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Division",
                table: "EventLog",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Division",
                table: "EventLog");
        }
    }
}
