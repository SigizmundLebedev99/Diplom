using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace TeamEdge.Migrations
{
    public partial class subtaskmig : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AssignedToId",
                table: "SubTasks",
                nullable: true);

            migrationBuilder.AddColumn<float>(
                name: "PersentOfWork",
                table: "SubTasks",
                nullable: false,
                defaultValue: 0f);

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
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SubTasks_AspNetUsers_AssignedToId",
                table: "SubTasks");

            migrationBuilder.DropIndex(
                name: "IX_SubTasks_AssignedToId",
                table: "SubTasks");

            migrationBuilder.DropColumn(
                name: "AssignedToId",
                table: "SubTasks");

            migrationBuilder.DropColumn(
                name: "PersentOfWork",
                table: "SubTasks");
        }
    }
}
