using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SolarLab.AdvertBoard.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Change_DeleteBehaviour_In_Comments_Table : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Adverts_AdvertId",
                table: "Comments");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Adverts_AdvertId",
                table: "Comments",
                column: "AdvertId",
                principalTable: "Adverts",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Adverts_AdvertId",
                table: "Comments");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Adverts_AdvertId",
                table: "Comments",
                column: "AdvertId",
                principalTable: "Adverts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
