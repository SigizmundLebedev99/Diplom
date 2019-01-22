using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace TeamEdge.Migrations
{
    public partial class subTasks : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_Tasks__TaskDescriptionId",
                table: "Tasks");

            migrationBuilder.DropIndex(
                name: "IX_Tasks__TaskDescriptionId",
                table: "Tasks");

            migrationBuilder.DropColumn(
                name: "_TaskDescriptionId",
                table: "Tasks");

            migrationBuilder.CreateTable(
                name: "SubTasks",
                columns: table => new
                {
                    DescriptionId = table.Column<int>(nullable: false),
                    AssignedToId = table.Column<int>(nullable: true),
                    Name = table.Column<string>(maxLength: 64, nullable: true),
                    Number = table.Column<int>(nullable: false),
                    ParentId = table.Column<int>(nullable: true),
                    Status = table.Column<byte>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubTasks", x => x.DescriptionId);
                    table.ForeignKey(
                        name: "FK_SubTasks_AspNetUsers_AssignedToId",
                        column: x => x.AssignedToId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SubTasks_WorkItemDescriptions_DescriptionId",
                        column: x => x.DescriptionId,
                        principalTable: "WorkItemDescriptions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SubTasks_Tasks_ParentId",
                        column: x => x.ParentId,
                        principalTable: "Tasks",
                        principalColumn: "DescriptionId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SubTasks_AssignedToId",
                table: "SubTasks",
                column: "AssignedToId");

            migrationBuilder.CreateIndex(
                name: "IX_SubTasks_ParentId",
                table: "SubTasks",
                column: "ParentId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SubTasks");

            migrationBuilder.AddColumn<int>(
                name: "_TaskDescriptionId",
                table: "Tasks",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Tasks__TaskDescriptionId",
                table: "Tasks",
                column: "_TaskDescriptionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_Tasks__TaskDescriptionId",
                table: "Tasks",
                column: "_TaskDescriptionId",
                principalTable: "Tasks",
                principalColumn: "DescriptionId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
