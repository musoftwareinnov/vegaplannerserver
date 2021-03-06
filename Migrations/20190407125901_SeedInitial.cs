﻿using Microsoft.EntityFrameworkCore.Migrations;

namespace vega.Migrations
{
    public partial class SeedInitial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //Create Statuses
            migrationBuilder.Sql("INSERT INTO StateStatus (Name, LastUpdate, GroupType, OrderId) VALUES ('InProgress', getdate(), 'InProgress', 1)");
            migrationBuilder.Sql("INSERT INTO StateStatus (Name, LastUpdate, GroupType, OrderId) VALUES ('Overdue', getdate(), 'InProgress', 2)");
            migrationBuilder.Sql("INSERT INTO StateStatus (Name, LastUpdate, GroupType, OrderId) VALUES ('Due', getdate(), 'InProgress', 3)");
            migrationBuilder.Sql("INSERT INTO StateStatus (Name, LastUpdate, GroupType, OrderId) VALUES ('OnTime', getdate(), 'InProgress', 4)");
            migrationBuilder.Sql("INSERT INTO StateStatus (Name, LastUpdate, GroupType, OrderId) VALUES ('Not InProgress', getdate(), 'Not InProgress', 5)");
            migrationBuilder.Sql("INSERT INTO StateStatus (Name, LastUpdate, GroupType, OrderId) VALUES ('Complete', getdate(), 'Not InProgress', 6)");
            migrationBuilder.Sql("INSERT INTO StateStatus (Name, LastUpdate, GroupType, OrderId) VALUES ('Overran', getdate(), 'Not InProgress', 7)");
            migrationBuilder.Sql("INSERT INTO StateStatus (Name, LastUpdate, GroupType, OrderId) VALUES ('Terminated', getdate(), 'Not InProgress', 8)");
            migrationBuilder.Sql("INSERT INTO StateStatus (Name, LastUpdate, GroupType, OrderId) VALUES ('Archived', getdate(), 'Not InProgress', 9)");
            migrationBuilder.Sql("INSERT INTO StateStatus (Name, LastUpdate, GroupType, OrderId) VALUES ('All', getdate(), 'All', 10)");

            //Create General Generator
            migrationBuilder.Sql("INSERT INTO StateInitialisers (Name, LastUpdate) VALUES ('General', getdate())"); 
            migrationBuilder.Sql("DELETE FROM StateInitialiserState WHERE StateInitialiserId = (SELECT ID FROM Stateinitialisers WHERE Name = 'General') ");
            migrationBuilder.Sql("INSERT INTO StateInitialiserState (Name, AlertToCompletionTime, CompletionTime, LastUpdate, StateInitialiserId, OrderId, isDeleted, canDelete) VALUES ('Initial',3, 10, getdate(), (SELECT ID FROM Stateinitialisers WHERE Name = 'General'), 1, 0, 1)");
            migrationBuilder.Sql("INSERT INTO StateInitialiserState (Name, AlertToCompletionTime, CompletionTime, LastUpdate, StateInitialiserId, OrderId, isDeleted, canDelete) VALUES ('EMail Sent',2, 15, getdate(), (SELECT ID FROM Stateinitialisers WHERE Name = 'General'), 2,0, 1)");
            migrationBuilder.Sql("INSERT INTO StateInitialiserState (Name, AlertToCompletionTime, CompletionTime, LastUpdate, StateInitialiserId, OrderId, isDeleted, canDelete) VALUES ('Client Visited',3, 6, getdate(), (SELECT ID FROM Stateinitialisers WHERE Name = 'General'), 3,0, 1)");
            migrationBuilder.Sql("INSERT INTO StateInitialiserState (Name, AlertToCompletionTime, CompletionTime, LastUpdate, StateInitialiserId, OrderId, isDeleted, canDelete) VALUES ('T&C Sent',4, 10, getdate(), (SELECT ID FROM Stateinitialisers WHERE Name = 'General'), 4,0, 1)");
            migrationBuilder.Sql("INSERT INTO StateInitialiserState (Name, AlertToCompletionTime, CompletionTime, LastUpdate, StateInitialiserId, OrderId, isDeleted, canDelete) VALUES ('T&C Received',5, 12, getdate(), (SELECT ID FROM Stateinitialisers WHERE Name = 'General'), 5,0, 1)");
            migrationBuilder.Sql("INSERT INTO StateInitialiserState (Name, AlertToCompletionTime, CompletionTime, LastUpdate, StateInitialiserId, OrderId, isDeleted, canDelete) VALUES ('Acquire Application No',3, 10, getdate(), (SELECT ID FROM Stateinitialisers WHERE Name = 'General'), 6,0, 0)");
            migrationBuilder.Sql("INSERT INTO StateInitialiserState (Name, AlertToCompletionTime, CompletionTime, LastUpdate, StateInitialiserId, OrderId, isDeleted, canDelete) VALUES ('Site Survey Booked',3, 10, getdate(), (SELECT ID FROM Stateinitialisers WHERE Name = 'General'), 7,0, 1)");
            migrationBuilder.Sql("INSERT INTO StateInitialiserState (Name, AlertToCompletionTime, CompletionTime, LastUpdate, StateInitialiserId, OrderId, isDeleted, canDelete) VALUES ('Site Survey Completed',3, 10, getdate(), (SELECT ID FROM Stateinitialisers WHERE Name = 'General'), 8,0 , 1)");
            migrationBuilder.Sql("INSERT INTO StateInitialiserState (Name, AlertToCompletionTime, CompletionTime, LastUpdate, StateInitialiserId, OrderId, isDeleted, canDelete) VALUES ('Building Regs Complete',3, 10, getdate(), (SELECT ID FROM Stateinitialisers WHERE Name = 'General'), 9,0 , 1)");

