using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mde.Project.Mobile.WebAPI.Migrations
{
    /// <inheritdoc />
    public partial class addtruefordangerous : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "00000000-0000-0000-0000-100000000000",
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAENN6/lVFoMHFC4duU8c4z+zlHbfMIFKdQV4u1KzoHUCqEM6iQFeyNfG7LE+mE8IIUA==");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "00000000-0000-0000-0000-200000000000",
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEG8ZoIvvQjRrYYqprh3Pp/oak1UhmH2LfdbJcr/pmCAmASkQ0TiCnUuBmqzgPnYgrA==");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "00000000-0000-0000-0000-300000000000",
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEE0cdClxDc6pduAvrdMka5umPuRaeZnPrdh3V3vt7WiHMGOQ2ssP6N36fU/Xueb5JA==");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "00000000-0000-0000-0000-400000000000",
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAENx/XbNXHvm0EHFu3deCDuz+i5fGKSpwTjl1FDTu2/MRC2gk4WQT1zG10cnvI4/EIw==");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "00000000-0000-0000-0000-500000000000",
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEFDhNRvadiFb5hSDT5CvLkzUFP+KnCFWyEe4sX3hg6bgClFBBWohrvKLJYIP/Rbirg==");

            migrationBuilder.UpdateData(
                table: "Cargos",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000035"),
                column: "IsDangerous",
                value: true);

            migrationBuilder.UpdateData(
                table: "Cargos",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000036"),
                column: "IsDangerous",
                value: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "00000000-0000-0000-0000-100000000000",
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEAEbAc/7rsKj07T1HKdTaRXoXT+pTt3sZoIA+tw/zXRwVeer3bgusboCd1A2sIh0Pw==");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "00000000-0000-0000-0000-200000000000",
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEIf0okfYLXtYZONM5/8QM5alAhUlNSWsXz8ShdCe6/q2PXaZt/bkrNqGrzQh2lSrsA==");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "00000000-0000-0000-0000-300000000000",
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEBIBfP0J7lXAiGYoxY9SVKldOLv5L+bVVPm6Yx7nTjvLranQtxg+PzIGvMW5dx1eCA==");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "00000000-0000-0000-0000-400000000000",
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEGNuIq+nvIU1Zr06rIK7pB0LLaG6UVVQbrEHHFeiwZkNS33iWPApDniUiiO6wpaFrQ==");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "00000000-0000-0000-0000-500000000000",
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAELt57MEU4LYv86sLtyWMocaeGTac2/dSPP2/N7Kh/z22cv+DV6uqvrJ0ztEDVSg2hw==");

            migrationBuilder.UpdateData(
                table: "Cargos",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000035"),
                column: "IsDangerous",
                value: false);

            migrationBuilder.UpdateData(
                table: "Cargos",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000036"),
                column: "IsDangerous",
                value: false);
        }
    }
}
