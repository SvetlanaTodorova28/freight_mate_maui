using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mde.Project.Mobile.WebAPI.Migrations
{
    /// <inheritdoc />
    public partial class consignorseed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "00000000-0000-0000-0000-000000000064", null, "Consignor", "CONSIGNOR" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "00000000-0000-0000-0000-100000000000",
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEARhgyEtQMe38f+V6mmpefFh00XAq65xOflyB6dOREksE4ByUmMnca8IwCl8LmFxwQ==");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "00000000-0000-0000-0000-200000000000",
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEGfuOg1a0egRlv12AAJ3HolgnUjWrQgGuZl2LLWdz+xk0FAkr4pdb2ueFFVnVw42ZA==");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "00000000-0000-0000-0000-300000000000",
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEMioIBTJhdUTF2AiP3dDb33/nYjzWXed8zx3JfScSD0cgoAd+rPZErlLvBKY0U5uVQ==");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "00000000-0000-0000-0000-400000000000",
                columns: new[] { "FunctionId", "PasswordHash" },
                values: new object[] { new Guid("00000000-0000-0000-0000-000000000084"), "AQAAAAIAAYagAAAAEAkFy7JDezSh+POEbvoLKDeopktKmt0jXqzdrXtiDLl2jAvp03wnISbXR+JuWMgmyQ==" });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "FirstName", "FunctionId", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "00000000-0000-0000-0000-500000000000", 0, "3YET3ANOTHER3UNIQUE3STRING3", "s@t.com", true, "Sve", new Guid("00000000-0000-0000-0000-000000000083"), "Tod", false, null, "S@T.COM", "S@T.COM", "AQAAAAIAAYagAAAAECgXGM3y+aTU8pO/uAJm3dRVKgsvHo2G3i//L6oInpuczNHCJGY2prW/sKHFh+njpA==", null, false, "3DIFFERENT3UNIQUE3STRING3", false, "s@t.com" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "00000000-0000-0000-0000-000000000064", "00000000-0000-0000-0000-500000000000" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "00000000-0000-0000-0000-000000000064", "00000000-0000-0000-0000-500000000000" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "00000000-0000-0000-0000-000000000064");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "00000000-0000-0000-0000-500000000000");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "00000000-0000-0000-0000-100000000000",
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEIgxFm98kyT7H2T04iUV88Bxo9QIF+pRw5X3cIpGzJFlFe7qpFhlinNZK+V/cjse7g==");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "00000000-0000-0000-0000-200000000000",
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEMMwU1IohY9IFZst9SBvBMKQlkOyAOEtPyffogE0FXW2b5RMJ4Aj74yEqHdWLHphyw==");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "00000000-0000-0000-0000-300000000000",
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEL4pFtNaPg6lkFjQN8Q5Ijwded9BVqNgToQwpLoczp6TbPpvGyQkyCLwDXGxSpW6oQ==");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "00000000-0000-0000-0000-400000000000",
                columns: new[] { "FunctionId", "PasswordHash" },
                values: new object[] { new Guid("00000000-0000-0000-0000-000000000083"), "AQAAAAIAAYagAAAAEFuyp9XpU93E0l1wX2LxrRz1xZEYhIbF6bYzC661StuK97vId6KD7/CvLs3a6/+6bQ==" });
        }
    }
}
