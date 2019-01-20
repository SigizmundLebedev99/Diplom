using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace TeamEdge.Migrations
{
    public partial class deleteRepoTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Projects_Repositories_RepositoryId",
                table: "Projects");

            migrationBuilder.DropTable(
                name: "Repositories");

            migrationBuilder.DropIndex(
                name: "IX_Projects_RepositoryId",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "RepositoryId",
                table: "Projects");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RepositoryId",
                table: "Projects",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Repositories",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreatorId = table.Column<int>(nullable: false),
                    DateOfCreation = table.Column<DateTime>(nullable: false),
                    ProjectId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Repositories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Repositories_AspNetUsers_CreatorId",
                        column: x => x.CreatorId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Repositories_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Projects_RepositoryId",
                table: "Projects",
                column: "RepositoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Repositories_CreatorId",
                table: "Repositories",
                column: "CreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_Repositories_ProjectId",
                table: "Repositories",
                column: "ProjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_Projects_Repositories_RepositoryId",
                table: "Projects",
                column: "RepositoryId",
                principalTable: "Repositories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
