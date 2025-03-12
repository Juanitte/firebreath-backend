using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FireBreath.UsersMicroservice.Migrations
{
    /// <inheritdoc />
    public partial class db3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            
            migrationBuilder.DropTable(name: "Attachments");
            migrationBuilder.DropTable(name: "Likes");
            migrationBuilder.DropTable(name: "Shares");
            migrationBuilder.DropTable(name: "Posts");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
