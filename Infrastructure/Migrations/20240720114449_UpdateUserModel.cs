using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateUserModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Films_Images_ImageId",
                table: "Films");

            migrationBuilder.DropIndex(
                name: "IX_Films_ImageId",
                table: "Films");

            migrationBuilder.AddColumn<Guid>(
                name: "FilmId",
                table: "Images",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Images_FilmId",
                table: "Images",
                column: "FilmId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Images_Films_FilmId",
                table: "Images",
                column: "FilmId",
                principalTable: "Films",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Images_Films_FilmId",
                table: "Images");

            migrationBuilder.DropIndex(
                name: "IX_Images_FilmId",
                table: "Images");

            migrationBuilder.DropColumn(
                name: "FilmId",
                table: "Images");

            migrationBuilder.CreateIndex(
                name: "IX_Films_ImageId",
                table: "Films",
                column: "ImageId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Films_Images_ImageId",
                table: "Films",
                column: "ImageId",
                principalTable: "Images",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
