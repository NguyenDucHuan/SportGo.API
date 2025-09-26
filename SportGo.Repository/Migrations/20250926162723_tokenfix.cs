using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SportGo.Repository.Migrations
{
    /// <inheritdoc />
    public partial class tokenfix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RefreshTokens_Users_UserId",
                table: "RefreshTokens");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RefreshTokens",
                table: "RefreshTokens");

            migrationBuilder.RenameTable(
                name: "RefreshTokens",
                newName: "UserRefreshTokens");

            migrationBuilder.RenameIndex(
                name: "IX_RefreshTokens_UserId",
                table: "UserRefreshTokens",
                newName: "IX_UserRefreshTokens_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_RefreshTokens_Token",
                table: "UserRefreshTokens",
                newName: "IX_UserRefreshTokens_Token");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserRefreshTokens",
                table: "UserRefreshTokens",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserRefreshTokens_Users_UserId",
                table: "UserRefreshTokens",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserRefreshTokens_Users_UserId",
                table: "UserRefreshTokens");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserRefreshTokens",
                table: "UserRefreshTokens");

            migrationBuilder.RenameTable(
                name: "UserRefreshTokens",
                newName: "RefreshTokens");

            migrationBuilder.RenameIndex(
                name: "IX_UserRefreshTokens_UserId",
                table: "RefreshTokens",
                newName: "IX_RefreshTokens_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_UserRefreshTokens_Token",
                table: "RefreshTokens",
                newName: "IX_RefreshTokens_Token");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RefreshTokens",
                table: "RefreshTokens",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RefreshTokens_Users_UserId",
                table: "RefreshTokens",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
