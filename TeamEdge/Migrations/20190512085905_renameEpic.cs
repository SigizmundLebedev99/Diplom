using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace TeamEdge.Migrations
{
    public partial class renameEpic : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_Epicks_EpickId",
                table: "Tasks");

            migrationBuilder.DropForeignKey(
                name: "FK_UserStories_Epicks_ParentId",
                table: "UserStories");

            migrationBuilder.DropTable(
                name: "Epicks");

            migrationBuilder.DropIndex(
                name: "IX_Tasks_EpickId",
                table: "Tasks");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "UserStories");

            migrationBuilder.AlterColumn<byte>(
                name: "EndsWith",
                table: "Timesheets",
                nullable: true,
                oldClrType: typeof(byte));

            migrationBuilder.AddColumn<int>(
                name: "EpicDescriptionId",
                table: "Tasks",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Epics",
                columns: table => new
                {
                    DescriptionId = table.Column<int>(nullable: false),
                    Name = table.Column<string>(maxLength: 64, nullable: true),
                    Number = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Epics", x => x.DescriptionId);
                    table.ForeignKey(
                        name: "FK_Epics_WorkItemDescriptions_DescriptionId",
                        column: x => x.DescriptionId,
                        principalTable: "WorkItemDescriptions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_EpicDescriptionId",
                table: "Tasks",
                column: "EpicDescriptionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_Epics_EpicDescriptionId",
                table: "Tasks",
                column: "EpicDescriptionId",
                principalTable: "Epics",
                principalColumn: "DescriptionId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserStories_Epics_ParentId",
                table: "UserStories",
                column: "ParentId",
                principalTable: "Epics",
                principalColumn: "DescriptionId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_Epics_EpicDescriptionId",
                table: "Tasks");

            migrationBuilder.DropForeignKey(
                name: "FK_UserStories_Epics_ParentId",
                table: "UserStories");

            migrationBuilder.DropTable(
                name: "Epics");

            migrationBuilder.DropIndex(
                name: "IX_Tasks_EpicDescriptionId",
                table: "Tasks");

            migrationBuilder.DropColumn(
                name: "EpicDescriptionId",
                table: "Tasks");

            migrationBuilder.AddColumn<byte>(
                name: "Status",
                table: "UserStories",
                nullable: false,
                defaultValue: (byte)0);

            migrationBuilder.AlterColumn<byte>(
                name: "EndsWith",
                table: "Timesheets",
                nullable: false,
                oldClrType: typeof(byte),
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "Epicks",
                columns: table => new
                {
                    DescriptionId = table.Column<int>(nullable: false),
                    Name = table.Column<string>(maxLength: 64, nullable: true),
                    Number = table.Column<int>(nullable: false),
                    Status = table.Column<byte>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Epicks", x => x.DescriptionId);
                    table.ForeignKey(
                        name: "FK_Epicks_WorkItemDescriptions_DescriptionId",
                        column: x => x.DescriptionId,
                        principalTable: "WorkItemDescriptions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_EpickId",
                table: "Tasks",
                column: "EpickId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_Epicks_EpickId",
                table: "Tasks",
                column: "EpickId",
                principalTable: "Epicks",
                principalColumn: "DescriptionId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserStories_Epicks_ParentId",
                table: "UserStories",
                column: "ParentId",
                principalTable: "Epicks",
                principalColumn: "DescriptionId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
