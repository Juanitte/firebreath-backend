using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FireBreath.UsersMicroservice.Migrations
{
    /// <inheritdoc />
    public partial class db2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(name: "Posts");
            migrationBuilder.DropTable(name: "Attachments");
            migrationBuilder.DropTable(name: "Likes");
            migrationBuilder.DropTable(name: "Shares");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