            //Create Custom Fields
            migrationBuilder.Sql("INSERT INTO StateInitialiserCustomFields (Name, Type, isMandatory, isPlanningAppField) VALUES ( 'ApplicationNo', 'String', 1, 1) "); 
            migrationBuilder.Sql("INSERT INTO StateInitialiserCustomFields (Name, Type, isMandatory, isPlanningAppField) VALUES ( 'Case Officer', 'String', 1, 1) "); 
            migrationBuilder.Sql("INSERT INTO StateInitialiserCustomFields (Name, Type, isMandatory, isPlanningAppField) VALUES ( 'Booking Time', 'String', 1, 1) "); 
            migrationBuilder.Sql("INSERT INTO StateInitialiserCustomFields (Name, Type, isMandatory, isPlanningAppField) VALUES ( 'Buidling Regs 1', 'String', 0, 0) "); 
            migrationBuilder.Sql("INSERT INTO StateInitialiserCustomFields (Name, Type, isMandatory, isPlanningAppField) VALUES ( 'Buidling Regs 2', 'String', 0, 0) "); 
            migrationBuilder.Sql("INSERT INTO StateInitialiserCustomFields (Name, Type, isMandatory, isPlanningAppField) VALUES ( 'Buidling Regs 3', 'String', 0, 0) "); 
   
            migrationBuilder.Sql("INSERT INTO StateInitialiserStateCustomFields(StateInitialiserStateId, StateInitialiserCustomFieldId ) VALUES ((select id from  StateInitialiserState  where name = 'Acquire Application No'),(select id from  StateInitialiserCustomFields  where name = 'ApplicationNo'))");  
            migrationBuilder.Sql("INSERT INTO StateInitialiserStateCustomFields(StateInitialiserStateId, StateInitialiserCustomFieldId ) VALUES ((select id from  StateInitialiserState  where name = 'Building Regs Complete'),(select id from  StateInitialiserCustomFields  where name = 'Buidling Regs 1'))");  
            migrationBuilder.Sql("INSERT INTO StateInitialiserStateCustomFields(StateInitialiserStateId, StateInitialiserCustomFieldId ) VALUES ((select id from  StateInitialiserState  where name = 'Building Regs Complete'),(select id from  StateInitialiserCustomFields  where name = 'Buidling Regs 2'))");  
            migrationBuilder.Sql("INSERT INTO StateInitialiserStateCustomFields(StateInitialiserStateId, StateInitialiserCustomFieldId ) VALUES ((select id from  StateInitialiserState  where name = 'Building Regs Complete'),(select id from  StateInitialiserCustomFields  where name = 'Buidling Regs 3'))");  
  
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

            migrationBuilder.Sql("INSERT INTO Title (Name, LastUpdate) VALUES ('Mr', getdate())");
            migrationBuilder.Sql("INSERT INTO Title (Name, LastUpdate) VALUES ('Mrs', getdate())");
            migrationBuilder.Sql("INSERT INTO Title (Name, LastUpdate) VALUES ('Ms', getdate())");
            migrationBuilder.Sql("INSERT INTO Title (Name, LastUpdate) VALUES ('Dr', getdate())");
            migrationBuilder.Sql("INSERT INTO Title (Name, LastUpdate) VALUES ('Sir', getdate())");

            migrationBuilder.Sql("INSERT INTO Fees (Name, DefaultAmount) VALUES ('Feasibility', 0)");
            migrationBuilder.Sql("INSERT INTO Fees (Name, DefaultAmount) VALUES ('Planning', 0)");
            migrationBuilder.Sql("INSERT INTO Fees (Name, DefaultAmount) VALUES ('Building Regs', 0)");

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
