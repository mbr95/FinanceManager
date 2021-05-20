using Microsoft.EntityFrameworkCore.Migrations;

namespace FinanceManager.API.Migrations
{
    public partial class DeleteRefreshTokenInvalidatedParam : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Invalidated",
                table: "RefreshTokens");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Invalidated",
                table: "RefreshTokens",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
