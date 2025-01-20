using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FriendsApp.Migrations
{
    /// <inheritdoc />
    public partial class UpdatePlanAndMeetingCascade : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PlanFriends_Friends_FriendId",
                table: "PlanFriends");

            migrationBuilder.AddForeignKey(
                name: "FK_PlanFriends_Friends_FriendId",
                table: "PlanFriends",
                column: "FriendId",
                principalTable: "Friends",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PlanFriends_Friends_FriendId",
                table: "PlanFriends");

            migrationBuilder.AddForeignKey(
                name: "FK_PlanFriends_Friends_FriendId",
                table: "PlanFriends",
                column: "FriendId",
                principalTable: "Friends",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
