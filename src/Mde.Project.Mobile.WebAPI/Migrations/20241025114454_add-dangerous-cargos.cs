using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Mde.Project.Mobile.WebAPI.Migrations
{
    /// <inheritdoc />
    public partial class adddangerouscargos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AppUserCargo",
                keyColumns: new[] { "AppUsersId", "CargosId" },
                keyValues: new object[] { "00000000-0000-0000-0000-400000000000", new Guid("00000000-0000-0000-0000-000000000033") });

            migrationBuilder.DropColumn(
                name: "IsDangerous",
                table: "Products");

            migrationBuilder.AddColumn<bool>(
                name: "IsDangerous",
                table: "Cargos",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.InsertData(
                table: "AppUserCargo",
                columns: new[] { "AppUsersId", "CargosId" },
                values: new object[,]
                {
                    { "00000000-0000-0000-0000-400000000000", new Guid("00000000-0000-0000-0000-000000000035") },
                    { "00000000-0000-0000-0000-500000000000", new Guid("00000000-0000-0000-0000-000000000036") }
                });

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

            migrationBuilder.InsertData(
                table: "CargoProduct",
                columns: new[] { "CargosId", "ProductsId" },
                values: new object[,]
                {
                    { new Guid("00000000-0000-0000-0000-000000000035"), new Guid("00000000-0000-0000-0000-000000000024") },
                    { new Guid("00000000-0000-0000-0000-000000000036"), new Guid("00000000-0000-0000-0000-000000000024") }
                });

            migrationBuilder.UpdateData(
                table: "Cargos",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000031"),
                column: "IsDangerous",
                value: false);

            migrationBuilder.UpdateData(
                table: "Cargos",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000032"),
                column: "IsDangerous",
                value: false);

            migrationBuilder.UpdateData(
                table: "Cargos",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000033"),
                column: "IsDangerous",
                value: false);

            migrationBuilder.UpdateData(
                table: "Cargos",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000034"),
                column: "IsDangerous",
                value: false);

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

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000024"),
                column: "Name",
                value: "Gaz");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AppUserCargo",
                keyColumns: new[] { "AppUsersId", "CargosId" },
                keyValues: new object[] { "00000000-0000-0000-0000-400000000000", new Guid("00000000-0000-0000-0000-000000000035") });

            migrationBuilder.DeleteData(
                table: "AppUserCargo",
                keyColumns: new[] { "AppUsersId", "CargosId" },
                keyValues: new object[] { "00000000-0000-0000-0000-500000000000", new Guid("00000000-0000-0000-0000-000000000036") });

            migrationBuilder.DeleteData(
                table: "CargoProduct",
                keyColumns: new[] { "CargosId", "ProductsId" },
                keyValues: new object[] { new Guid("00000000-0000-0000-0000-000000000035"), new Guid("00000000-0000-0000-0000-000000000024") });

            migrationBuilder.DeleteData(
                table: "CargoProduct",
                keyColumns: new[] { "CargosId", "ProductsId" },
                keyValues: new object[] { new Guid("00000000-0000-0000-0000-000000000036"), new Guid("00000000-0000-0000-0000-000000000024") });

            migrationBuilder.DropColumn(
                name: "IsDangerous",
                table: "Cargos");

            migrationBuilder.AddColumn<bool>(
                name: "IsDangerous",
                table: "Products",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.InsertData(
                table: "AppUserCargo",
                columns: new[] { "AppUsersId", "CargosId" },
                values: new object[] { "00000000-0000-0000-0000-400000000000", new Guid("00000000-0000-0000-0000-000000000033") });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "00000000-0000-0000-0000-100000000000",
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEAq2K2QpDGuD9Xbu0MRjwNYcqQ0Hy5La36WeZmEAdwjzSDqwgYwuel+XGWXOORLCMg==");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "00000000-0000-0000-0000-200000000000",
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEMKA9ffT2jnNfXIYsnq8ChL6FRAYve1zRYMD15iPDQ9CQPqhefzXS/4P9FjpP5UXfQ==");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "00000000-0000-0000-0000-300000000000",
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEHbhDbHPGOTwyxnGY5lV6/13vCE8b1FPi1AQGtE4kzCJYccfEaJpJkeu3zW4mDZsQA==");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "00000000-0000-0000-0000-400000000000",
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEGLd18U8A3hStQanP+EucXpvoo8EdYKDLseQsHVQI5dDDsHgsjuLN/903+gXvTjISw==");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "00000000-0000-0000-0000-500000000000",
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEHO61Deoh/dVBN7/1Qd1gp1BxqOwlbIDnyvp6ONZ9rGlE1X+c7PI4iiZ+7Zwo3LKoA==");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000021"),
                column: "IsDangerous",
                value: false);

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000022"),
                column: "IsDangerous",
                value: false);

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000023"),
                column: "IsDangerous",
                value: false);

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000024"),
                columns: new[] { "IsDangerous", "Name" },
                values: new object[] { false, "Training" });
        }
    }
}
