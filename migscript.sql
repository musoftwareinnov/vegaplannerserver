CREATE TABLE [PlanningAppSurveyors] (
    [PlanningAppId] int NOT NULL,
    [InternalAppUserId] int NOT NULL,
    CONSTRAINT [PK_PlanningAppSurveyors] PRIMARY KEY ([PlanningAppId], [InternalAppUserId]),
    CONSTRAINT [FK_PlanningAppSurveyors_AppUsers_InternalAppUserId] FOREIGN KEY ([InternalAppUserId]) REFERENCES [AppUsers] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_PlanningAppSurveyors_PlanningApps_PlanningAppId] FOREIGN KEY ([PlanningAppId]) REFERENCES [PlanningApps] ([Id]) ON DELETE CASCADE
);

GO

CREATE INDEX [IX_PlanningAppSurveyors_InternalAppUserId] ON [PlanningAppSurveyors] ([InternalAppUserId]);

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20181122110128_AddSurveyors', N'2.0.2-rtm-10011');

GO

CREATE TABLE [PlanningAppDrawers] (
    [PlanningAppId] int NOT NULL,
    [InternalAppUserId] int NOT NULL,
    CONSTRAINT [PK_PlanningAppDrawers] PRIMARY KEY ([PlanningAppId], [InternalAppUserId]),
    CONSTRAINT [FK_PlanningAppDrawers_AppUsers_InternalAppUserId] FOREIGN KEY ([InternalAppUserId]) REFERENCES [AppUsers] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_PlanningAppDrawers_PlanningApps_PlanningAppId] FOREIGN KEY ([PlanningAppId]) REFERENCES [PlanningApps] ([Id]) ON DELETE CASCADE
);

GO

CREATE INDEX [IX_PlanningAppDrawers_InternalAppUserId] ON [PlanningAppDrawers] ([InternalAppUserId]);

GO


