using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace vega.Migrations
{
    public partial class AddCityCountyAddress : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DevelopmentAddress_AddressLine2",
                table: "PlanningApps",
                newName: "DevelopmentAddress_County");

            migrationBuilder.RenameColumn(
                name: "CustomerAddress_AddressLine2",
                table: "Customers",
                newName: "CustomerAddress_County");

            migrationBuilder.AddColumn<string>(
                name: "DevelopmentAddress_City",
                table: "PlanningApps",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CustomerAddress_City",
                table: "Customers",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DevelopmentAddress_City",
                table: "PlanningApps");

            migrationBuilder.DropColumn(
                name: "CustomerAddress_City",
                table: "Customers");

            migrationBuilder.RenameColumn(
                name: "DevelopmentAddress_County",
                table: "PlanningApps",
                newName: "DevelopmentAddress_AddressLine2");

            migrationBuilder.RenameColumn(
                name: "CustomerAddress_County",
                table: "Customers",
                newName: "CustomerAddress_AddressLine2");
        }
    }
}
