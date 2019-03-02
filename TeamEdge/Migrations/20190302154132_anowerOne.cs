using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace TeamEdge.Migrations
{
    public partial class anowerOne : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "GauntPreviousId",
                table: "SummaryTasks",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_SummaryTasks_GauntPreviousId",
                table: "SummaryTasks",
                column: "GauntPreviousId");

            migrationBuilder.AddForeignKey(
                name: "FK_SummaryTasks_WorkItemDescriptions_GauntPreviousId",
                table: "SummaryTasks",
                column: "GauntPreviousId",
                principalTable: "WorkItemDescriptions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SummaryTasks_WorkItemDescriptions_GauntPreviousId",
                table: "SummaryTasks");

            migrationBuilder.DropIndex(
                name: "IX_SummaryTasks_GauntPreviousId",
                table: "SummaryTasks");

            migrationBuilder.DropColumn(
                name: "GauntPreviousId",
                table: "SummaryTasks");
        }
    }
}
