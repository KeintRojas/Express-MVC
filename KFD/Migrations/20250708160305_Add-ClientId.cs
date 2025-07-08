using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KFD.Migrations
{
    /// <inheritdoc />
    public partial class AddClientId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "clientID",
                table: "orders",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "clientID",
                table: "orders");
        }
    }
}
