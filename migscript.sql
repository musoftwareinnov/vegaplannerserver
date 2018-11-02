INSERT INTO StateStatus (Name, LastUpdate, GroupType, OrderId) VALUES ('InProgress', getdate(), 'InProgress', 1)

GO

INSERT INTO StateStatus (Name, LastUpdate, GroupType, OrderId) VALUES ('Overdue', getdate(), 'InProgress', 2)

GO

INSERT INTO StateStatus (Name, LastUpdate, GroupType, OrderId) VALUES ('Due', getdate(), 'InProgress', 3)

GO

INSERT INTO StateStatus (Name, LastUpdate, GroupType, OrderId) VALUES ('OnTime', getdate(), 'InProgress', 4)

GO

INSERT INTO StateStatus (Name, LastUpdate, GroupType, OrderId) VALUES ('Not InProgress', getdate(), 'Not InProgress', 5)

GO

INSERT INTO StateStatus (Name, LastUpdate, GroupType, OrderId) VALUES ('Complete', getdate(), 'Not InProgress', 6)

GO

INSERT INTO StateStatus (Name, LastUpdate, GroupType, OrderId) VALUES ('Overran', getdate(), 'Not InProgress', 7)

GO

INSERT INTO StateStatus (Name, LastUpdate, GroupType, OrderId) VALUES ('Terminated', getdate(), 'Not InProgress', 8)

GO

INSERT INTO StateStatus (Name, LastUpdate, GroupType, OrderId) VALUES ('Archived', getdate(), 'Not InProgress', 9)

GO

INSERT INTO StateStatus (Name, LastUpdate, GroupType, OrderId) VALUES ('All', getdate(), 'All', 10)

GO

INSERT INTO Makes (Name) VALUES ('Make1')

GO

INSERT INTO Makes (Name) VALUES ('Make2')

GO

INSERT INTO Makes (Name) VALUES ('Make3')

GO

INSERT INTO Models (Name, MakeId) VALUES ('Make1-ModelA', (SELECT ID FROM Makes WHERE Name = 'Make1') )

GO

INSERT INTO Models (Name, MakeId) VALUES ('Make1-ModelB', (SELECT ID FROM Makes WHERE Name = 'Make1') )

GO

INSERT INTO Models (Name, MakeId) VALUES ('Make1-ModelC', (SELECT ID FROM Makes WHERE Name = 'Make1') )

GO

INSERT INTO Models (Name, MakeId) VALUES ('Make2-ModelA', (SELECT ID FROM Makes WHERE Name = 'Make2') )

GO

INSERT INTO Models (Name, MakeId) VALUES ('Make2-ModelB', (SELECT ID FROM Makes WHERE Name = 'Make2') )

GO

INSERT INTO Models (Name, MakeId) VALUES ('Make2-ModelC', (SELECT ID FROM Makes WHERE Name = 'Make2') )

GO

INSERT INTO Models (Name, MakeId) VALUES ('Make3-ModelA', (SELECT ID FROM Makes WHERE Name = 'Make3') )

GO

INSERT INTO Models (Name, MakeId) VALUES ('Make3-ModelB', (SELECT ID FROM Makes WHERE Name = 'Make3') )

GO

INSERT INTO Models (Name, MakeId) VALUES ('Make3-ModelC', (SELECT ID FROM Makes WHERE Name = 'Make3') )

GO

INSERT INTO Features (Name) VALUES ('Feature1')

GO

INSERT INTO Features (Name) VALUES ('Feature2')

GO

INSERT INTO Features (Name) VALUES ('Feature3')

GO

INSERT INTO StateInitialisers (Name, LastUpdate) VALUES ('General', getdate())

GO

DELETE FROM StateInitialiserState WHERE StateInitialiserId = (SELECT ID FROM Stateinitialisers WHERE Name = 'General') 

GO

INSERT INTO StateInitialiserState (Name, AlertToCompletionTime, CompletionTime, LastUpdate, StateInitialiserId, OrderId, isDeleted, canDelete) VALUES ('Initial',3, 10, getdate(), (SELECT ID FROM Stateinitialisers WHERE Name = 'General'), 1, 0, 1)

GO

INSERT INTO StateInitialiserState (Name, AlertToCompletionTime, CompletionTime, LastUpdate, StateInitialiserId, OrderId, isDeleted, canDelete) VALUES ('EMail Sent',2, 15, getdate(), (SELECT ID FROM Stateinitialisers WHERE Name = 'General'), 2,0, 1)

GO

INSERT INTO StateInitialiserState (Name, AlertToCompletionTime, CompletionTime, LastUpdate, StateInitialiserId, OrderId, isDeleted, canDelete) VALUES ('Client Visited',3, 6, getdate(), (SELECT ID FROM Stateinitialisers WHERE Name = 'General'), 3,0, 1)

GO

INSERT INTO StateInitialiserState (Name, AlertToCompletionTime, CompletionTime, LastUpdate, StateInitialiserId, OrderId, isDeleted, canDelete) VALUES ('T&C Sent',4, 10, getdate(), (SELECT ID FROM Stateinitialisers WHERE Name = 'General'), 4,0, 1)

