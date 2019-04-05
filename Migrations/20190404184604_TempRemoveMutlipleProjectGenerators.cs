using Microsoft.EntityFrameworkCore.Migrations;

namespace vega.Migrations
{
    public partial class TempRemoveMutlipleProjectGenerators : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PlanningApps_ProjectGenerators_ProjectGeneratorId",
                table: "PlanningApps");

            migrationBuilder.DropIndex(
                name: "IX_PlanningApps_ProjectGeneratorId",
                table: "PlanningApps");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
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
    }
}
