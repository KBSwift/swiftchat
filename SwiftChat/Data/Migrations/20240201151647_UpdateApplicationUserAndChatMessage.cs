using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SwiftChat.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateApplicationUserAndChatMessage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Upvotes",
                table: "ChatMessages",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "SavedMessages",
                columns: table => new
                {
                    SavedByUsersId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    SavedMessagesId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SavedMessages", x => new { x.SavedByUsersId, x.SavedMessagesId });
                    table.ForeignKey(
                        name: "FK_SavedMessages_AspNetUsers_SavedByUsersId",
                        column: x => x.SavedByUsersId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SavedMessages_ChatMessages_SavedMessagesId",
                        column: x => x.SavedMessagesId,
                        principalTable: "ChatMessages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SavedMessages_SavedMessagesId",
                table: "SavedMessages",
                column: "SavedMessagesId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SavedMessages");

            migrationBuilder.DropColumn(
                name: "Upvotes",
                table: "ChatMessages");
        }
    }
}
