﻿using Microsoft.EntityFrameworkCore.Migrations;

namespace vega.Migrations
{
    public partial class ProjectFinal : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ProjectGeneratorId",
                table: "PlanningApps",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_PlanningApps_ProjectGeneratorId",
                table: "PlanningApps",
                column: "ProjectGeneratorId");

            migrationBuilder.AddForeignKey(
                name: "FK_PlanningApps_ProjectGenerators_ProjectGeneratorId",
                table: "PlanningApps",
                column: "ProjectGeneratorId",
                principalTable: "ProjectGenerators",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PlanningApps_ProjectGenerators_ProjectGeneratorId",
                table: "PlanningApps");

            migrationBuilder.DropIndex(
                name: "IX_PlanningApps_ProjectGeneratorId",
                table: "PlanningApps");

            migrationBuilder.DropColumn(
                name: "ProjectGeneratorId",
                table: "PlanningApps");
        }
    }
}
