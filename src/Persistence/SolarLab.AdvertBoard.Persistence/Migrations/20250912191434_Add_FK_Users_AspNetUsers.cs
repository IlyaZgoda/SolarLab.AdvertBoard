using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SolarLab.AdvertBoard.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Add_FK_Users_AspNetUsers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Users_IdentityId",
                table: "Users");

            migrationBuilder.CreateIndex(
                name: "IX_Users_IdentityId",
                table: "Users",
                column: "IdentityId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_AspNetUsers_IdentityId",
                table: "Users",
                column: "IdentityId",
                principalTable: "AspNetUsers",
                principalColumn: "IdentityUserId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_AspNetUsers_IdentityId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_IdentityId",
                table: "Users");

            migrationBuilder.CreateIndex(
                name: "IX_Users_IdentityId",
                table: "Users",
                column: "IdentityId");
        }
    }
}
