using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace erooms.Migrations
{
    /// <inheritdoc />
    public partial class SeedUsers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "NRP",
                table: "Users",
                newName: "Username");

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "PasswordHash", "Role", "Username" },
                values: new object[,]
                {
                    { 1, "$2a$11$Qdcaxpxif6L2isBfTTflwuiXKhJlLt3ZOKujThIigbklXRSMogCcG", "Admin", "admin" },
                    { 2, "$2a$11$j8QcO5.Xc3Rea5ERnq5bdewyrufdiDxkYWgn0QWzrfssWvv7nlIWm", "Mahasiswa", "12345" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.RenameColumn(
                name: "Username",
                table: "Users",
                newName: "NRP");
        }
    }
}
