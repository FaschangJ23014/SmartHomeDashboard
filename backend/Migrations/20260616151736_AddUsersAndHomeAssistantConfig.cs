using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backend.Migrations
{
    /// <inheritdoc />
    public partial class AddUsersAndHomeAssistantConfig : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AppUserId",
                table: "Rooms",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "AppUserId",
                table: "Devices",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "AppUsers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Email = table.Column<string>(type: "TEXT", nullable: false),
                    DisplayName = table.Column<string>(type: "TEXT", nullable: false),
                    PasswordHash = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "HomeAssistantConfigs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    BaseUrl = table.Column<string>(type: "TEXT", nullable: false),
                    TokenEncrypted = table.Column<string>(type: "TEXT", nullable: false),
                    AppUserId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HomeAssistantConfigs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HomeAssistantConfigs_AppUsers_AppUserId",
                        column: x => x.AppUserId,
                        principalTable: "AppUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Rooms_AppUserId",
                table: "Rooms",
                column: "AppUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Devices_AppUserId",
                table: "Devices",
                column: "AppUserId");

            migrationBuilder.CreateIndex(
                name: "IX_HomeAssistantConfigs_AppUserId",
                table: "HomeAssistantConfigs",
                column: "AppUserId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Devices_AppUsers_AppUserId",
                table: "Devices",
                column: "AppUserId",
                principalTable: "AppUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Rooms_AppUsers_AppUserId",
                table: "Rooms",
                column: "AppUserId",
                principalTable: "AppUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Devices_AppUsers_AppUserId",
                table: "Devices");

            migrationBuilder.DropForeignKey(
                name: "FK_Rooms_AppUsers_AppUserId",
                table: "Rooms");

            migrationBuilder.DropTable(
                name: "HomeAssistantConfigs");

            migrationBuilder.DropTable(
                name: "AppUsers");

            migrationBuilder.DropIndex(
                name: "IX_Rooms_AppUserId",
                table: "Rooms");

            migrationBuilder.DropIndex(
                name: "IX_Devices_AppUserId",
                table: "Devices");

            migrationBuilder.DropColumn(
                name: "AppUserId",
                table: "Rooms");

            migrationBuilder.DropColumn(
                name: "AppUserId",
                table: "Devices");
        }
    }
}
