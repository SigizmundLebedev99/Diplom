using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace TeamEdge.Migrations
{
    public partial class check1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_Epics_EpicDescriptionId",
                table: "Tasks");

            migrationBuilder.DropIndex(
                name: "IX_Tasks_EpicDescriptionId",
                table: "Tasks");

            migrationBuilder.DropColumn(
                name: "RepoRole",
                table: "UserProjects");

            migrationBuilder.DropColumn(
                name: "EpicDescriptionId",
                table: "Tasks");

            migrationBuilder.DropColumn(
                name: "RepoRole",
                table: "Invites");

            migrationBuilder.RenameColumn(
                name: "EpickId",
                table: "Tasks",
                newName: "EpicId");

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_EpicId",
                table: "Tasks",
                column: "EpicId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_Epics_EpicId",
                table: "Tasks",
                column: "EpicId",
                principalTable: "Epics",
                principalColumn: "DescriptionId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_Epics_EpicId",
                table: "Tasks");

            migrationBuilder.DropIndex(
                name: "IX_Tasks_EpicId",
                table: "Tasks");

            migrationBuilder.RenameColumn(
                name: "EpicId",
                table: "Tasks",
                newName: "EpickId");

            migrationBuilder.AddColumn<int>(
                name: "RepoRole",
                table: "UserProjects",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "EpicDescriptionId",
                table: "Tasks",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RepoRole",
                table: "Invites",
                nullable: false,
                defaultValue: 0);

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
        }
    }
}
