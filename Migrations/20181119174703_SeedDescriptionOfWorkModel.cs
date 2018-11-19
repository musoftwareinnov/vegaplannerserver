using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace vega.Migrations
{
    public partial class SeedDescriptionOfWorkModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("INSERT INTO DescriptionOfWork (Name, LastUpdate) VALUES ('Single Storey Rear Planning', getdate())");
            migrationBuilder.Sql("INSERT INTO DescriptionOfWork (Name, LastUpdate) VALUES ('Single Storey Rear - PD', getdate())");
            migrationBuilder.Sql("INSERT INTO DescriptionOfWork (Name, LastUpdate) VALUES ('Loft Conversions - PD', getdate())");
            migrationBuilder.Sql("INSERT INTO DescriptionOfWork (Name, LastUpdate) VALUES ('Loft Conversion Planning', getdate())");
            migrationBuilder.Sql("INSERT INTO DescriptionOfWork (Name, LastUpdate) VALUES ('Single Storey Side/Rear', getdate())");
            migrationBuilder.Sql("INSERT INTO DescriptionOfWork (Name, LastUpdate) VALUES ('Two Storey Side', getdate())");
            migrationBuilder.Sql("INSERT INTO DescriptionOfWork (Name, LastUpdate) VALUES ('Two Storey Rear', getdate())");
            migrationBuilder.Sql("INSERT INTO DescriptionOfWork (Name, LastUpdate) VALUES ('Garden Room', getdate())");
            migrationBuilder.Sql("INSERT INTO DescriptionOfWork (Name, LastUpdate) VALUES ('First Floor Side', getdate())");
            migrationBuilder.Sql("INSERT INTO DescriptionOfWork (Name, LastUpdate) VALUES ('New Build House', getdate())");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE DescriptionOfWork");
        }
    }
}
