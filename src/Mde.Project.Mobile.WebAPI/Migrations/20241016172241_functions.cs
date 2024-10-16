using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Mde.Project.Mobile.WebAPI.Migrations
{
    /// <inheritdoc />
    public partial class functions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AccessLevelType",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<Guid>(
                name: "FunctionId",
                table: "AspNetUsers",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "Functions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Functions", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "00000000-0000-0000-0000-100000000000",
                columns: new[] { "FunctionId", "PasswordHash" },
                values: new object[] { new Guid("00000000-0000-0000-0000-000000000081"), "AQAAAAIAAYagAAAAEEauiFZyQYfde4n7sKwm09p5ARmfuRTt479QIQ772By14WDv9uLz9OM9AvyhtxZJsQ==" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "00000000-0000-0000-0000-200000000000",
                columns: new[] { "FunctionId", "PasswordHash" },
                values: new object[] { new Guid("00000000-0000-0000-0000-000000000082"), "AQAAAAIAAYagAAAAEGP4qt/51AVHv/TckTEpQ66p07bzYZX7RShHxYUVZJuYXRB5g89UzrLaxSZyqNfKPQ==" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "00000000-0000-0000-0000-300000000000",
                columns: new[] { "FunctionId", "PasswordHash" },
                values: new object[] { new Guid("00000000-0000-0000-0000-000000000082"), "AQAAAAIAAYagAAAAEHZqthOKqAjH8tTl3KxLVV28Yx14o7R0oH1AGcU+bIjkQOQi6zEtTcLBBpfq5XdLWg==" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "00000000-0000-0000-0000-400000000000",
                columns: new[] { "FunctionId", "PasswordHash" },
                values: new object[] { new Guid("00000000-0000-0000-0000-000000000083"), "AQAAAAIAAYagAAAAENDPMDjoBQTfr8/L3hVNCTyF200y0YYwH1hpbp2ukcubrByzi4zYEZhCF3COW/4o4w==" });

            migrationBuilder.InsertData(
                table: "Functions",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { new Guid("00000000-0000-0000-0000-000000000081"), "Admin@fedex.com" },
                    { new Guid("00000000-0000-0000-0000-000000000082"), "Driver" },
                    { new Guid("00000000-0000-0000-0000-000000000083"), "Consignee" },
                    { new Guid("00000000-0000-0000-0000-000000000084"), "Consignor" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_FunctionId",
                table: "AspNetUsers",
                column: "FunctionId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Functions_FunctionId",
                table: "AspNetUsers",
                column: "FunctionId",
                principalTable: "Functions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Functions_FunctionId",
                table: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "Functions");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_FunctionId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "FunctionId",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<int>(
                name: "AccessLevelType",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "00000000-0000-0000-0000-100000000000",
                columns: new[] { "AccessLevelType", "PasswordHash" },
                values: new object[] { 2, "AQAAAAIAAYagAAAAEJvrxaY2rjUZxaDw4oEV2odZ1HEvAtKDhFLjmjTHdUICqD6qm4n+5xgTVwwnNIFFCQ==" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "00000000-0000-0000-0000-200000000000",
                columns: new[] { "AccessLevelType", "PasswordHash" },
                values: new object[] { 0, "AQAAAAIAAYagAAAAEHUygX8lEsRXCq36P3zRH+zZGq9ljEHRgLbOvKma/21Cfqxq4QrDrNUXhGRu34jbtw==" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "00000000-0000-0000-0000-300000000000",
                columns: new[] { "AccessLevelType", "PasswordHash" },
                values: new object[] { 0, "AQAAAAIAAYagAAAAEEcARinKMkI3ddUUWGzlGM1uGxxyCHz4UX5ykireuciFTC51iM4BTgxzjbzSDg6T9w==" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "00000000-0000-0000-0000-400000000000",
                columns: new[] { "AccessLevelType", "PasswordHash" },
                values: new object[] { 1, "AQAAAAIAAYagAAAAEOuGF2SJ8wQvi/f9PGPc1PzuZZD4VWsBTBoNbBjLskq79YtuLCWt2TVTCf6ixNMjCw==" });
        }
    }
}
