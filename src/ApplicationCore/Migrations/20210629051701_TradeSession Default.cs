using Microsoft.EntityFrameworkCore.Migrations;

namespace ApplicationCore.Migrations
{
    public partial class TradeSessionDefault : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "TradeSessions");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "TradeSessions");

            migrationBuilder.AddColumn<bool>(
                name: "Default",
                table: "TradeSessions",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Default",
                table: "TradeSessions");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "TradeSessions",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "TradeSessions",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
