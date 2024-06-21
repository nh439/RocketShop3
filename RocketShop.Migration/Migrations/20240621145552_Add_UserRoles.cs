using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace RocketShop.Migration.Migrations
{
    /// <inheritdoc />
    public partial class Add_UserRoles : Microsoft.EntityFrameworkCore.Migrations.Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    RoleName = table.Column<string>(type: "text", nullable: false),
                    CreateFixtureRequest = table.Column<bool>(type: "boolean", nullable: false),
                    EndFixtureRequest = table.Column<bool>(type: "boolean", nullable: false),
                    UpdateFixture = table.Column<bool>(type: "boolean", nullable: false),
                    ViewAnotherUserFixtureData = table.Column<bool>(type: "boolean", nullable: false),
                    CreateProduct = table.Column<bool>(type: "boolean", nullable: false),
                    UpdateProduct = table.Column<bool>(type: "boolean", nullable: false),
                    DeleteProduct = table.Column<bool>(type: "boolean", nullable: false),
                    Sell = table.Column<bool>(type: "boolean", nullable: false),
                    SellSpeicalProduct = table.Column<bool>(type: "boolean", nullable: false),
                    SellerProductManagement = table.Column<bool>(type: "boolean", nullable: false),
                    ViewAnotherSalesValues = table.Column<bool>(type: "boolean", nullable: false),
                    ViewSaleData = table.Column<bool>(type: "boolean", nullable: false),
                    CreateEmployee = table.Column<bool>(type: "boolean", nullable: false),
                    UpdateEmployee = table.Column<bool>(type: "boolean", nullable: false),
                    SetResign = table.Column<bool>(type: "boolean", nullable: false),
                    ViewEmployeeDeepData = table.Column<bool>(type: "boolean", nullable: false),
                    ApplicationAdmin = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "text", nullable: false),
                    RoleId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRoles", x => new { x.RoleId, x.UserId });
                });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "ApplicationAdmin", "CreateEmployee", "CreateFixtureRequest", "CreateProduct", "DeleteProduct", "EndFixtureRequest", "RoleName", "Sell", "SellSpeicalProduct", "SellerProductManagement", "SetResign", "UpdateEmployee", "UpdateFixture", "UpdateProduct", "ViewAnotherSalesValues", "ViewAnotherUserFixtureData", "ViewEmployeeDeepData", "ViewSaleData" },
                values: new object[] { 1, true, true, true, true, true, true, "Application Starter", true, true, true, true, true, true, true, true, true, true, true });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "UserRoles");
        }
    }
}
