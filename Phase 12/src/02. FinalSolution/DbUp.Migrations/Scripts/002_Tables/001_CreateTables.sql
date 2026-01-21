-- Create core tables for Pezza application
-- Created: 2025-10-30

-- Customer table
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Customer')
BEGIN
    CREATE TABLE [dbo].[Customer]
    (
        [Id] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY DEFAULT NEWID(),
        [Address] NVARCHAR(500) NULL,
        [City] NVARCHAR(100) NULL,
        [Email] NVARCHAR(256) NULL,
        [FirstName] NVARCHAR(100) NOT NULL,
        [LastName] NVARCHAR(100) NOT NULL,
        [Phone] NVARCHAR(20) NULL,
        [ZipCode] NVARCHAR(10) NULL,
        [CreatedDate] DATETIME NOT NULL DEFAULT GETUTCDATE(),
        [UpdatedDate] DATETIME NULL
    );
    
    CREATE INDEX IX_Customer_Email ON [dbo].[Customer] ([Email]);
    CREATE INDEX IX_Customer_FirstName ON [dbo].[Customer] ([FirstName]);
    
    PRINT 'Customer table created successfully';
END

-- Pizza table
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Pizza')
BEGIN
    CREATE TABLE [dbo].[Pizza]
    (
        [Id] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY DEFAULT NEWID(),
        [Description] NVARCHAR(500) NULL,
        [Name] NVARCHAR(200) NOT NULL,
        [Price] DECIMAL(10, 2) NOT NULL,
        [PictureUrl] NVARCHAR(500) NULL,
        [Offer] DECIMAL(10, 2) NULL,
        [OfferEnds] DATETIME NULL,
        [OfferStarts] DATETIME NULL,
        [CreatedDate] DATETIME NOT NULL DEFAULT GETUTCDATE(),
        [UpdatedDate] DATETIME NULL
    );
    
    CREATE INDEX IX_Pizza_Name ON [dbo].[Pizza] ([Name]);
    
    PRINT 'Pizza table created successfully';
END

-- Notify table
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Notify')
BEGIN
    CREATE TABLE [dbo].[Notify]
    (
        [Id] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY DEFAULT NEWID(),
        [Title] NVARCHAR(200) NOT NULL,
        [Message] NVARCHAR(MAX) NOT NULL,
        [IsRead] BIT NOT NULL DEFAULT 0,
        [CreatedDate] DATETIME NOT NULL DEFAULT GETUTCDATE()
    );
    
    CREATE INDEX IX_Notify_IsRead ON [dbo].[Notify] ([IsRead]);
    
    PRINT 'Notify table created successfully';
END

-- Order table
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Order')
BEGIN
    CREATE TABLE [dbo].[Order]
    (
        [Id] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY DEFAULT NEWID(),
        [CustomerId] UNIQUEIDENTIFIER NOT NULL,
        [OrderDate] DATETIME NOT NULL DEFAULT GETUTCDATE(),
        [OrderNumber] NVARCHAR(50) NOT NULL UNIQUE,
        [Total] DECIMAL(10, 2) NOT NULL,
        [Completed] DATETIME NULL,
        [CreatedDate] DATETIME NOT NULL DEFAULT GETUTCDATE(),
        [UpdatedDate] DATETIME NULL,
        CONSTRAINT FK_Order_Customer FOREIGN KEY ([CustomerId]) REFERENCES [dbo].[Customer] ([Id]) ON DELETE CASCADE
    );
    
    CREATE INDEX IX_Order_CustomerId ON [dbo].[Order] ([CustomerId]);
    CREATE INDEX IX_Order_OrderDate ON [dbo].[Order] ([OrderDate]);
    CREATE INDEX IX_Order_OrderNumber ON [dbo].[Order] ([OrderNumber]);
    
    PRINT 'Order table created successfully';
END

-- OrderItem table
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'OrderItem')
BEGIN
    CREATE TABLE [dbo].[OrderItem]
    (
        [Id] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY DEFAULT NEWID(),
        [OrderId] UNIQUEIDENTIFIER NOT NULL,
        [PizzaId] UNIQUEIDENTIFIER NOT NULL,
        [Quantity] INT NOT NULL,
        [UnitPrice] DECIMAL(10, 2) NOT NULL,
        [Total] DECIMAL(10, 2) NOT NULL,
        [CreatedDate] DATETIME NOT NULL DEFAULT GETUTCDATE(),
        CONSTRAINT FK_OrderItem_Order FOREIGN KEY ([OrderId]) REFERENCES [dbo].[Order] ([Id]) ON DELETE CASCADE,
        CONSTRAINT FK_OrderItem_Pizza FOREIGN KEY ([PizzaId]) REFERENCES [dbo].[Pizza] ([Id])
    );
    
    CREATE INDEX IX_OrderItem_OrderId ON [dbo].[OrderItem] ([OrderId]);
    CREATE INDEX IX_OrderItem_PizzaId ON [dbo].[OrderItem] ([PizzaId]);
    
    PRINT 'OrderItem table created successfully';
END

-- Stock table
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Stock')
BEGIN
    CREATE TABLE [dbo].[Stock]
    (
        [Id] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY DEFAULT NEWID(),
        [PizzaId] UNIQUEIDENTIFIER NOT NULL,
        [Quantity] INT NOT NULL DEFAULT 0,
        [ReorderLevel] INT NOT NULL DEFAULT 10,
        [CreatedDate] DATETIME NOT NULL DEFAULT GETUTCDATE(),
        [UpdatedDate] DATETIME NULL,
        CONSTRAINT FK_Stock_Pizza FOREIGN KEY ([PizzaId]) REFERENCES [dbo].[Pizza] ([Id]) ON DELETE CASCADE
    );
    
    CREATE INDEX IX_Stock_PizzaId ON [dbo].[Stock] ([PizzaId]);
    
    PRINT 'Stock table created successfully';
END
