using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mde.Project.Mobile.WebAPI.Migrations
{
    /// <inheritdoc />
    public partial class removeconst : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "00000000-0000-0000-0000-100000000000",
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEJvrxaY2rjUZxaDw4oEV2odZ1HEvAtKDhFLjmjTHdUICqD6qm4n+5xgTVwwnNIFFCQ==");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "00000000-0000-0000-0000-200000000000",
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEHUygX8lEsRXCq36P3zRH+zZGq9ljEHRgLbOvKma/21Cfqxq4QrDrNUXhGRu34jbtw==");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "00000000-0000-0000-0000-300000000000",
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEEcARinKMkI3ddUUWGzlGM1uGxxyCHz4UX5ykireuciFTC51iM4BTgxzjbzSDg6T9w==");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "00000000-0000-0000-0000-400000000000",
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEOuGF2SJ8wQvi/f9PGPc1PzuZZD4VWsBTBoNbBjLskq79YtuLCWt2TVTCf6ixNMjCw==");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "00000000-0000-0000-0000-100000000000",
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEKUUNj4Pn3xGd9d27kRQSzAPtyhy1xosPzy4ACvNNyzK09WS4iaJOEQvJ9Me1QkeQg==");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "00000000-0000-0000-0000-200000000000",
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEFadVYe/iMvFakYdVHjEm1Q5rcT4tekHaxLW14glRrorC9fqF4ed5N8i/Sz/dIsUaw==");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "00000000-0000-0000-0000-300000000000",
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEOPTeNivKpnDoyfyVS5f31dg0nvrnC2dFzn6oNbpEhiWVujWHgFzUIydASe6RipArA==");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "00000000-0000-0000-0000-400000000000",
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEE9V/OJ5MraW84galuPnQZAq7JkcGSTLHzL0/KVj/8pZwH8CIOWrEqhPGaXK22EgtQ==");
        }
    }
}
