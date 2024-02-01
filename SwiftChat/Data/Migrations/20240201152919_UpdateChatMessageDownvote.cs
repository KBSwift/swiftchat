using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SwiftChat.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateChatMessageDownvote : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Downvotes",
                table: "ChatMessages",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Downvotes",
                table: "ChatMessages");
        }
    }
}
