using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mde.Project.Mobile.WebAPI.Migrations
{
    /// <inheritdoc />
    public partial class withoutconst : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "00000000-0000-0000-0000-100000000000",
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAECDMwMMv/Z4Zr2mXOuHK0aShfKywsDAU4+dQlHbaY7NTTYaytK4CtOTbrJmW1wL4GQ==");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "00000000-0000-0000-0000-200000000000",
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEJjiw2kUeNEpRwLeujqf+8RXjvfoyb7s822sf3tjRDp7/uMHiERszHCdX+F0x2O1mQ==");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "00000000-0000-0000-0000-300000000000",
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEBMNAOHddJ3VWUs6J6SB8724OM7676xDIqMgOLF1zhmeQ0RE8xcSGDRFEoEnf0IDeg==");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "00000000-0000-0000-0000-400000000000",
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEEXlfK/SrRYsq5yXr6z0GoI6Z87qYCgzR3pUZ/jFYbjenGP4heyjaplyQobpvttR8w==");
        }
    }
}
