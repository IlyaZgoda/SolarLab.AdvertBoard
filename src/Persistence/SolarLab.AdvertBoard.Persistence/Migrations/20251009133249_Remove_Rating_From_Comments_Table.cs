using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SolarLab.AdvertBoard.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Remove_Rating_From_Comments_Table : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Rating",
                table: "Comments");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Rating",
                table: "Comments",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }
    }
}
