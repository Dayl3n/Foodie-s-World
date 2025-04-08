using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FoodiesWorld.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialSetup2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Opinion",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "AspNetUsers",
                type: "nvarchar(13)",
                maxLength: 13,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Opinion_UserId",
                table: "Opinion",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Opinion_AspNetUsers_UserId",
                table: "Opinion",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Opinion_AspNetUsers_UserId",
                table: "Opinion");

            migrationBuilder.DropIndex(
                name: "IX_Opinion_UserId",
                table: "Opinion");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Opinion");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "AspNetUsers");
        }
    }
}
