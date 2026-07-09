using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fixly.Migrations
{
    /// <inheritdoc />
    public partial class IncomingRequests : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Service",
                table: "ServiceRequests",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Service",
                table: "ServiceRequests");
        }
    }
}
