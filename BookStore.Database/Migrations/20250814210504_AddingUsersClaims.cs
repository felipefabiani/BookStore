using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookStore.Database.Migrations
{
    /// <inheritdoc />
    public partial class AddingUsersClaims : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ClaimUser_Claims_ClaimsId",
                table: "ClaimUser");

            migrationBuilder.DropForeignKey(
                name: "FK_ClaimUser_Users_UsersId",
                table: "ClaimUser");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ClaimUser",
                table: "ClaimUser");

            migrationBuilder.RenameTable(
                name: "ClaimUser",
                newName: "UsersClaims");

            migrationBuilder.RenameIndex(
                name: "IX_ClaimUser_UsersId",
                table: "UsersClaims",
                newName: "IX_UsersClaims_UsersId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UsersClaims",
                table: "UsersClaims",
                columns: new[] { "ClaimsId", "UsersId" });

            migrationBuilder.AddForeignKey(
                name: "FK_UsersClaims_Claims_ClaimsId",
                table: "UsersClaims",
                column: "ClaimsId",
                principalTable: "Claims",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UsersClaims_Users_UsersId",
                table: "UsersClaims",
                column: "UsersId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UsersClaims_Claims_ClaimsId",
                table: "UsersClaims");

            migrationBuilder.DropForeignKey(
                name: "FK_UsersClaims_Users_UsersId",
                table: "UsersClaims");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UsersClaims",
                table: "UsersClaims");

            migrationBuilder.RenameTable(
                name: "UsersClaims",
                newName: "ClaimUser");

            migrationBuilder.RenameIndex(
                name: "IX_UsersClaims_UsersId",
                table: "ClaimUser",
                newName: "IX_ClaimUser_UsersId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ClaimUser",
                table: "ClaimUser",
                columns: new[] { "ClaimsId", "UsersId" });

            migrationBuilder.AddForeignKey(
                name: "FK_ClaimUser_Claims_ClaimsId",
                table: "ClaimUser",
                column: "ClaimsId",
                principalTable: "Claims",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ClaimUser_Users_UsersId",
                table: "ClaimUser",
                column: "UsersId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
