using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mde.Project.Mobile.WebAPI.Migrations
{
    /// <inheritdoc />
    public partial class removebruges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Cargos",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000033"));

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "00000000-0000-0000-0000-100000000000",
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAECGbL8VwcvnOGIL8LDRwwnAQcSOaw6TG+jR7fe3oZrlBmJrbB7iFrL8ek9rrjXIu7A==");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "00000000-0000-0000-0000-200000000000",
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEOh9WJsesoyZ50KKkdOO+eZAnkP6ObJ2EGBXhV4Vrv9+TLl6agN1i0bFcNLKGbDEWg==");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "00000000-0000-0000-0000-300000000000",
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAED91/XdCVvzDRsqm/+EXyAETn5DUOw0rbstMNXFh0BnMQ8mwdgcx6KnOOwRlgwLFpg==");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "00000000-0000-0000-0000-400000000000",
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEHvq6O7cDAWasv1sYxmjMZ+7GwRB8+26R68M0RcgrH4OLtQB1gpDaeh2tFhUwuWZ4A==");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "00000000-0000-0000-0000-500000000000",
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEBh3CSH5mcwriDLVBjTVg9qRidEZjrV+gmetNWGViBwowLVWRkBVTp6PK5MIO0TYJA==");

            migrationBuilder.UpdateData(
                table: "Cargos",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000032"),
                column: "Destination",
                value: "Cody Rd, London E16 4SR, United Kingdom");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "00000000-0000-0000-0000-100000000000",
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAELAPDUahzRZnp6zaJAX9gFkAUGEMJbRUEONHKh1m96wGpjuTzoRFjxZZ7n2cJOZQJw==");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "00000000-0000-0000-0000-200000000000",
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEH/93ARn1WakbJ0/g7kuQBZRDyB5fOJtxRacY3lpl5OmebHgw/iZ+pxd8/ulLzDKRA==");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "00000000-0000-0000-0000-300000000000",
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEI7Jq2oNWapizY+9ERAW/Cf50IoyOcBPcwtoGuiQ5rEobgU75ad7OEx0tl3zYNHQJQ==");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "00000000-0000-0000-0000-400000000000",
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAECIlqYxwXT5+41XybB80fpUGxyrPHn2RaK6nhTnqIM9AXEkgqTZr3QseFwCi4fmmWg==");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "00000000-0000-0000-0000-500000000000",
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEEInGk3vQF62pNNq1/ERZChiJCbV33mwEtDCixctElzzvNPkfFjPD7GlJhdhIMUebA==");

            migrationBuilder.UpdateData(
                table: "Cargos",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000032"),
                column: "Destination",
                value: "Cody Road, London E16 4SR, United Kingdom");

            migrationBuilder.InsertData(
                table: "Cargos",
                columns: new[] { "Id", "AppUserId", "Destination", "IsDangerous", "TotalWeight" },
                values: new object[] { new Guid("00000000-0000-0000-0000-000000000033"), "00000000-0000-0000-0000-400000000000", "Kustlaan 32, 8380 Bruges, Belgium", false, 1500.5 });
        }
    }
}
