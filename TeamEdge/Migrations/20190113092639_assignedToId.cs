using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace TeamEdge.Migrations
{
    public partial class assignedToId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "AssignedToId",
                table: "Tasks",
                nullable: true,
                oldClrType: typeof(int));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "AssignedToId",
                table: "Tasks",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);
        }
    }
}
