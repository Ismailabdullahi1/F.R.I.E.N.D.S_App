using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FriendsApp.Migrations
{
    /// <inheritdoc />
    public partial class RestrictPlanAndMeetingDeletion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Meetings_Friends_FriendId",
                table: "Meetings");

            migrationBuilder.DropForeignKey(
                name: "FK_PlanFriends_Plans_PlanId",
                table: "PlanFriends");

            migrationBuilder.AddForeignKey(
                name: "FK_Meetings_Friends_FriendId",
                table: "Meetings",
                column: "FriendId",
                principalTable: "Friends",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PlanFriends_Plans_PlanId",
                table: "PlanFriends",
                column: "PlanId",
                principalTable: "Plans",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Meetings_Friends_FriendId",
                table: "Meetings");

            migrationBuilder.DropForeignKey(
                name: "FK_PlanFriends_Plans_PlanId",
                table: "PlanFriends");

            migrationBuilder.AddForeignKey(
                name: "FK_Meetings_Friends_FriendId",
                table: "Meetings",
                column: "FriendId",
                principalTable: "Friends",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PlanFriends_Plans_PlanId",
                table: "PlanFriends",
                column: "PlanId",
                principalTable: "Plans",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
