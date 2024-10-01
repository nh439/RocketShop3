using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RocketShop.Migration.Migrations.AuditLog
{
    /// <inheritdoc />
    public partial class AddActorName : Microsoft.EntityFrameworkCore.Migrations.Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ActorName",
                table: "EventLog",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_EventLog_ActorName",
                table: "EventLog",
                column: "ActorName");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_EventLog_ActorName",
                table: "EventLog");

            migrationBuilder.DropColumn(
                name: "ActorName",
                table: "EventLog");
        }
    }
}
