using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace TeamEdge.Migrations
{
    public partial class isPicture : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TestCases");

            migrationBuilder.DropTable(
                name: "WorkItemHistories");

            migrationBuilder.DropColumn(
                name: "DescriptionCode",
                table: "WorkItemDescriptions");

            migrationBuilder.DropColumn(
                name: "AcceptenceCriteriaCode",
                table: "UserStories");

            migrationBuilder.DropColumn(
                name: "Risk",
                table: "UserStories");

            migrationBuilder.DropColumn(
                name: "Json",
                table: "Comments");

            migrationBuilder.RenameColumn(
                name: "FilePath",
                table: "Files",
                newName: "Path");

            migrationBuilder.AddColumn<bool>(
                name: "IsPicture",
                table: "Files",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "AcceptenceCriteria",
                table: "Features",
                nullable: true);

            migrationBuilder.AddColumn<byte>(
                name: "Risk",
                table: "Features",
                nullable: false,
                defaultValue: (byte)0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsPicture",
                table: "Files");

            migrationBuilder.DropColumn(
                name: "AcceptenceCriteria",
                table: "Features");

            migrationBuilder.DropColumn(
                name: "Risk",
                table: "Features");

            migrationBuilder.RenameColumn(
                name: "Path",
                table: "Files",
                newName: "FilePath");

            migrationBuilder.AddColumn<string>(
                name: "DescriptionCode",
                table: "WorkItemDescriptions",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AcceptenceCriteriaCode",
                table: "UserStories",
                nullable: true);

            migrationBuilder.AddColumn<byte>(
                name: "Risk",
                table: "UserStories",
                nullable: false,
                defaultValue: (byte)0);

            migrationBuilder.AddColumn<string>(
                name: "Json",
                table: "Comments",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "TestCases",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreatorId = table.Column<int>(nullable: false),
                    DateOfCreation = table.Column<DateTime>(nullable: false),
                    ObjectId = table.Column<string>(maxLength: 64, nullable: true),
                    WorkItemId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TestCases", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TestCases_AspNetUsers_CreatorId",
                        column: x => x.CreatorId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TestCases_WorkItemDescriptions_WorkItemId",
                        column: x => x.WorkItemId,
                        principalTable: "WorkItemDescriptions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "WorkItemHistories",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreatorId = table.Column<int>(nullable: false),
                    DateOfCreation = table.Column<DateTime>(nullable: false),
                    ObjectId = table.Column<string>(maxLength: 64, nullable: true),
                    WorkItemId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkItemHistories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WorkItemHistories_AspNetUsers_CreatorId",
                        column: x => x.CreatorId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WorkItemHistories_WorkItemDescriptions_WorkItemId",
                        column: x => x.WorkItemId,
                        principalTable: "WorkItemDescriptions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TestCases_CreatorId",
                table: "TestCases",
                column: "CreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_TestCases_WorkItemId",
                table: "TestCases",
                column: "WorkItemId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkItemHistories_CreatorId",
                table: "WorkItemHistories",
                column: "CreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkItemHistories_WorkItemId",
                table: "WorkItemHistories",
                column: "WorkItemId");
        }
    }
}
