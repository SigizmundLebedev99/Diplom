using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace TeamEdge.Migrations
{
    public partial class endedBy : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "EndedById",
                table: "Timesheets",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Timesheets_EndedById",
                table: "Timesheets",
                column: "EndedById");

            migrationBuilder.AddForeignKey(
                name: "FK_Timesheets_AspNetUsers_EndedById",
                table: "Timesheets",
                column: "EndedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Timesheets_AspNetUsers_EndedById",
                table: "Timesheets");

            migrationBuilder.DropIndex(
                name: "IX_Timesheets_EndedById",
                table: "Timesheets");

            migrationBuilder.DropColumn(
                name: "EndedById",
                table: "Timesheets");
        }
    }
}
