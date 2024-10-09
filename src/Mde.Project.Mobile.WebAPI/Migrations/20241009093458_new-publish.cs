using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mde.Project.Mobile.WebAPI.Migrations
{
    /// <inheritdoc />
    public partial class newpublish : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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
        }
    }
}
