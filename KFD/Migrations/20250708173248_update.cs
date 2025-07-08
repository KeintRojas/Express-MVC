using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KFD.Migrations
{
    /// <inheritdoc />
    public partial class update : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "clientID",
                table: "orders");

            migrationBuilder.AddColumn<string>(
                name: "CustomerId",
                table: "orders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CustomerId",
                table: "orders");

            migrationBuilder.AddColumn<int>(
                name: "clientID",
                table: "orders",
                type: "int",
                nullable: true);
        }
    }
}
