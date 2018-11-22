using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace vega.Migrations
{
    public partial class AddSurveyors : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PlanningAppSurveyors",
                columns: table => new
                {
                    PlanningAppId = table.Column<int>(nullable: false),
                    InternalAppUserId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlanningAppSurveyors", x => new { x.PlanningAppId, x.InternalAppUserId });
                    table.ForeignKey(
                        name: "FK_PlanningAppSurveyors_AppUsers_InternalAppUserId",
                        column: x => x.InternalAppUserId,
                        principalTable: "AppUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PlanningAppSurveyors_PlanningApps_PlanningAppId",
                        column: x => x.PlanningAppId,
                        principalTable: "PlanningApps",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PlanningAppSurveyors_InternalAppUserId",
                table: "PlanningAppSurveyors",
                column: "InternalAppUserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PlanningAppSurveyors");
        }
    }
}
