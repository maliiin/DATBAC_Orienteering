using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace orienteering_backend.Migrations
{
    public partial class namechangeTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Checkpoint_Track_TrackId",
                table: "Checkpoint");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Track",
                table: "Track");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Checkpoint",
                table: "Checkpoint");

            migrationBuilder.RenameTable(
                name: "Track",
                newName: "Tracks");

            migrationBuilder.RenameTable(
                name: "Checkpoint",
                newName: "Checkpoints");

            migrationBuilder.RenameIndex(
                name: "IX_Checkpoint_TrackId",
                table: "Checkpoints",
                newName: "IX_Checkpoints_TrackId");

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "Tracks",
                type: "char(36)",
                nullable: false,
                collation: "ascii_general_ci",
                oldClrType: typeof(int),
                oldType: "int")
                .OldAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "Tracks",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<Guid>(
                name: "TrackId",
                table: "Checkpoints",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci",
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Tracks",
                table: "Tracks",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Checkpoints",
                table: "Checkpoints",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Checkpoints_Tracks_TrackId",
                table: "Checkpoints",
                column: "TrackId",
                principalTable: "Tracks",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Checkpoints_Tracks_TrackId",
                table: "Checkpoints");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Tracks",
                table: "Tracks");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Checkpoints",
                table: "Checkpoints");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Tracks");

            migrationBuilder.RenameTable(
                name: "Tracks",
                newName: "Track");

            migrationBuilder.RenameTable(
                name: "Checkpoints",
                newName: "Checkpoint");

            migrationBuilder.RenameIndex(
                name: "IX_Checkpoints_TrackId",
                table: "Checkpoint",
                newName: "IX_Checkpoint_TrackId");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "Track",
                type: "int",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "char(36)")
                .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn)
                .OldAnnotation("Relational:Collation", "ascii_general_ci");

            migrationBuilder.AlterColumn<int>(
                name: "TrackId",
                table: "Checkpoint",
                type: "int",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldNullable: true)
                .OldAnnotation("Relational:Collation", "ascii_general_ci");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Track",
                table: "Track",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Checkpoint",
                table: "Checkpoint",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Checkpoint_Track_TrackId",
                table: "Checkpoint",
                column: "TrackId",
                principalTable: "Track",
                principalColumn: "Id");
        }
    }
}
