using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace vega.Migrations
{
    public partial class SeedCustomerTitle : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("INSERT INTO Title (Name, LastUpdate) VALUES ('Mr', getdate())");
            migrationBuilder.Sql("INSERT INTO Title (Name, LastUpdate) VALUES ('Mrs', getdate())");
            migrationBuilder.Sql("INSERT INTO Title (Name, LastUpdate) VALUES ('Ms', getdate())");
            migrationBuilder.Sql("INSERT INTO Title (Name, LastUpdate) VALUES ('Dr', getdate())");
            migrationBuilder.Sql("INSERT INTO Title (Name, LastUpdate) VALUES ('Sir', getdate())");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
