using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace TeamEdge.Migrations
{
    public partial class number : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Duration",
                table: "Tasks");

            migrationBuilder.DropColumn(
                name: "Duration",
                table: "Sprints");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Sprints");

            migrationBuilder.AddColumn<int>(
                name: "Number",
                table: "Sprints",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Number",
                table: "Sprints");

            migrationBuilder.AddColumn<short>(
                name: "Duration",
                table: "Tasks",
                nullable: true);

            migrationBuilder.AddColumn<short>(
                name: "Duration",
                table: "Sprints",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Sprints",
                maxLength: 64,
                nullable: true);
        }
    }
}
