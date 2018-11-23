using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace vega.Migrations
{
    public partial class AddDrawers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PlanningAppDrawers",
                columns: table => new
                {
                    PlanningAppId = table.Column<int>(nullable: false),
                    InternalAppUserId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlanningAppDrawers", x => new { x.PlanningAppId, x.InternalAppUserId });
                    table.ForeignKey(
                        name: "FK_PlanningAppDrawers_AppUsers_InternalAppUserId",
                        column: x => x.InternalAppUserId,
                        principalTable: "AppUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PlanningAppDrawers_PlanningApps_PlanningAppId",
                        column: x => x.PlanningAppId,
                        principalTable: "PlanningApps",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PlanningAppDrawers_InternalAppUserId",
                table: "PlanningAppDrawers",
                column: "InternalAppUserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PlanningAppDrawers");
        }
    }
}
