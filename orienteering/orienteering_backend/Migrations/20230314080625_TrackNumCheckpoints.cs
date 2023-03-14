using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace orienteeringbackend.Migrations
{
    /// <inheritdoc />
    public partial class TrackNumCheckpoints : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "NumCheckpoints",
                table: "Tracks",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NumCheckpoints",
                table: "Tracks");
        }
    }
}
