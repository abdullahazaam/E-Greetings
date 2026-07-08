using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace E_Greetings.Migrations
{
    /// <inheritdoc />
    public partial class AddScheduling : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsScheduled",
                table: "Cards",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsSent",
                table: "Cards",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "ScheduleDate",
                table: "Cards",
                type: "datetime2",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2026, 6, 29, 23, 49, 53, 956, DateTimeKind.Local).AddTicks(9377));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2026, 6, 29, 23, 49, 53, 956, DateTimeKind.Local).AddTicks(9379));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2026, 6, 29, 23, 49, 53, 956, DateTimeKind.Local).AddTicks(9381));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: 4,
                column: "CreatedDate",
                value: new DateTime(2026, 6, 29, 23, 49, 53, 956, DateTimeKind.Local).AddTicks(9383));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsScheduled",
                table: "Cards");

            migrationBuilder.DropColumn(
                name: "IsSent",
                table: "Cards");

            migrationBuilder.DropColumn(
                name: "ScheduleDate",
                table: "Cards");

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2026, 6, 25, 15, 53, 10, 739, DateTimeKind.Local).AddTicks(2805));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2026, 6, 25, 15, 53, 10, 739, DateTimeKind.Local).AddTicks(2808));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2026, 6, 25, 15, 53, 10, 739, DateTimeKind.Local).AddTicks(2809));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: 4,
                column: "CreatedDate",
                value: new DateTime(2026, 6, 25, 15, 53, 10, 739, DateTimeKind.Local).AddTicks(2811));
        }
    }
}
