using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mde.Project.Mobile.WebAPI.Migrations
{
    /// <inheritdoc />
    public partial class rousse : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "00000000-0000-0000-0000-100000000000",
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEBbofXpYSlalq16rpr17FSO4PZpS99bSbZzEQUBHYGOO89ludChQt/ekRmVpPLoQIw==");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "00000000-0000-0000-0000-200000000000",
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEAWAJ3l7j5YNhmHyyoseZuylW1uR1XUXUV75GlveGIVE7JbAOLyGjwGaG4pruiOM9A==");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "00000000-0000-0000-0000-300000000000",
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEMHvOs+RVvKy2+qedg7blba+lTUnEmgXeb0kxNbzGpSnqnTnhPsJFfn1B3gPzn0twg==");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "00000000-0000-0000-0000-400000000000",
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEOad7wkYp7gVZQDE46AEsr8H46+RSm+HHCRlB7JM7tcC2xlbhKeBypUjqrIZeLXBrw==");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "00000000-0000-0000-0000-500000000000",
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAENwMYGCGic9EgkotihqlRgiELyPDCJBalwE81qNeQcgTBD5Ezl2NO9gJCvz5GWUzzA==");

            migrationBuilder.UpdateData(
                table: "Cargos",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000032"),
                column: "Destination",
                value: "Rodopi 2, 7005 Rousse, Bulgaria");

            migrationBuilder.UpdateData(
                table: "Cargos",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000035"),
                column: "Destination",
                value: "Heiststraat 10, 8380 Brugge, Belgium");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
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

            migrationBuilder.UpdateData(
                table: "Cargos",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000035"),
                column: "Destination",
                value: "Heiststraat 10, 8380 Bruges, Belgium");
        }
    }
}
