using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace TeamEdge.Migrations
{
    public partial class summaryTask : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SubTasks_AspNetUsers_AssignedToId",
                table: "SubTasks");

            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_Sprints_SprintId",
                table: "Tasks");

            migrationBuilder.DropIndex(
                name: "IX_SubTasks_AssignedToId",
                table: "SubTasks");

            migrationBuilder.DropColumn(
                name: "AssignedToId",
                table: "SubTasks");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "Invites");

            migrationBuilder.RenameColumn(
                name: "SprintId",
                table: "Tasks",
                newName: "SummaryTaskDescriptionId");

            migrationBuilder.RenameIndex(
                name: "IX_Tasks_SprintId",
                table: "Tasks",
                newName: "IX_Tasks_SummaryTaskDescriptionId");

            migrationBuilder.AddColumn<DateTime>(
                name: "DateFinish",
                table: "Tasks",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateStart",
                table: "Tasks",
                nullable: true);

            migrationBuilder.AddColumn<short>(
                name: "TimeSpan",
                table: "Tasks",
                nullable: true);

            migrationBuilder.AddColumn<short>(
                name: "Duration",
                table: "Sprints",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ToUserId",
                table: "Invites",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "SummaryTasks",
                columns: table => new
                {
                    DescriptionId = table.Column<int>(nullable: false),
                    Duration = table.Column<short>(nullable: true),
                    EndDate = table.Column<DateTime>(nullable: true),
                    Name = table.Column<string>(maxLength: 64, nullable: true),
                    Number = table.Column<int>(nullable: false),
                    ParentId = table.Column<int>(nullable: true),
                    StartDate = table.Column<DateTime>(nullable: true),
                    Status = table.Column<byte>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SummaryTasks", x => x.DescriptionId);
                    table.ForeignKey(
                        name: "FK_SummaryTasks_WorkItemDescriptions_DescriptionId",
                        column: x => x.DescriptionId,
                        principalTable: "WorkItemDescriptions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SummaryTasks_SummaryTasks_ParentId",
                        column: x => x.ParentId,
                        principalTable: "SummaryTasks",
                        principalColumn: "DescriptionId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SummaryTasks_ParentId",
                table: "SummaryTasks",
                column: "ParentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_SummaryTasks_SummaryTaskDescriptionId",
                table: "Tasks",
                column: "SummaryTaskDescriptionId",
                principalTable: "SummaryTasks",
                principalColumn: "DescriptionId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_SummaryTasks_SummaryTaskDescriptionId",
                table: "Tasks");

            migrationBuilder.DropTable(
                name: "SummaryTasks");

            migrationBuilder.DropColumn(
                name: "DateFinish",
                table: "Tasks");

            migrationBuilder.DropColumn(
                name: "DateStart",
                table: "Tasks");

            migrationBuilder.DropColumn(
                name: "TimeSpan",
                table: "Tasks");

            migrationBuilder.DropColumn(
                name: "Duration",
                table: "Sprints");

            migrationBuilder.RenameColumn(
                name: "SummaryTaskDescriptionId",
                table: "Tasks",
                newName: "SprintId");

            migrationBuilder.RenameIndex(
                name: "IX_Tasks_SummaryTaskDescriptionId",
                table: "Tasks",
                newName: "IX_Tasks_SprintId");

            migrationBuilder.AddColumn<int>(
                name: "AssignedToId",
                table: "SubTasks",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ToUserId",
                table: "Invites",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Invites",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_SubTasks_AssignedToId",
                table: "SubTasks",
                column: "AssignedToId");

            migrationBuilder.AddForeignKey(
                name: "FK_SubTasks_AspNetUsers_AssignedToId",
                table: "SubTasks",
                column: "AssignedToId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_Sprints_SprintId",
                table: "Tasks",
                column: "SprintId",
                principalTable: "Sprints",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
