IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
CREATE TABLE [Customers] (
    [Id] int NOT NULL IDENTITY,
    [Name] nvarchar(100) NOT NULL,
    [Email] nvarchar(100) NOT NULL,
    [Status] nvarchar(50) NOT NULL,
    CONSTRAINT [PK_Customers] PRIMARY KEY ([Id])
);

CREATE TABLE [Servers] (
    [Id] int NOT NULL IDENTITY,
    [Name] nvarchar(100) NOT NULL,
    [isOnline] bit NOT NULL,
    CONSTRAINT [PK_Servers] PRIMARY KEY ([Id])
);

CREATE TABLE [Orders] (
    [Id] int NOT NULL IDENTITY,
    [CustomerId] int NOT NULL,
    [Total] decimal(18,2) NOT NULL,
    [Placed] datetime2 NOT NULL,
    [Completed] datetime2 NULL,
    CONSTRAINT [PK_Orders] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Orders_Customers_CustomerId] FOREIGN KEY ([CustomerId]) REFERENCES [Customers] ([Id]) ON DELETE CASCADE
);

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Email', N'Name', N'Status') AND [object_id] = OBJECT_ID(N'[Customers]'))
    SET IDENTITY_INSERT [Customers] ON;
INSERT INTO [Customers] ([Id], [Email], [Name], [Status])
VALUES (1, N'john.doe@example.com', N'John Doe', N'Active'),
(2, N'jane.smith@example.com', N'Jane Smith', N'Active');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Email', N'Name', N'Status') AND [object_id] = OBJECT_ID(N'[Customers]'))
    SET IDENTITY_INSERT [Customers] OFF;

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Name', N'isOnline') AND [object_id] = OBJECT_ID(N'[Servers]'))
    SET IDENTITY_INSERT [Servers] ON;
INSERT INTO [Servers] ([Id], [Name], [isOnline])
VALUES (1, N'Production Server', CAST(1 AS bit)),
(2, N'Backup Server', CAST(1 AS bit)),
(3, N'Development Server', CAST(0 AS bit)),
(4, N'Testing Server', CAST(1 AS bit)),
(5, N'Staging Server', CAST(0 AS bit));
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Name', N'isOnline') AND [object_id] = OBJECT_ID(N'[Servers]'))
    SET IDENTITY_INSERT [Servers] OFF;

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Completed', N'CustomerId', N'Placed', N'Total') AND [object_id] = OBJECT_ID(N'[Orders]'))
    SET IDENTITY_INSERT [Orders] ON;
INSERT INTO [Orders] ([Id], [Completed], [CustomerId], [Placed], [Total])
VALUES (1, '2024-06-16T00:00:00.0000000', 1, '2024-06-15T00:00:00.0000000', 150.99),
(2, '2024-07-01T00:00:00.0000000', 1, '2024-06-30T00:00:00.0000000', 89.5);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Completed', N'CustomerId', N'Placed', N'Total') AND [object_id] = OBJECT_ID(N'[Orders]'))
    SET IDENTITY_INSERT [Orders] OFF;

CREATE INDEX [IX_Orders_CustomerId] ON [Orders] ([CustomerId]);

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20250714190435_InitialCreate', N'9.0.7');

DELETE FROM [Customers]
WHERE [Id] = 2;
SELECT @@ROWCOUNT;


DELETE FROM [Orders]
WHERE [Id] = 1;
SELECT @@ROWCOUNT;


DELETE FROM [Orders]
WHERE [Id] = 2;
SELECT @@ROWCOUNT;


DELETE FROM [Customers]
WHERE [Id] = 1;
SELECT @@ROWCOUNT;


INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20250715103418_RemoveSeedDataFromModel', N'9.0.7');

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20250715141656_UpdateCustomerOrderRelationship', N'9.0.7');

COMMIT;
GO

