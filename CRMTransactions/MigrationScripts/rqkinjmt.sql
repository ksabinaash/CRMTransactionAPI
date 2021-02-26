IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;

GO

CREATE TABLE [CallAction] (
    [Id] int NOT NULL IDENTITY,
    [Actions] nvarchar(max) NULL,
    CONSTRAINT [PK_CallAction] PRIMARY KEY ([Id])
);

GO

CREATE TABLE [CallPurpose] (
    [Id] int NOT NULL IDENTITY,
    [PurposeoftheCall] nvarchar(max) NULL,
    CONSTRAINT [PK_CallPurpose] PRIMARY KEY ([Id])
);

GO

CREATE TABLE [ValidCalls] (
    [ValidCallId] int NOT NULL IDENTITY,
    [ExternalCallId] int NULL,
    [LabName] nvarchar(max) NULL,
    [LabPhoneNumber] nvarchar(max) NULL,
    [CustomerMobileNumber] nvarchar(max) NULL,
    [EventTime] datetime2 NOT NULL,
    [CallDuration] int NOT NULL,
    [CallType] nvarchar(max) NULL,
    [CallPurpose] nvarchar(max) NULL,
    [Action] nvarchar(max) NULL,
    [Comment] nvarchar(max) NULL,
    CONSTRAINT [PK_ValidCalls] PRIMARY KEY ([ValidCallId])
);

GO

CREATE TABLE [WhiteList] (
    [Id] int NOT NULL IDENTITY,
    [MobileNumber] nvarchar(max) NULL,
    [Name] nvarchar(max) NULL,
    CONSTRAINT [PK_WhiteList] PRIMARY KEY ([Id])
);

GO

CREATE TABLE [MissedCalls] (
    [Id] int NOT NULL IDENTITY,
    [ExternalCallId] int NULL,
    [LabName] nvarchar(max) NULL,
    [LabPhoneNumber] nvarchar(max) NULL,
    [CustomerMobileNumber] nvarchar(max) NULL,
    [EventTime] datetime2 NOT NULL,
    [ValidCallId] int NULL,
    CONSTRAINT [PK_MissedCalls] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_MissedCalls_ValidCalls_ValidCallId] FOREIGN KEY ([ValidCallId]) REFERENCES [ValidCalls] ([ValidCallId]) ON DELETE NO ACTION
);

GO

CREATE INDEX [IX_MissedCalls_ValidCallId] ON [MissedCalls] ([ValidCallId]);

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20210226071138_v01', N'3.1.12');

GO

ALTER TABLE [ValidCalls] ADD [CallStatus] nvarchar(max) NULL;

GO

ALTER TABLE [ValidCalls] ADD [FollowUpTime] datetime2 NOT NULL DEFAULT '0001-01-01T00:00:00.0000000';

GO

ALTER TABLE [ValidCalls] ADD [UpdatedDateTime] datetime2 NOT NULL DEFAULT '0001-01-01T00:00:00.0000000';

GO

ALTER TABLE [ValidCalls] ADD [UpdatedUser] nvarchar(max) NULL;

GO

ALTER TABLE [MissedCalls] ADD [CustomerName] nvarchar(max) NULL;

GO

ALTER TABLE [MissedCalls] ADD [IsWhiteListed] bit NOT NULL DEFAULT CAST(0 AS bit);

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20210226073831_v02', N'3.1.12');

GO