GO

INSERT INTO StateInitialiserState (Name, AlertToCompletionTime, CompletionTime, LastUpdate, StateInitialiserId, OrderId, isDeleted, canDelete) VALUES ('T&C Received',5, 12, getdate(), (SELECT ID FROM Stateinitialisers WHERE Name = 'General'), 5,0, 1)

GO

INSERT INTO StateInitialiserState (Name, AlertToCompletionTime, CompletionTime, LastUpdate, StateInitialiserId, OrderId, isDeleted, canDelete) VALUES ('Acquire Application No',3, 10, getdate(), (SELECT ID FROM Stateinitialisers WHERE Name = 'General'), 6,0, 0)

GO

INSERT INTO StateInitialiserState (Name, AlertToCompletionTime, CompletionTime, LastUpdate, StateInitialiserId, OrderId, isDeleted, canDelete) VALUES ('Site Survey Booked',3, 10, getdate(), (SELECT ID FROM Stateinitialisers WHERE Name = 'General'), 7,0, 1)

GO

INSERT INTO StateInitialiserState (Name, AlertToCompletionTime, CompletionTime, LastUpdate, StateInitialiserId, OrderId, isDeleted, canDelete) VALUES ('Site Survey Completed',3, 10, getdate(), (SELECT ID FROM Stateinitialisers WHERE Name = 'General'), 8,0 , 1)

GO

INSERT INTO StateInitialiserState (Name, AlertToCompletionTime, CompletionTime, LastUpdate, StateInitialiserId, OrderId, isDeleted, canDelete) VALUES ('Building Regs Complete',3, 10, getdate(), (SELECT ID FROM Stateinitialisers WHERE Name = 'General'), 9,0 , 1)

GO

INSERT INTO StateInitialiserCustomFields (Name, Type, isMandatory, isPlanningAppField) VALUES ( 'ApplicationNo', 'String', 1, 1) 

GO

INSERT INTO StateInitialiserCustomFields (Name, Type, isMandatory, isPlanningAppField) VALUES ( 'Case Officer', 'String', 1, 1) 

GO

INSERT INTO StateInitialiserCustomFields (Name, Type, isMandatory, isPlanningAppField) VALUES ( 'Booking Time', 'String', 1, 1) 

GO

INSERT INTO StateInitialiserCustomFields (Name, Type, isMandatory, isPlanningAppField) VALUES ( 'Buidling Regs 1', 'String', 0, 0) 

GO

INSERT INTO StateInitialiserCustomFields (Name, Type, isMandatory, isPlanningAppField) VALUES ( 'Buidling Regs 2', 'String', 0, 0) 

GO

INSERT INTO StateInitialiserCustomFields (Name, Type, isMandatory, isPlanningAppField) VALUES ( 'Buidling Regs 3', 'String', 0, 0) 

GO

INSERT INTO StateInitialiserStateCustomFields(StateInitialiserStateId, StateInitialiserCustomFieldId ) VALUES ((select id from  StateInitialiserState  where name = 'Acquire Application No'),(select id from  StateInitialiserCustomFields  where name = 'ApplicationNo'))

GO

INSERT INTO StateInitialiserStateCustomFields(StateInitialiserStateId, StateInitialiserCustomFieldId ) VALUES ((select id from  StateInitialiserState  where name = 'Building Regs Complete'),(select id from  StateInitialiserCustomFields  where name = 'Buidling Regs 1'))

GO

INSERT INTO StateInitialiserStateCustomFields(StateInitialiserStateId, StateInitialiserCustomFieldId ) VALUES ((select id from  StateInitialiserState  where name = 'Building Regs Complete'),(select id from  StateInitialiserCustomFields  where name = 'Buidling Regs 2'))

GO

INSERT INTO StateInitialiserStateCustomFields(StateInitialiserStateId, StateInitialiserCustomFieldId ) VALUES ((select id from  StateInitialiserState  where name = 'Building Regs Complete'),(select id from  StateInitialiserCustomFields  where name = 'Buidling Regs 3'))

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20180926103542_SeedDatabase', N'2.0.2-rtm-10011');

GO

EXEC sp_rename N'PlanningApps.DevelopmentAddress_AddressLine2', N'DevelopmentAddress_County', N'COLUMN';

GO

EXEC sp_rename N'Customers.CustomerAddress_AddressLine2', N'CustomerAddress_County', N'COLUMN';

GO

ALTER TABLE [PlanningApps] ADD [DevelopmentAddress_City] nvarchar(max) NULL;

GO

ALTER TABLE [Customers] ADD [CustomerAddress_City] nvarchar(max) NULL;

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20181023105541_AddCityCountyAddress', N'2.0.2-rtm-10011');

GO

CREATE TABLE [BusinessDates] (
    [Id] int NOT NULL IDENTITY,
    [CurrBusDate] datetime2 NOT NULL,
    [NextBusDate] datetime2 NOT NULL,
    [PrevBusDate] datetime2 NOT NULL,
    CONSTRAINT [PK_BusinessDates] PRIMARY KEY ([Id])
);

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20181031120011_BusinessDates', N'2.0.2-rtm-10011');

GO

