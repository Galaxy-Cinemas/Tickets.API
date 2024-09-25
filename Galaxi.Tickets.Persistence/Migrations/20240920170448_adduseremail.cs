using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Galaxi.Tickets.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class adduseremail : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserEmail",
                schema: "DBO",
                table: "Ticket",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserEmail",
                schema: "DBO",
                table: "Ticket");
        }
    }
}
