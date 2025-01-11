using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mde.Project.Mobile.WebAPI.Migrations
{
    /// <inheritdoc />
    public partial class changenewaddresses : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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
                keyValue: new Guid("00000000-0000-0000-0000-000000000031"),
                columns: new[] { "AppUserId", "Destination" },
                values: new object[] { "00000000-0000-0000-0000-400000000000", "Via Giambellino, 7, 20146 Milan MI, Italy" });

            migrationBuilder.UpdateData(
                table: "Cargos",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000032"),
                column: "Destination",
                value: "Cody Road, London E16 4SR, United Kingdom");

            migrationBuilder.UpdateData(
                table: "Cargos",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000033"),
                column: "Destination",
                value: "Kustlaan 32, 8380 Bruges, Belgium");

            migrationBuilder.UpdateData(
                table: "Cargos",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000034"),
                column: "Destination",
                value: "Prof. Tsvetan Lazarov 3, 1592 Sofia, Bulgaria");

            migrationBuilder.UpdateData(
                table: "Cargos",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000035"),
                column: "Destination",
                value: "Heiststraat 10, 8380 Bruges, Belgium");

            migrationBuilder.UpdateData(
                table: "Cargos",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000036"),
                columns: new[] { "AppUserId", "Destination" },
                values: new object[] { "00000000-0000-0000-0000-500000000000", "Berolinastraße 7, 10178 Berlin, Germany" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "00000000-0000-0000-0000-100000000000",
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEO5mGvV4jJntyv5u4C7aZCajbUl1Zf3c1WO9x+PRpYJM4vS0w88zQklmlN0lnA8HIQ==");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "00000000-0000-0000-0000-200000000000",
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEEu8Wzkg7VFVdQxs1YEyvOLGx9pqQhk84EFSFarQp6vLL83Xby/YU5blOeOwYW/k9g==");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "00000000-0000-0000-0000-300000000000",
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAELnhL5vNWr+1uqiHYfcVyAOwLksMtSvzFUU+O1gR7qvGTKx/tR37ZNziV0UbPRphzw==");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "00000000-0000-0000-0000-400000000000",
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEI8QqCE4bMvBurg3jqKfyltw0dMszI4Etma/ckdEP7Qur6N85tbWv3o/r3SFtDDT+Q==");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "00000000-0000-0000-0000-500000000000",
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEJUqWdUqys3k3sHGc1pZtqfNxkHL7g0CxgXm6fXbMfFK79r3BdriSd86pXU9BbGb0A==");

            migrationBuilder.UpdateData(
                table: "Cargos",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000031"),
                columns: new[] { "AppUserId", "Destination" },
                values: new object[] { "00000000-0000-0000-0000-200000000000", "Milan" });

            migrationBuilder.UpdateData(
                table: "Cargos",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000032"),
                column: "Destination",
                value: "London");

            migrationBuilder.UpdateData(
                table: "Cargos",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000033"),
                column: "Destination",
                value: "Zeebrugge");

            migrationBuilder.UpdateData(
                table: "Cargos",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000034"),
                column: "Destination",
                value: "Sofia");

            migrationBuilder.UpdateData(
                table: "Cargos",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000035"),
                column: "Destination",
                value: "Zeebrugge");

            migrationBuilder.UpdateData(
                table: "Cargos",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000036"),
                columns: new[] { "AppUserId", "Destination" },
                values: new object[] { "00000000-0000-0000-0000-300000000000", "Berlin" });
        }
    }
}
