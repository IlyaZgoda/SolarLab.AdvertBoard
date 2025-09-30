using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SolarLab.AdvertBoard.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Add_Contact_Email_And_Remove_FK_Identity_Id : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_AspNetUsers_IdentityId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_IdentityId",
                table: "Users");

            migrationBuilder.AddColumn<string>(
                name: "ContactEmail",
                table: "Users",
                type: "character varying(320)",
                maxLength: 320,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Users_IdentityId",
                table: "Users",
                column: "IdentityId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Users_IdentityId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "ContactEmail",
                table: "Users");

            migrationBuilder.RenameColumn(
                name: "Email",
                table: "AspNetUsers",
                newName: "ContactEmail");

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
    }
}
