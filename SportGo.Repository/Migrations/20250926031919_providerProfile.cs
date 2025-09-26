using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SportGo.Repository.Migrations
{
    /// <inheritdoc />
    public partial class providerProfile : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ProviderStatus",
                table: "Users",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ProviderProfile",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    BusinessName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Address = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    BusinessPhoneNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    BusinessLicenseUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BankAccountName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    BankAccountNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    BankName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProviderProfile", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProviderProfile_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProviderProfile_UserId",
                table: "ProviderProfile",
                column: "UserId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProviderProfile");

            migrationBuilder.DropColumn(
                name: "ProviderStatus",
                table: "Users");
        }
    }
}
