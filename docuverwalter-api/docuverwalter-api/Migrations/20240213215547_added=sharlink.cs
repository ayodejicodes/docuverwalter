using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace docuverwalter_api.Migrations
{
    /// <inheritdoc />
    public partial class addedsharlink : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ShareLink",
                table: "DocumentShareLinks");

            migrationBuilder.RenameColumn(
                name: "ValidUntil",
                table: "DocumentShareLinks",
                newName: "ExpiryDateTime");

            migrationBuilder.RenameColumn(
                name: "DocumentShareLinkId",
                table: "DocumentShareLinks",
                newName: "ShareLinkId");

            migrationBuilder.AddColumn<string>(
                name: "GeneratedLink",
                table: "DocumentShareLinks",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "DocumentShareLinks",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GeneratedLink",
                table: "DocumentShareLinks");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "DocumentShareLinks");

            migrationBuilder.RenameColumn(
                name: "ExpiryDateTime",
                table: "DocumentShareLinks",
                newName: "ValidUntil");

            migrationBuilder.RenameColumn(
                name: "ShareLinkId",
                table: "DocumentShareLinks",
                newName: "DocumentShareLinkId");

            migrationBuilder.AddColumn<string>(
                name: "ShareLink",
                table: "DocumentShareLinks",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
