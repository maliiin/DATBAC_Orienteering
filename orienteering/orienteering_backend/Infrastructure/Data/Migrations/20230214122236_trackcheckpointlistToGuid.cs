using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace orienteeringbackend.Migrations
{
    /// <inheritdoc />
    public partial class trackcheckpointlistToGuid : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Checkpoints_Tracks_TrackId",
                table: "Checkpoints");

            migrationBuilder.DropIndex(
                name: "IX_Checkpoints_TrackId",
                table: "Checkpoints");

            migrationBuilder.AlterColumn<Guid>(
                name: "TrackId",
                table: "Checkpoints",
                type: "char(36)",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldNullable: true)
                .OldAnnotation("Relational:Collation", "ascii_general_ci");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<Guid>(
                name: "TrackId",
                table: "Checkpoints",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)")
                .OldAnnotation("Relational:Collation", "ascii_general_ci");

            migrationBuilder.CreateIndex(
                name: "IX_Checkpoints_TrackId",
                table: "Checkpoints",
                column: "TrackId");

            migrationBuilder.AddForeignKey(
                name: "FK_Checkpoints_Tracks_TrackId",
                table: "Checkpoints",
                column: "TrackId",
                principalTable: "Tracks",
                principalColumn: "Id");
        }
    }
}
