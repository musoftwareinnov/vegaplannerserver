using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace vega.Migrations
{
    public partial class ProjectGeneratorSequence : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StateInitialisers_ProjectGenerators_ProjectGeneratorId",
                table: "StateInitialisers");

            migrationBuilder.DropIndex(
                name: "IX_StateInitialisers_ProjectGeneratorId",
                table: "StateInitialisers");

            migrationBuilder.DropColumn(
                name: "ProjectGeneratorId",
                table: "StateInitialisers");

            migrationBuilder.AddColumn<int>(
                name: "OrderId",
                table: "StateInitialisers",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<decimal>(
                name: "Amount",
                table: "PlanningAppFees",
                type: "decimal(10, 2)",
                nullable: false,
                oldClrType: typeof(decimal));

            migrationBuilder.AlterColumn<decimal>(
                name: "DefaultAmount",
                table: "Fees",
                type: "decimal(10, 2)",
                nullable: false,
                oldClrType: typeof(decimal));

            migrationBuilder.CreateTable(
                name: "ProjectGeneratorSequence",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    SeqId = table.Column<int>(nullable: false),
                    GeneratorId = table.Column<int>(nullable: true),
                    ProjectGeneratorId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectGeneratorSequence", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProjectGeneratorSequence_StateInitialisers_GeneratorId",
                        column: x => x.GeneratorId,
                        principalTable: "StateInitialisers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProjectGeneratorSequence_ProjectGenerators_ProjectGeneratorId",
                        column: x => x.ProjectGeneratorId,
                        principalTable: "ProjectGenerators",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProjectGeneratorSequence_GeneratorId",
                table: "ProjectGeneratorSequence",
                columns: new string[] { "ProjectGeneratorId", "SeqId"} );

            migrationBuilder.CreateIndex(
                name: "IX_ProjectGeneratorSequence_ProjectGeneratorId",
                table: "ProjectGeneratorSequence",
                column: "ProjectGeneratorId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProjectGeneratorSequence");

            migrationBuilder.DropColumn(
                name: "OrderId",
                table: "StateInitialisers");

            migrationBuilder.AddColumn<int>(
                name: "ProjectGeneratorId",
                table: "StateInitialisers",
                nullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "Amount",
                table: "PlanningAppFees",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(10, 2)");

            migrationBuilder.AlterColumn<decimal>(
                name: "DefaultAmount",
                table: "Fees",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(10, 2)");

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
    }
}
