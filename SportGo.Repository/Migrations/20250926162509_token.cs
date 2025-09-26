using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SportGo.Repository.Migrations
{
    /// <inheritdoc />
    public partial class token : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProviderProfile_Users_UserId",
                table: "ProviderProfile");

            migrationBuilder.DropForeignKey(
                name: "FK_UserRefreshToken_Users_UserId",
                table: "UserRefreshToken");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserRefreshToken",
                table: "UserRefreshToken");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProviderProfile",
                table: "ProviderProfile");

            migrationBuilder.RenameTable(
                name: "UserRefreshToken",
                newName: "RefreshTokens");

            migrationBuilder.RenameTable(
                name: "ProviderProfile",
                newName: "ProviderProfiles");

            migrationBuilder.RenameIndex(
                name: "IX_UserRefreshToken_UserId",
                table: "RefreshTokens",
                newName: "IX_RefreshTokens_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_UserRefreshToken_Token",
                table: "RefreshTokens",
                newName: "IX_RefreshTokens_Token");

            migrationBuilder.RenameIndex(
                name: "IX_ProviderProfile_UserId",
                table: "ProviderProfiles",
                newName: "IX_ProviderProfiles_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RefreshTokens",
                table: "RefreshTokens",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProviderProfiles",
                table: "ProviderProfiles",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProviderProfiles_Users_UserId",
                table: "ProviderProfiles",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RefreshTokens_Users_UserId",
                table: "RefreshTokens",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProviderProfiles_Users_UserId",
                table: "ProviderProfiles");

            migrationBuilder.DropForeignKey(
                name: "FK_RefreshTokens_Users_UserId",
                table: "RefreshTokens");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RefreshTokens",
                table: "RefreshTokens");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProviderProfiles",
                table: "ProviderProfiles");

            migrationBuilder.RenameTable(
                name: "RefreshTokens",
                newName: "UserRefreshToken");

            migrationBuilder.RenameTable(
                name: "ProviderProfiles",
                newName: "ProviderProfile");

            migrationBuilder.RenameIndex(
                name: "IX_RefreshTokens_UserId",
                table: "UserRefreshToken",
                newName: "IX_UserRefreshToken_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_RefreshTokens_Token",
                table: "UserRefreshToken",
                newName: "IX_UserRefreshToken_Token");

            migrationBuilder.RenameIndex(
                name: "IX_ProviderProfiles_UserId",
                table: "ProviderProfile",
                newName: "IX_ProviderProfile_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserRefreshToken",
                table: "UserRefreshToken",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProviderProfile",
                table: "ProviderProfile",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProviderProfile_Users_UserId",
                table: "ProviderProfile",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserRefreshToken_Users_UserId",
                table: "UserRefreshToken",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
