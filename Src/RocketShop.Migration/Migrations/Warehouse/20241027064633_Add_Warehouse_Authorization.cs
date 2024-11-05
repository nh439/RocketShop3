using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace RocketShop.Migration.Migrations.Warehouse
{
    /// <inheritdoc />
    public partial class Add_Warehouse_Authorization : Microsoft.EntityFrameworkCore.Migrations.Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Authorization_Allowed_Object",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    Client = table.Column<long>(type: "bigint", nullable: false),
                    ObjectName = table.Column<string>(type: "text", nullable: false),
                    Read = table.Column<bool>(type: "boolean", nullable: false),
                    Write = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Authorization_Allowed_Object", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Authorization_Client",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ClientId = table.Column<string>(type: "text", nullable: false),
                    ClientName = table.Column<string>(type: "text", nullable: false),
                    TokenExpiration = table.Column<int>(type: "integer", nullable: true),
                    Created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LockUntil = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Updated = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreateBy = table.Column<string>(type: "text", nullable: true),
                    UpdateBy = table.Column<string>(type: "text", nullable: true),
                    Application = table.Column<string>(type: "text", nullable: true),
                    MaxinumnAccess = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Authorization_Client", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Authorization_Client_Secret",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    Client = table.Column<long>(type: "bigint", nullable: false),
                    SecretValue = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    Salt = table.Column<string>(type: "text", nullable: false),
                    Created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Expired = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Authorization_Client_Secret", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Authorization_Token",
                columns: table => new
                {
                    TokenKey = table.Column<string>(type: "text", nullable: false),
                    Client = table.Column<long>(type: "bigint", nullable: false),
                    RemainingAccess = table.Column<int>(type: "integer", nullable: true),
                    IssueDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    TokenAge = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Authorization_Token", x => x.TokenKey);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Authorization_Allowed_Object_Client_ObjectName",
                table: "Authorization_Allowed_Object",
                columns: new[] { "Client", "ObjectName" });

            migrationBuilder.CreateIndex(
                name: "IX_Authorization_Client_Secret_Client",
                table: "Authorization_Client_Secret",
                column: "Client");

            migrationBuilder.CreateIndex(
                name: "IX_Authorization_Token_Client",
                table: "Authorization_Token",
                column: "Client");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Authorization_Allowed_Object");

            migrationBuilder.DropTable(
                name: "Authorization_Client");

            migrationBuilder.DropTable(
                name: "Authorization_Client_Secret");

            migrationBuilder.DropTable(
                name: "Authorization_Token");
        }
    }
}
