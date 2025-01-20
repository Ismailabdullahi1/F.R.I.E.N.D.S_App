using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FriendsApp.Migrations
{
    /// <inheritdoc />
    public partial class ConfigureCascadeDelete : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PlanFriends_Friends_FriendsId",
                table: "PlanFriends");

            migrationBuilder.RenameColumn(
                name: "FriendsId",
                table: "PlanFriends",
                newName: "FriendId");

            migrationBuilder.AddForeignKey(
                name: "FK_PlanFriends_Friends_FriendId",
                table: "PlanFriends",
                column: "FriendId",
                principalTable: "Friends",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PlanFriends_Friends_FriendId",
                table: "PlanFriends");

            migrationBuilder.RenameColumn(
                name: "FriendId",
                table: "PlanFriends",
                newName: "FriendsId");

            migrationBuilder.AddForeignKey(
                name: "FK_PlanFriends_Friends_FriendsId",
                table: "PlanFriends",
                column: "FriendsId",
                principalTable: "Friends",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
