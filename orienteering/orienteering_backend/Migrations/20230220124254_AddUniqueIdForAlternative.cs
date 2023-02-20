using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace orienteeringbackend.Migrations
{
    /// <inheritdoc />
    public partial class AddUniqueIdForAlternative : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Alternative",
                table: "Alternative");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "Alternative",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .OldAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AddColumn<Guid>(
                name: "UniqueId",
                table: "Alternative",
                type: "char(36)",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                collation: "ascii_general_ci");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Alternative",
                table: "Alternative",
                column: "UniqueId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Alternative",
                table: "Alternative");

            migrationBuilder.DropColumn(
                name: "UniqueId",
                table: "Alternative");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "Alternative",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Alternative",
                table: "Alternative",
                column: "Id");
        }
    }
}
