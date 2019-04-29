using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace TeamEdge.Migrations
{
    public partial class comment_Files : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CommentFiles_WorkItemFiles_FileId_WorkItemId",
                table: "CommentFiles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CommentFiles",
                table: "CommentFiles");

            migrationBuilder.DropColumn(
                name: "WorkItemId",
                table: "CommentFiles");

            migrationBuilder.AddColumn<int>(
                name: "WorkItemFileFileId",
                table: "CommentFiles",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "WorkItemFileWorkItemId",
                table: "CommentFiles",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_CommentFiles",
                table: "CommentFiles",
                columns: new[] { "FileId", "CommentId" });

            migrationBuilder.CreateIndex(
                name: "IX_CommentFiles_WorkItemFileFileId_WorkItemFileWorkItemId",
                table: "CommentFiles",
                columns: new[] { "WorkItemFileFileId", "WorkItemFileWorkItemId" });

            migrationBuilder.AddForeignKey(
                name: "FK_CommentFiles_Files_FileId",
                table: "CommentFiles",
                column: "FileId",
                principalTable: "Files",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CommentFiles_WorkItemFiles_WorkItemFileFileId_WorkItemFileWorkItemId",
                table: "CommentFiles",
                columns: new[] { "WorkItemFileFileId", "WorkItemFileWorkItemId" },
                principalTable: "WorkItemFiles",
                principalColumns: new[] { "FileId", "WorkItemId" },
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CommentFiles_Files_FileId",
                table: "CommentFiles");

            migrationBuilder.DropForeignKey(
                name: "FK_CommentFiles_WorkItemFiles_WorkItemFileFileId_WorkItemFileWorkItemId",
                table: "CommentFiles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CommentFiles",
                table: "CommentFiles");

            migrationBuilder.DropIndex(
                name: "IX_CommentFiles_WorkItemFileFileId_WorkItemFileWorkItemId",
                table: "CommentFiles");

            migrationBuilder.DropColumn(
                name: "WorkItemFileFileId",
                table: "CommentFiles");

            migrationBuilder.DropColumn(
                name: "WorkItemFileWorkItemId",
                table: "CommentFiles");

            migrationBuilder.AddColumn<int>(
                name: "WorkItemId",
                table: "CommentFiles",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_CommentFiles",
                table: "CommentFiles",
                columns: new[] { "FileId", "WorkItemId", "CommentId" });

            migrationBuilder.AddForeignKey(
                name: "FK_CommentFiles_WorkItemFiles_FileId_WorkItemId",
                table: "CommentFiles",
                columns: new[] { "FileId", "WorkItemId" },
                principalTable: "WorkItemFiles",
                principalColumns: new[] { "FileId", "WorkItemId" },
                onDelete: ReferentialAction.Cascade);
        }
    }
}
