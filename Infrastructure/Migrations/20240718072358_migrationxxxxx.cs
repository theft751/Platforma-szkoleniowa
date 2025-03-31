using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class migrationxxxxx : Migration
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

            migrationBuilder.AlterColumn<Guid>(
                name: "ImageId",
                table: "Films",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Films_Images_ImageId",
                table: "Films");

            migrationBuilder.DropIndex(
                name: "IX_Films_ImageId",
                table: "Films");

            migrationBuilder.AlterColumn<Guid>(
                name: "ImageId",
                table: "Films",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.CreateIndex(
                name: "IX_Films_ImageId",
                table: "Films",
                column: "ImageId",
                unique: true,
                filter: "[ImageId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Films_Images_ImageId",
                table: "Films",
                column: "ImageId",
                principalTable: "Images",
                principalColumn: "Id");
        }
    }
}
