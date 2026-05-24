-- Add DeliveryId and DeliveryStatus columns to Order table for Phase 14 delivery integration
-- Created: 2026-05-04

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Order]') AND name = 'DeliveryId')
BEGIN
    ALTER TABLE [dbo].[Order]
    ADD [DeliveryId] NVARCHAR(100) NULL;

    PRINT 'DeliveryId column added to Order table';
END

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Order]') AND name = 'DeliveryStatus')
BEGIN
    ALTER TABLE [dbo].[Order]
    ADD [DeliveryStatus] NVARCHAR(50) NULL;

    PRINT 'DeliveryStatus column added to Order table';
END

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_Order_DeliveryId' AND object_id = OBJECT_ID('[dbo].[Order]'))
BEGIN
    CREATE INDEX IX_Order_DeliveryId ON [dbo].[Order] ([DeliveryId]);
    PRINT 'Index IX_Order_DeliveryId created';
END
