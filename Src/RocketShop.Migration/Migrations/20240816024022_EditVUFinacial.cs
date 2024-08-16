using Microsoft.EntityFrameworkCore.Migrations;
using RocketShop.Database;

#nullable disable

namespace RocketShop.Migration.Migrations
{
    /// <inheritdoc />
    public partial class EditVUFinacial : Microsoft.EntityFrameworkCore.Migrations.Migration
    {
        readonly CreateViewSql createView = CreateView.UserFinacialViewV2;
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql($"Drop View \"{TableConstraint.UserFinacialView}\"");
            migrationBuilder.Sql(createView.Up);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql($"Drop View \"{TableConstraint.UserFinacialView}\"");
            migrationBuilder.Sql(createView.Down);
        }
    }
}
