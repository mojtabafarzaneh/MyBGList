using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyBGListApi.Migrations
{
    /// <inheritdoc />
    public partial class withbuildoption : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UserRated",
                table: "BoardGames",
                newName: "UsersRated");

            migrationBuilder.RenameColumn(
                name: "OwnedUser",
                table: "BoardGames",
                newName: "OwnedUsers");

            migrationBuilder.AddColumn<int>(
                name: "BGGRank",
                table: "BoardGames",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "ComplexityAverage",
                table: "BoardGames",
                type: "decimal(4,2)",
                precision: 4,
                scale: 2,
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BGGRank",
                table: "BoardGames");

            migrationBuilder.DropColumn(
                name: "ComplexityAverage",
                table: "BoardGames");

            migrationBuilder.RenameColumn(
                name: "UsersRated",
                table: "BoardGames",
                newName: "UserRated");

            migrationBuilder.RenameColumn(
                name: "OwnedUsers",
                table: "BoardGames",
                newName: "OwnedUser");
        }
    }
}
