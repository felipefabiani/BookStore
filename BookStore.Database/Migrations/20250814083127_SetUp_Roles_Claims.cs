using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BookStore.Database.Migrations
{
    /// <inheritdoc />
    public partial class SetUp_Roles_Claims : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "UserClaims",
                keyColumns: new[] { "ClaimId", "UserId" },
                keyValues: new object[] { 1, 1 });

            migrationBuilder.DeleteData(
                table: "UserClaims",
                keyColumns: new[] { "ClaimId", "UserId" },
                keyValues: new object[] { 2, 1 });

            migrationBuilder.DeleteData(
                table: "UserClaims",
                keyColumns: new[] { "ClaimId", "UserId" },
                keyValues: new object[] { 3, 1 });

            migrationBuilder.DeleteData(
                table: "UserClaims",
                keyColumns: new[] { "ClaimId", "UserId" },
                keyValues: new object[] { 4, 1 });

            migrationBuilder.DeleteData(
                table: "UserClaims",
                keyColumns: new[] { "ClaimId", "UserId" },
                keyValues: new object[] { 5, 1 });

            migrationBuilder.DeleteData(
                table: "UserClaims",
                keyColumns: new[] { "ClaimId", "UserId" },
                keyValues: new object[] { 6, 1 });

            migrationBuilder.DeleteData(
                table: "UserClaims",
                keyColumns: new[] { "ClaimId", "UserId" },
                keyValues: new object[] { 7, 1 });

            migrationBuilder.DeleteData(
                table: "UserClaims",
                keyColumns: new[] { "ClaimId", "UserId" },
                keyValues: new object[] { 8, 1 });

            migrationBuilder.DeleteData(
                table: "UserClaims",
                keyColumns: new[] { "ClaimId", "UserId" },
                keyValues: new object[] { 9, 1 });

            migrationBuilder.DeleteData(
                table: "UserClaims",
                keyColumns: new[] { "ClaimId", "UserId" },
                keyValues: new object[] { 1, 2 });

            migrationBuilder.DeleteData(
                table: "UserClaims",
                keyColumns: new[] { "ClaimId", "UserId" },
                keyValues: new object[] { 2, 2 });

            migrationBuilder.DeleteData(
                table: "UserClaims",
                keyColumns: new[] { "ClaimId", "UserId" },
                keyValues: new object[] { 3, 2 });

            migrationBuilder.DeleteData(
                table: "UserClaims",
                keyColumns: new[] { "ClaimId", "UserId" },
                keyValues: new object[] { 4, 2 });

            migrationBuilder.DeleteData(
                table: "UserClaims",
                keyColumns: new[] { "ClaimId", "UserId" },
                keyValues: new object[] { 5, 3 });

            migrationBuilder.DeleteData(
                table: "UserClaims",
                keyColumns: new[] { "ClaimId", "UserId" },
                keyValues: new object[] { 6, 3 });

            migrationBuilder.DeleteData(
                table: "UserClaims",
                keyColumns: new[] { "ClaimId", "UserId" },
                keyValues: new object[] { 7, 3 });

            migrationBuilder.DeleteData(
                table: "UserClaims",
                keyColumns: new[] { "ClaimId", "UserId" },
                keyValues: new object[] { 8, 3 });

            migrationBuilder.DeleteData(
                table: "UserClaims",
                keyColumns: new[] { "ClaimId", "UserId" },
                keyValues: new object[] { 9, 4 });

            migrationBuilder.DeleteData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { 1, 1 });

            migrationBuilder.DeleteData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { 2, 1 });

            migrationBuilder.DeleteData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { 3, 1 });

            migrationBuilder.DeleteData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { 1, 2 });

            migrationBuilder.DeleteData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { 2, 3 });

            migrationBuilder.DeleteData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { 3, 4 });

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 4);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "DateOfBirth", "Email", "FirstName", "LastName", "Password" },
                values: new object[,]
                {
                    { 1, new DateTimeOffset(new DateTime(1985, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "full.access@article.ie", "Full", "Access", "hashed-password-1" },
                    { 2, new DateTimeOffset(new DateTime(1985, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "admin.test@article.ie", "Admin", "Test", "hashed-password-2" },
                    { 3, new DateTimeOffset(new DateTime(1985, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "author.test@article.ie", "Author", "Test", "hashed-password-3" },
                    { 4, new DateTimeOffset(new DateTime(1985, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "user.test@article.ie", "User", "Test", "hashed-password-4" }
                });

            migrationBuilder.InsertData(
                table: "UserClaims",
                columns: new[] { "ClaimId", "UserId" },
                values: new object[,]
                {
                    { 1, 1 },
                    { 2, 1 },
                    { 3, 1 },
                    { 4, 1 },
                    { 5, 1 },
                    { 6, 1 },
                    { 7, 1 },
                    { 8, 1 },
                    { 9, 1 },
                    { 1, 2 },
                    { 2, 2 },
                    { 3, 2 },
                    { 4, 2 },
                    { 5, 3 },
                    { 6, 3 },
                    { 7, 3 },
                    { 8, 3 },
                    { 9, 4 }
                });

            migrationBuilder.InsertData(
                table: "UserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[,]
                {
                    { 1, 1 },
                    { 2, 1 },
                    { 3, 1 },
                    { 1, 2 },
                    { 2, 3 },
                    { 3, 4 }
                });
        }
    }
}
