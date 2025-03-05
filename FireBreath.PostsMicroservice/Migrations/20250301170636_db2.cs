using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FireBreath.PostsMicroservice.Migrations
{
    /// <inheritdoc />
    public partial class db2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Timestamp",
                table: "Posts",
                newName: "Created");

            migrationBuilder.AddColumn<string>(
                name: "AuthorTag",
                table: "Posts",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AuthorTag",
                table: "Posts");

            migrationBuilder.RenameColumn(
                name: "Created",
                table: "Posts",
                newName: "Timestamp");
        }
    }
}
