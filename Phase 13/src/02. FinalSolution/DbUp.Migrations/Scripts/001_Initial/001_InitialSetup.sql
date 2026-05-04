-- Initial database setup
-- Created: 2025-10-30

-- Create DeploymentHistory table for DbUp tracking
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'SchemaVersions')
BEGIN
    CREATE TABLE [dbo].[SchemaVersions]
    (
        [Id] INT NOT NULL PRIMARY KEY IDENTITY(1,1),
        [SchemaVersion] INT NOT NULL,
        [Description] NVARCHAR(255) NOT NULL,
        [Installed] DATETIME NOT NULL DEFAULT GETUTCDATE(),
        [Success] BIT NOT NULL DEFAULT 1
    );
    
    CREATE INDEX IX_SchemaVersions_SchemaVersion ON [dbo].[SchemaVersions] ([SchemaVersion]);
    
    PRINT 'SchemaVersions table created successfully';
END
ELSE
BEGIN
    PRINT 'SchemaVersions table already exists';
END
