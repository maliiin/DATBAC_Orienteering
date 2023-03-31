using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace orienteeringbackend.Migrations
{
    /// <inheritdoc />
    public partial class AddOrderToCheckpoint : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Order",
                table: "Checkpoints",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Order",
                table: "Checkpoints");
        }
    }
}
