using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace vega.Migrations
{
    public partial class SeedFees : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("INSERT INTO Fees (Name, DefaultAmount) VALUES ('Feasibility', 100)");
            migrationBuilder.Sql("INSERT INTO Fees (Name, DefaultAmount) VALUES ('Planning', 102)");
            migrationBuilder.Sql("INSERT INTO Fees (Name, DefaultAmount) VALUES ('Building Regs', 103)");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
