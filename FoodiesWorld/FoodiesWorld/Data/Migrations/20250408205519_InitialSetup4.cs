﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FoodiesWorld.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialSetup4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

        }
    }
}
