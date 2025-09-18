using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SocialOffice.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddInterestTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Interests",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Interests",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "Interests",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "Interests",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Interests",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "Interests",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "Interests",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Interests");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Interests");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "Interests");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Interests");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "Interests");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "Interests");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Interests",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");
        }
    }
}
