using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace vega.Migrations
{
    public partial class ProjectGenerator : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ProjectGeneratorId",
                table: "StateInitialisers",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "GeneratorOrder",
                table: "PlanningAppState",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "ProjectGenerators",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectGenerators", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_StateInitialisers_ProjectGeneratorId",
                table: "StateInitialisers",
                column: "ProjectGeneratorId");

            migrationBuilder.AddForeignKey(
                name: "FK_StateInitialisers_ProjectGenerators_ProjectGeneratorId",
                table: "StateInitialisers",
                column: "ProjectGeneratorId",
                principalTable: "ProjectGenerators",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StateInitialisers_ProjectGenerators_ProjectGeneratorId",
                table: "StateInitialisers");

            migrationBuilder.DropTable(
                name: "ProjectGenerators");

            migrationBuilder.DropIndex(
                name: "IX_StateInitialisers_ProjectGeneratorId",
                table: "StateInitialisers");

            migrationBuilder.DropColumn(
                name: "ProjectGeneratorId",
                table: "StateInitialisers");

            migrationBuilder.DropColumn(
                name: "GeneratorOrder",
                table: "PlanningAppState");
        }
    }
}
