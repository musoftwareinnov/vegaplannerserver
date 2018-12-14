using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace vega.Migrations
{
    public partial class ChangAppUserType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PlanningAppDrawers_AppUsers_InternalAppUserId",
                table: "PlanningAppDrawers");

            migrationBuilder.DropForeignKey(
                name: "FK_PlanningAppSurveyors_AppUsers_InternalAppUserId",
                table: "PlanningAppSurveyors");

            migrationBuilder.DropTable(
                name: "AppUsers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PlanningAppSurveyors",
                table: "PlanningAppSurveyors");

            migrationBuilder.DropIndex(
                name: "IX_PlanningAppSurveyors_InternalAppUserId",
                table: "PlanningAppSurveyors");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PlanningAppDrawers",
                table: "PlanningAppDrawers");

            migrationBuilder.DropIndex(
                name: "IX_PlanningAppDrawers_InternalAppUserId",
                table: "PlanningAppDrawers");

            migrationBuilder.DropColumn(
                name: "InternalAppUserId",
                table: "PlanningAppSurveyors");

            migrationBuilder.DropColumn(
                name: "InternalAppUserId",
                table: "PlanningAppDrawers");

            migrationBuilder.AddColumn<string>(
                name: "AppUserId",
                table: "PlanningAppSurveyors",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "AppUserId",
                table: "PlanningAppDrawers",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PlanningAppSurveyors",
                table: "PlanningAppSurveyors",
                columns: new[] { "PlanningAppId", "AppUserId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_PlanningAppDrawers",
                table: "PlanningAppDrawers",
                columns: new[] { "PlanningAppId", "AppUserId" });

            migrationBuilder.CreateIndex(
                name: "IX_PlanningAppSurveyors_AppUserId",
                table: "PlanningAppSurveyors",
                column: "AppUserId");

            migrationBuilder.CreateIndex(
                name: "IX_PlanningAppDrawers_AppUserId",
                table: "PlanningAppDrawers",
                column: "AppUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_PlanningAppDrawers_AspNetUsers_AppUserId",
                table: "PlanningAppDrawers",
                column: "AppUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PlanningAppSurveyors_AspNetUsers_AppUserId",
                table: "PlanningAppSurveyors",
                column: "AppUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PlanningAppDrawers_AspNetUsers_AppUserId",
                table: "PlanningAppDrawers");

            migrationBuilder.DropForeignKey(
                name: "FK_PlanningAppSurveyors_AspNetUsers_AppUserId",
                table: "PlanningAppSurveyors");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PlanningAppSurveyors",
                table: "PlanningAppSurveyors");

            migrationBuilder.DropIndex(
                name: "IX_PlanningAppSurveyors_AppUserId",
                table: "PlanningAppSurveyors");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PlanningAppDrawers",
                table: "PlanningAppDrawers");

            migrationBuilder.DropIndex(
                name: "IX_PlanningAppDrawers_AppUserId",
                table: "PlanningAppDrawers");

            migrationBuilder.DropColumn(
                name: "AppUserId",
                table: "PlanningAppSurveyors");

            migrationBuilder.DropColumn(
                name: "AppUserId",
                table: "PlanningAppDrawers");

            migrationBuilder.AddColumn<int>(
                name: "InternalAppUserId",
                table: "PlanningAppSurveyors",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "InternalAppUserId",
                table: "PlanningAppDrawers",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_PlanningAppSurveyors",
                table: "PlanningAppSurveyors",
                columns: new[] { "PlanningAppId", "InternalAppUserId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_PlanningAppDrawers",
                table: "PlanningAppDrawers",
                columns: new[] { "PlanningAppId", "InternalAppUserId" });

            migrationBuilder.CreateTable(
                name: "AppUsers",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Gender = table.Column<string>(nullable: true),
                    IdentityId = table.Column<string>(nullable: true),
                    Locale = table.Column<string>(nullable: true),
                    Location = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppUsers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppUsers_AspNetUsers_IdentityId",
                        column: x => x.IdentityId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PlanningAppSurveyors_InternalAppUserId",
                table: "PlanningAppSurveyors",
                column: "InternalAppUserId");

            migrationBuilder.CreateIndex(
                name: "IX_PlanningAppDrawers_InternalAppUserId",
                table: "PlanningAppDrawers",
                column: "InternalAppUserId");

            migrationBuilder.CreateIndex(
                name: "IX_AppUsers_IdentityId",
                table: "AppUsers",
                column: "IdentityId");

            migrationBuilder.AddForeignKey(
                name: "FK_PlanningAppDrawers_AppUsers_InternalAppUserId",
                table: "PlanningAppDrawers",
                column: "InternalAppUserId",
                principalTable: "AppUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PlanningAppSurveyors_AppUsers_InternalAppUserId",
                table: "PlanningAppSurveyors",
                column: "InternalAppUserId",
                principalTable: "AppUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
