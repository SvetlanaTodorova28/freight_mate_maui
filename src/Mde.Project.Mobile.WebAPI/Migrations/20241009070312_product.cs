using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mde.Project.Mobile.WebAPI.Migrations
{
    /// <inheritdoc />
    public partial class product : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "00000000-0000-0000-0000-100000000000",
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEJ0/V5jdwvbw2iEBHGBNTdag0wXtd675P0+P1ghjYQEykNhBH65ZTMiYTAFI1PgM7A==");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "00000000-0000-0000-0000-200000000000",
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAENBpTKzYx2SVLpP3qZKzdfPXY2mrKjADRh27Z59f9BAm/J63+9ZMYKnqCdkYfJtYUA==");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "00000000-0000-0000-0000-300000000000",
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEKXV64aiUJW+laOrl6yAAmvsbFfnIWb3MBa80sfmerj6RbGf3Jgz+1gaJJSryjm5Rw==");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "00000000-0000-0000-0000-400000000000",
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEG8rUTk0Y84gJ7fNka6MP14krV0vBS6IHfOiUoWYSnFgEwec35pdWwK/wC0wKLxsIQ==");

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "CategoryId", "IsDangerous", "Name" },
                values: new object[] { new Guid("00000000-0000-0000-0000-000000000024"), new Guid("00000000-0000-0000-0000-000000000012"), false, "Training" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000024"));

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "00000000-0000-0000-0000-100000000000",
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEId/YRCKJWf6GtLLUQ2BV4rvabyk53dao9QF99V06gp7SeiekcX4dbDgBrGkyi3cLw==");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "00000000-0000-0000-0000-200000000000",
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEGlk79a78JDOUzaI1KByDICXzhj9f7Jlsv+GjIUXd/jBp/Z0vd5PC2VIpJFhfQvtOA==");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "00000000-0000-0000-0000-300000000000",
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEM8A/SmFgVeNtM2mI0fszv30g2ff9zzq6UiaPVDL8nruVYYmIbvcPpV6KEh3AO8tgg==");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "00000000-0000-0000-0000-400000000000",
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEESkId3fRZtobvQKiV1pL+hhNBAQJYe7+nVXFd+LMMgJ/MWRGhfSsxwYmnyB0ubVyQ==");
        }
    }
}
