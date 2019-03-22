using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace TeamEdge.Migrations
{
    public partial class downGrade : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserStories_Features_ParentId",
                table: "UserStories");

            migrationBuilder.DropTable(
                name: "CodeLinks");

            migrationBuilder.DropTable(
                name: "Features");

            migrationBuilder.AddColumn<int>(
                name: "EpickId",
                table: "Tasks",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SprintId",
                table: "Tasks",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_EpickId",
                table: "Tasks",
                column: "EpickId");

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_GantPreviousId",
                table: "Tasks",
                column: "GantPreviousId");

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_SprintId",
                table: "Tasks",
                column: "SprintId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_Epicks_EpickId",
                table: "Tasks",
                column: "EpickId",
                principalTable: "Epicks",
                principalColumn: "DescriptionId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_WorkItemDescriptions_GantPreviousId",
                table: "Tasks",
                column: "GantPreviousId",
                principalTable: "WorkItemDescriptions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_Sprints_SprintId",
                table: "Tasks",
                column: "SprintId",
                principalTable: "Sprints",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserStories_Epicks_ParentId",
                table: "UserStories",
                column: "ParentId",
                principalTable: "Epicks",
                principalColumn: "DescriptionId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_Epicks_EpickId",
                table: "Tasks");

            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_WorkItemDescriptions_GantPreviousId",
                table: "Tasks");

            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_Sprints_SprintId",
                table: "Tasks");

            migrationBuilder.DropForeignKey(
                name: "FK_UserStories_Epicks_ParentId",
                table: "UserStories");

            migrationBuilder.DropIndex(
                name: "IX_Tasks_EpickId",
                table: "Tasks");

            migrationBuilder.DropIndex(
                name: "IX_Tasks_GantPreviousId",
                table: "Tasks");

            migrationBuilder.DropIndex(
                name: "IX_Tasks_SprintId",
                table: "Tasks");

            migrationBuilder.DropColumn(
                name: "EpickId",
                table: "Tasks");

            migrationBuilder.DropColumn(
                name: "SprintId",
                table: "Tasks");

            migrationBuilder.CreateTable(
                name: "CodeLinks",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Branch = table.Column<string>(nullable: true),
                    WorkItemId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CodeLinks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CodeLinks_WorkItemDescriptions_WorkItemId",
                        column: x => x.WorkItemId,
                        principalTable: "WorkItemDescriptions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Features",
                columns: table => new
                {
                    DescriptionId = table.Column<int>(nullable: false),
                    AcceptenceCriteria = table.Column<string>(nullable: true),
                    Name = table.Column<string>(maxLength: 64, nullable: true),
                    Number = table.Column<int>(nullable: false),
                    ParentId = table.Column<int>(nullable: true),
                    Risk = table.Column<byte>(nullable: false),
                    Status = table.Column<byte>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Features", x => x.DescriptionId);
                    table.ForeignKey(
                        name: "FK_Features_WorkItemDescriptions_DescriptionId",
                        column: x => x.DescriptionId,
                        principalTable: "WorkItemDescriptions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Features_Epicks_ParentId",
                        column: x => x.ParentId,
                        principalTable: "Epicks",
                        principalColumn: "DescriptionId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CodeLinks_WorkItemId",
                table: "CodeLinks",
                column: "WorkItemId");

            migrationBuilder.CreateIndex(
                name: "IX_Features_ParentId",
                table: "Features",
                column: "ParentId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserStories_Features_ParentId",
                table: "UserStories",
                column: "ParentId",
                principalTable: "Features",
                principalColumn: "DescriptionId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
