using Microsoft.EntityFrameworkCore.Migrations;

namespace vega.Migrations
{
    public partial class RemoveSingleUseGenerator : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PlanningApps_StateInitialisers_StateInitialiserId",
                table: "PlanningApps");

            migrationBuilder.DropIndex(
                name: "IX_PlanningApps_StateInitialiserId",
                table: "PlanningApps");

            migrationBuilder.DropColumn(
                name: "StateInitialiserId",
                table: "PlanningApps");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "StateInitialiserId",
                table: "PlanningApps",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_PlanningApps_StateInitialiserId",
                table: "PlanningApps",
                column: "StateInitialiserId");

            migrationBuilder.AddForeignKey(
                name: "FK_PlanningApps_StateInitialisers_StateInitialiserId",
                table: "PlanningApps",
                column: "StateInitialiserId",
                principalTable: "StateInitialisers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
