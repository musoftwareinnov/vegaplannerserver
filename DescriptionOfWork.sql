In Development mode
Database Connection String: server=localhost; database=vega; user id=sa; password=MyComplexPassword!1234
Business Date : 04/02/2020 00:00:00
Business Date set to :04/02/2020 00:00:00
ALTER TABLE [PlanningApps] ADD [DescriptionOfWork] nvarchar(max) NULL;

GO

CREATE TABLE [DescriptionOfWork] (
    [Id] int NOT NULL IDENTITY,
    [LastUpdate] datetime2 NOT NULL,
    [Name] nvarchar(255) NOT NULL,
    CONSTRAINT [PK_DescriptionOfWork] PRIMARY KEY ([Id])
);

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20181119174123_DescriptionOfWorkModel', N'2.0.2-rtm-10011');

GO

INSERT INTO DescriptionOfWork (Name, LastUpdate) VALUES ('Single Storey Rear Planning', getdate())

GO

INSERT INTO DescriptionOfWork (Name, LastUpdate) VALUES ('Single Storey Rear - PD', getdate())

GO

INSERT INTO DescriptionOfWork (Name, LastUpdate) VALUES ('Loft Conversions - PD', getdate())

GO

INSERT INTO DescriptionOfWork (Name, LastUpdate) VALUES ('Loft Conversion Planning', getdate())

GO

INSERT INTO DescriptionOfWork (Name, LastUpdate) VALUES ('Single Storey Side/Rear', getdate())

GO

INSERT INTO DescriptionOfWork (Name, LastUpdate) VALUES ('Two Storey Side', getdate())

GO

INSERT INTO DescriptionOfWork (Name, LastUpdate) VALUES ('Two Storey Rear', getdate())

GO

INSERT INTO DescriptionOfWork (Name, LastUpdate) VALUES ('Garden Room', getdate())

GO

INSERT INTO DescriptionOfWork (Name, LastUpdate) VALUES ('First Floor Side', getdate())

GO

INSERT INTO DescriptionOfWork (Name, LastUpdate) VALUES ('New Build House', getdate())

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20181119174703_SeedDescriptionOfWorkModel', N'2.0.2-rtm-10011');

GO


