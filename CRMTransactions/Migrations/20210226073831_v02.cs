using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CRMTransactions.Migrations
{
    public partial class v02 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CallStatus",
                table: "ValidCalls",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "FollowUpTime",
                table: "ValidCalls",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDateTime",
                table: "ValidCalls",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "UpdatedUser",
                table: "ValidCalls",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CustomerName",
                table: "MissedCalls",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsWhiteListed",
                table: "MissedCalls",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CallStatus",
                table: "ValidCalls");

            migrationBuilder.DropColumn(
                name: "FollowUpTime",
                table: "ValidCalls");

            migrationBuilder.DropColumn(
                name: "UpdatedDateTime",
                table: "ValidCalls");

            migrationBuilder.DropColumn(
                name: "UpdatedUser",
                table: "ValidCalls");

            migrationBuilder.DropColumn(
                name: "CustomerName",
                table: "MissedCalls");

            migrationBuilder.DropColumn(
                name: "IsWhiteListed",
                table: "MissedCalls");
        }
    }
}
