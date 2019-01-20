using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace TeamEdge.Migrations
{
    public partial class wiFiles : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WorkItemFiles_Files_FileId",
                table: "WorkItemFiles");

            migrationBuilder.AddForeignKey(
                name: "FK_WorkItemFiles_Files_FileId",
                table: "WorkItemFiles",
                column: "FileId",
                principalTable: "Files",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WorkItemFiles_Files_FileId",
                table: "WorkItemFiles");

            migrationBuilder.AddForeignKey(
                name: "FK_WorkItemFiles_Files_FileId",
                table: "WorkItemFiles",
                column: "FileId",
                principalTable: "Files",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
