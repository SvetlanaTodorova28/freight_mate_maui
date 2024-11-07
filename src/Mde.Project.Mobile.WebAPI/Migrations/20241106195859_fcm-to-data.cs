using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mde.Project.Mobile.WebAPI.Migrations
{
    /// <inheritdoc />
    public partial class fcmtodata : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FCMToken",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "00000000-0000-0000-0000-100000000000",
                columns: new[] { "FCMToken", "PasswordHash" },
                values: new object[] { null, "AQAAAAIAAYagAAAAEAlj+dg5fjE2sGyIH7VncrSrf4X9K9Bm9/F3ltmxMO2oFTvp2nY630xN0njpLCgjpg==" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "00000000-0000-0000-0000-200000000000",
                columns: new[] { "FCMToken", "PasswordHash" },
                values: new object[] { null, "AQAAAAIAAYagAAAAEKf47oKKlKtMVYUC1VyxXOp/gihwoJ4uCsgmlCgiFxO3BtGPgBRyOWI3yRwjnUFERg==" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "00000000-0000-0000-0000-300000000000",
                columns: new[] { "FCMToken", "PasswordHash" },
                values: new object[] { null, "AQAAAAIAAYagAAAAEDpImOOwIUv2jBhFenTZ3T4DRhO2kNz2wOU36ihrXCZho4uDvyHiKh9coyq7wH+nDA==" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "00000000-0000-0000-0000-400000000000",
                columns: new[] { "FCMToken", "PasswordHash" },
                values: new object[] { null, "AQAAAAIAAYagAAAAEIBIwsemESwFoN4Nz7/4AlWwV5o1vS5j0CSL1C+HPXpBsditTeQmAg8emj2i3DhKeA==" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "00000000-0000-0000-0000-500000000000",
                columns: new[] { "FCMToken", "PasswordHash" },
                values: new object[] { null, "AQAAAAIAAYagAAAAEGVzUr6QcZFOrniJ2Xb1nhoQ9ukyvwJXgQcBvCMpRHaiTL1RgPxXvrXtclpaMPiz5w==" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FCMToken",
                table: "AspNetUsers");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "00000000-0000-0000-0000-100000000000",
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEI9+Z0mTOYbXjekkEBajPam7V9D1SN54I92Qpqd3fad+tB2hSjr+S/NtkfadEglu/g==");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "00000000-0000-0000-0000-200000000000",
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEAMU0NpNyrZ9BkaCkstrtv8TIJU6bW23aZwYgzPFQA5RsvSRlp70QpuiAz/Dady70A==");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "00000000-0000-0000-0000-300000000000",
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAENru5gXiewzsThc1kAyZZq9LCPCu1HhYtoWU111w316q3jhOwHUSvU+Xsr/VAde2vg==");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "00000000-0000-0000-0000-400000000000",
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEDG2ab6ltBStYsB75R+QiSKgRXp7SaMejOGxT8hXds5ajs5gAdl/vCAFkMelSMxI/Q==");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "00000000-0000-0000-0000-500000000000",
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEKj13DLu0qE4Fj3jUgdBGIMcbmVrfTkvG8O+M916TNfyjilYlADVAkLcD3fpYVoqnA==");
        }
    }
}
