using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace vega.Migrations
{
    public partial class CustomerTitleLookup : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Customers_Title_CustomerContact_TitleId",
                table: "Customers");

            migrationBuilder.DropForeignKey(
                name: "FK_PlanningApps_Title_Developer_TitleId",
                table: "PlanningApps");

            migrationBuilder.DropForeignKey(
                name: "FK_Vehicles_Title_Contact_TitleId",
                table: "Vehicles");

            migrationBuilder.DropIndex(
                name: "IX_Vehicles_Contact_TitleId",
                table: "Vehicles");

            migrationBuilder.DropIndex(
                name: "IX_PlanningApps_Developer_TitleId",
                table: "PlanningApps");

            migrationBuilder.DropIndex(
                name: "IX_Customers_CustomerContact_TitleId",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "Contact_TitleId",
                table: "Vehicles");

            migrationBuilder.DropColumn(
                name: "Developer_TitleId",
                table: "PlanningApps");

            migrationBuilder.DropColumn(
                name: "CustomerContact_TitleId",
                table: "Customers");

            migrationBuilder.AddColumn<string>(
                name: "Contact_CustomerTitle",
                table: "Vehicles",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Contact_CustomerTitleId",
                table: "Vehicles",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Developer_CustomerTitle",
                table: "PlanningApps",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Developer_CustomerTitleId",
                table: "PlanningApps",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "CustomerContact_CustomerTitle",
                table: "Customers",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CustomerContact_CustomerTitleId",
                table: "Customers",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Contact_CustomerTitle",
                table: "Vehicles");

            migrationBuilder.DropColumn(
                name: "Contact_CustomerTitleId",
                table: "Vehicles");

            migrationBuilder.DropColumn(
                name: "Developer_CustomerTitle",
                table: "PlanningApps");

            migrationBuilder.DropColumn(
                name: "Developer_CustomerTitleId",
                table: "PlanningApps");

            migrationBuilder.DropColumn(
                name: "CustomerContact_CustomerTitle",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "CustomerContact_CustomerTitleId",
                table: "Customers");

            migrationBuilder.AddColumn<int>(
                name: "Contact_TitleId",
                table: "Vehicles",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Developer_TitleId",
                table: "PlanningApps",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CustomerContact_TitleId",
                table: "Customers",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Vehicles_Contact_TitleId",
                table: "Vehicles",
                column: "Contact_TitleId");

            migrationBuilder.CreateIndex(
                name: "IX_PlanningApps_Developer_TitleId",
                table: "PlanningApps",
                column: "Developer_TitleId");

            migrationBuilder.CreateIndex(
                name: "IX_Customers_CustomerContact_TitleId",
                table: "Customers",
                column: "CustomerContact_TitleId");

            migrationBuilder.AddForeignKey(
                name: "FK_Customers_Title_CustomerContact_TitleId",
                table: "Customers",
                column: "CustomerContact_TitleId",
                principalTable: "Title",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PlanningApps_Title_Developer_TitleId",
                table: "PlanningApps",
                column: "Developer_TitleId",
                principalTable: "Title",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Vehicles_Title_Contact_TitleId",
                table: "Vehicles",
                column: "Contact_TitleId",
                principalTable: "Title",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
