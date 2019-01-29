using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace vega.Migrations
{
    public partial class SeedPlanningFees : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("INSERT INTO Fees (Name) VALUES ('Feasibility')");
            migrationBuilder.Sql("INSERT INTO Fees (Name) VALUES ('Planning')");
            migrationBuilder.Sql("INSERT INTO Fees (Name) VALUES ('Building Regs')");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
