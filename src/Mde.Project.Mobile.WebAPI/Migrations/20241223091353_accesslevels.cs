using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Mde.Project.Mobile.WebAPI.Migrations
{
    /// <inheritdoc />
    public partial class accesslevels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AccessLevels",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { new Guid("00000000-0000-0000-0000-000000000084"), "ManageUsers" },
                    { new Guid("00000000-0000-0000-0000-000000000085"), "CrudProducts" },
                    { new Guid("00000000-0000-0000-0000-000000000086"), "CrudCustomers" },
                    { new Guid("00000000-0000-0000-0000-000000000087"), "SalesRep" }
                });

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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AccessLevels",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000084"));

            migrationBuilder.DeleteData(
                table: "AccessLevels",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000085"));

            migrationBuilder.DeleteData(
                table: "AccessLevels",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000086"));

            migrationBuilder.DeleteData(
                table: "AccessLevels",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000087"));

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "00000000-0000-0000-0000-100000000000",
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEAlj+dg5fjE2sGyIH7VncrSrf4X9K9Bm9/F3ltmxMO2oFTvp2nY630xN0njpLCgjpg==");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "00000000-0000-0000-0000-200000000000",
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEKf47oKKlKtMVYUC1VyxXOp/gihwoJ4uCsgmlCgiFxO3BtGPgBRyOWI3yRwjnUFERg==");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "00000000-0000-0000-0000-300000000000",
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEDpImOOwIUv2jBhFenTZ3T4DRhO2kNz2wOU36ihrXCZho4uDvyHiKh9coyq7wH+nDA==");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "00000000-0000-0000-0000-400000000000",
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEIBIwsemESwFoN4Nz7/4AlWwV5o1vS5j0CSL1C+HPXpBsditTeQmAg8emj2i3DhKeA==");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "00000000-0000-0000-0000-500000000000",
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEGVzUr6QcZFOrniJ2Xb1nhoQ9ukyvwJXgQcBvCMpRHaiTL1RgPxXvrXtclpaMPiz5w==");
        }
    }
}
