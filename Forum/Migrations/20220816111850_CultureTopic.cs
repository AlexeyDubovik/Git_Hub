using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Forum.Migrations
{
    public partial class CultureTopic : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "ID",
                keyValue: new Guid("c0145821-b675-4c19-8697-9f97f9dd2f22"));

            migrationBuilder.AddColumn<string>(
                name: "Culture",
                table: "Topics",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "ID", "Avatar", "Email", "LogMoment", "Login", "PassHash", "PassSalt", "RealName", "RegMoment" },
                values: new object[] { new Guid("0e694724-b7ec-482d-9364-9f0a43c1e504"), "default", "default", null, "Admin", "default", "default", "Admin", new DateTime(2022, 8, 16, 14, 18, 49, 790, DateTimeKind.Local).AddTicks(5925) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "ID",
                keyValue: new Guid("0e694724-b7ec-482d-9364-9f0a43c1e504"));

            migrationBuilder.DropColumn(
                name: "Culture",
                table: "Topics");

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "ID", "Avatar", "Email", "LogMoment", "Login", "PassHash", "PassSalt", "RealName", "RegMoment" },
                values: new object[] { new Guid("c0145821-b675-4c19-8697-9f97f9dd2f22"), "default", "default", null, "Admin", "default", "default", "Admin", new DateTime(2022, 8, 12, 14, 7, 23, 25, DateTimeKind.Local).AddTicks(4775) });
        }
    }
}
