using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace TeamEdge.Migrations
{
    public partial class doNothing : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WorkItemFiles_WorkItemDescriptions_WorkItemId",
                table: "WorkItemFiles");

            migrationBuilder.AddForeignKey(
                name: "FK_WorkItemFiles_WorkItemDescriptions_WorkItemId",
                table: "WorkItemFiles",
                column: "WorkItemId",
                principalTable: "WorkItemDescriptions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WorkItemFiles_WorkItemDescriptions_WorkItemId",
                table: "WorkItemFiles");

            migrationBuilder.AddForeignKey(
                name: "FK_WorkItemFiles_WorkItemDescriptions_WorkItemId",
                table: "WorkItemFiles",
                column: "WorkItemId",
                principalTable: "WorkItemDescriptions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
