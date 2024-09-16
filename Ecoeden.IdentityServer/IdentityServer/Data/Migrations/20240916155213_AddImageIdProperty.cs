using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IdentityServer.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddImageIdProperty : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImageId",
                table: "AspNetUsers",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageId",
                table: "AspNetUsers");
        }
    }
}
