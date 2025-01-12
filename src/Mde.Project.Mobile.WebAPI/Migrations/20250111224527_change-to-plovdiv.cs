using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mde.Project.Mobile.WebAPI.Migrations
{
    /// <inheritdoc />
    public partial class changetoplovdiv : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "00000000-0000-0000-0000-100000000000",
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAECjFGtby/Q2FPah9By0BulCJuJtgqA2/bvHGPi0upQurQv8dJB1QRlBn+FFHNJu9VA==");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "00000000-0000-0000-0000-200000000000",
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEGloLjS4zLsibZ+WG82t3tDc9AVqTkgWzEXmTKXuqcdADkNm5Ut6rC9BHe23FfDZ0Q==");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "00000000-0000-0000-0000-300000000000",
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEOouIDkLml4uGbwUYjNyVmIRG12eIGC9d/9jkbjogdqoe4AKRKRjjx7eZE8WxL+rCw==");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "00000000-0000-0000-0000-400000000000",
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEJckXtmtKH7VJxR7AeQlNOXAu8iqKbBKz3SwhKVGoVzQm6iqsO6tgyzuhL4gfM/lCA==");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "00000000-0000-0000-0000-500000000000",
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEMMNp5TlSoQ3IK8IIi3ysh/AEE2gYg3UZahQYF9Csxm7YD/VAA/ArNgNkNr9moglGg==");

            migrationBuilder.UpdateData(
                table: "Cargos",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000032"),
                column: "Destination",
                value: "Tsar Simeon 2, 4023 Plovdiv, Bulgaria");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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
        }
    }
}
