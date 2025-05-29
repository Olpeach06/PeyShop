using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PeyShop.Migrations
{
    /// <inheritdoc />
    public partial class _1q2w : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Zakazy_Users_UsersId",
                table: "Zakazy");

            migrationBuilder.RenameColumn(
                name: "UsersId",
                table: "Zakazy",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Zakazy_UsersId",
                table: "Zakazy",
                newName: "IX_Zakazy_UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Zakazy_Users_UserId",
                table: "Zakazy",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UsersId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Zakazy_Users_UserId",
                table: "Zakazy");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Zakazy",
                newName: "UsersId");

            migrationBuilder.RenameIndex(
                name: "IX_Zakazy_UserId",
                table: "Zakazy",
                newName: "IX_Zakazy_UsersId");

            migrationBuilder.AddForeignKey(
                name: "FK_Zakazy_Users_UsersId",
                table: "Zakazy",
                column: "UsersId",
                principalTable: "Users",
                principalColumn: "UsersId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
