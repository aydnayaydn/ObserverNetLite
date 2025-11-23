using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ObserverNetLite.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class HashUserPasswords : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("8e445865-a24d-4543-a6c6-9443d048cdb9"),
                column: "Password",
                value: "0192023a7bbd73250516f069df18b500");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("9e225865-a24d-4543-a6c6-9443d048cdb9"),
                column: "Password",
                value: "6ad14ba9986e3615423dfca256d04e3f");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("8e445865-a24d-4543-a6c6-9443d048cdb9"),
                column: "Password",
                value: "admin123");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("9e225865-a24d-4543-a6c6-9443d048cdb9"),
                column: "Password",
                value: "user123");
        }
    }
}
