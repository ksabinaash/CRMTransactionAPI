using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CRMTransactions.Migrations
{
    public partial class v01 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CallAction",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Actions = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CallAction", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CallPurpose",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PurposeoftheCall = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CallPurpose", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ValidCalls",
                columns: table => new
                {
                    ValidCallId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ExternalCallId = table.Column<int>(nullable: true),
                    LabName = table.Column<string>(nullable: true),
                    LabPhoneNumber = table.Column<string>(nullable: true),
                    CustomerMobileNumber = table.Column<string>(nullable: true),
                    EventTime = table.Column<DateTime>(nullable: false),
                    CallDuration = table.Column<int>(nullable: false),
                    CallType = table.Column<string>(nullable: true),
                    CallPurpose = table.Column<string>(nullable: true),
                    Action = table.Column<string>(nullable: true),
                    Comment = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ValidCalls", x => x.ValidCallId);
                });

            migrationBuilder.CreateTable(
                name: "WhiteList",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MobileNumber = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WhiteList", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MissedCalls",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ExternalCallId = table.Column<int>(nullable: true),
                    LabName = table.Column<string>(nullable: true),
                    LabPhoneNumber = table.Column<string>(nullable: true),
                    CustomerMobileNumber = table.Column<string>(nullable: true),
                    EventTime = table.Column<DateTime>(nullable: false),
                    ValidCallId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MissedCalls", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MissedCalls_ValidCalls_ValidCallId",
                        column: x => x.ValidCallId,
                        principalTable: "ValidCalls",
                        principalColumn: "ValidCallId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MissedCalls_ValidCallId",
                table: "MissedCalls",
                column: "ValidCallId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CallAction");

            migrationBuilder.DropTable(
                name: "CallPurpose");

            migrationBuilder.DropTable(
                name: "MissedCalls");

            migrationBuilder.DropTable(
                name: "WhiteList");

            migrationBuilder.DropTable(
                name: "ValidCalls");
        }
    }
}
