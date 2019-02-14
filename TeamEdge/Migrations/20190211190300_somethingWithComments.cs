using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace TeamEdge.Migrations
{
    public partial class somethingWithComments : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "UserProjects",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "CommentFiles",
                columns: table => new
                {
                    FileId = table.Column<int>(nullable: false),
                    WorkItemId = table.Column<int>(nullable: false),
                    CommentId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommentFiles", x => new { x.FileId, x.WorkItemId, x.CommentId });
                    table.ForeignKey(
                        name: "FK_CommentFiles_Comments_CommentId",
                        column: x => x.CommentId,
                        principalTable: "Comments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CommentFiles_WorkItemFiles_FileId_WorkItemId",
                        columns: x => new { x.FileId, x.WorkItemId },
                        principalTable: "WorkItemFiles",
                        principalColumns: new[] { "FileId", "WorkItemId" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CommentFiles_CommentId",
                table: "CommentFiles",
                column: "CommentId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CommentFiles");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "UserProjects");
        }
    }
}
