using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace docuverwalter_api.Migrations
{
    /// <inheritdoc />
    public partial class remigrate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "DocumentType",
                table: "Documents",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<DateTime>(
                name: "LastEditedTime",
                table: "Documents",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastEditedTime",
                table: "Documents");

            migrationBuilder.AlterColumn<int>(
                name: "DocumentType",
                table: "Documents",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }
    }
}
