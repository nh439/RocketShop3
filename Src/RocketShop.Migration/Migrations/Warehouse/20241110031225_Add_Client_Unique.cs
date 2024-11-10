using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RocketShop.Migration.Migrations
{
    /// <inheritdoc />
    public partial class Add_Client_Unique : Microsoft.EntityFrameworkCore.Migrations.Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Authorization_Client_ClientId",
                table: "Authorization_Client",
                column: "ClientId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Authorization_Client_ClientName",
                table: "Authorization_Client",
                column: "ClientName",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Authorization_Client_ClientId",
                table: "Authorization_Client");

            migrationBuilder.DropIndex(
                name: "IX_Authorization_Client_ClientName",
                table: "Authorization_Client");
        }
    }
}
