using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace TeamEdge.Migrations
{
    public partial class timesheet : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_WorkItemDescriptions_GantPreviousId",
                table: "Tasks");

            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_SummaryTasks_ParentSummaryTaskId",
                table: "Tasks");

            migrationBuilder.DropTable(
                name: "SummaryTasks");

            migrationBuilder.DropIndex(
                name: "IX_Tasks_GantPreviousId",
                table: "Tasks");

            migrationBuilder.DropIndex(
                name: "IX_Tasks_ParentSummaryTaskId",
                table: "Tasks");

            migrationBuilder.DropColumn(
                name: "EndDate",
                table: "Tasks");

            migrationBuilder.DropColumn(
                name: "GantPreviousId",
                table: "Tasks");

            migrationBuilder.DropColumn(
                name: "ParentSummaryTaskId",
                table: "Tasks");

            migrationBuilder.DropColumn(
                name: "StartDate",
                table: "Tasks");

            migrationBuilder.CreateTable(
                name: "Timesheets",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreatorId = table.Column<int>(nullable: false),
                    DateOfCreation = table.Column<DateTime>(nullable: false),
                    EndDate = table.Column<DateTime>(nullable: true),
                    EndsWith = table.Column<byte>(nullable: false),
                    StartDate = table.Column<DateTime>(nullable: false),
                    SubTaskId = table.Column<int>(nullable: true),
                    TaskId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Timesheets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Timesheets_AspNetUsers_CreatorId",
                        column: x => x.CreatorId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Timesheets_SubTasks_SubTaskId",
                        column: x => x.SubTaskId,
                        principalTable: "SubTasks",
                        principalColumn: "DescriptionId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Timesheets_Tasks_TaskId",
                        column: x => x.TaskId,
                        principalTable: "Tasks",
                        principalColumn: "DescriptionId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Timesheets_CreatorId",
                table: "Timesheets",
                column: "CreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_Timesheets_SubTaskId",
                table: "Timesheets",
                column: "SubTaskId");

            migrationBuilder.CreateIndex(
                name: "IX_Timesheets_TaskId",
                table: "Timesheets",
                column: "TaskId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Timesheets");

            migrationBuilder.AddColumn<DateTime>(
                name: "EndDate",
                table: "Tasks",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "GantPreviousId",
                table: "Tasks",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ParentSummaryTaskId",
                table: "Tasks",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "StartDate",
                table: "Tasks",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "SummaryTasks",
                columns: table => new
                {
                    DescriptionId = table.Column<int>(nullable: false),
                    Duration = table.Column<short>(nullable: true),
                    EndDate = table.Column<DateTime>(nullable: true),
                    GauntPreviousId = table.Column<int>(nullable: true),
                    Name = table.Column<string>(maxLength: 64, nullable: true),
                    Number = table.Column<int>(nullable: false),
                    ParentId = table.Column<int>(nullable: true),
                    ParentSummaryTaskId = table.Column<int>(nullable: true),
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
                        name: "FK_SummaryTasks_WorkItemDescriptions_GauntPreviousId",
                        column: x => x.GauntPreviousId,
                        principalTable: "WorkItemDescriptions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_GantPreviousId",
                table: "Tasks",
                column: "GantPreviousId");

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_ParentSummaryTaskId",
                table: "Tasks",
                column: "ParentSummaryTaskId");

            migrationBuilder.CreateIndex(
                name: "IX_SummaryTasks_GauntPreviousId",
                table: "SummaryTasks",
                column: "GauntPreviousId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_WorkItemDescriptions_GantPreviousId",
                table: "Tasks",
                column: "GantPreviousId",
                principalTable: "WorkItemDescriptions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_SummaryTasks_ParentSummaryTaskId",
                table: "Tasks",
                column: "ParentSummaryTaskId",
                principalTable: "SummaryTasks",
                principalColumn: "DescriptionId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
