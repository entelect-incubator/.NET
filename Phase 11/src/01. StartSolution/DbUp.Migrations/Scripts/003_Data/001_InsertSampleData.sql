-- Insert sample data for Pezza application
-- Created: 2025-10-30

-- Insert sample pizzas
IF NOT EXISTS (SELECT 1 FROM [dbo].[Pizza] WHERE [Name] = 'Margherita')
BEGIN
    INSERT INTO [dbo].[Pizza] ([Name], [Description], [Price], [Offer])
    VALUES 
        ('Margherita', 'Classic tomato, mozzarella, and basil', 9.99, NULL),
        ('Pepperoni', 'Tomato, mozzarella, and pepperoni', 11.99, 10.99),
        ('Quattro Formaggi', 'Four cheese blend', 12.99, NULL),
        ('Vegetarian', 'Fresh vegetables and cheese', 10.99, NULL),
        ('Hawaiian', 'Ham, pineapple, and cheese', 12.99, 11.99),
        ('BBQ Chicken', 'BBQ sauce, chicken, and onions', 13.99, NULL),
        ('Spicy Diavola', 'Spicy salami and chili', 12.99, NULL),
        ('Prosciutto e Rucola', 'Prosciutto and arugula', 13.99, NULL);
    
    PRINT 'Sample pizzas inserted successfully';
END
ELSE
BEGIN
    PRINT 'Sample pizzas already exist';
END

-- Insert sample customers
IF NOT EXISTS (SELECT 1 FROM [dbo].[Customer] WHERE [Email] = 'john@example.com')
BEGIN
    INSERT INTO [dbo].[Customer] ([FirstName], [LastName], [Email], [Phone], [Address], [City], [ZipCode])
    VALUES 
        ('John', 'Doe', 'john@example.com', '555-0001', '123 Main St', 'New York', '10001'),
        ('Jane', 'Smith', 'jane@example.com', '555-0002', '456 Oak Ave', 'Los Angeles', '90001'),
        ('Bob', 'Johnson', 'bob@example.com', '555-0003', '789 Pine Rd', 'Chicago', '60601'),
        ('Alice', 'Williams', 'alice@example.com', '555-0004', '321 Elm St', 'Houston', '77001');
    
    PRINT 'Sample customers inserted successfully';
END
ELSE
BEGIN
    PRINT 'Sample customers already exist';
END

-- Insert stock for each pizza
IF NOT EXISTS (SELECT 1 FROM [dbo].[Stock] WHERE [ReorderLevel] = 10)
BEGIN
    INSERT INTO [dbo].[Stock] ([PizzaId], [Quantity], [ReorderLevel])
    SELECT [Id], 50, 10 FROM [dbo].[Pizza];
    
    PRINT 'Stock inventory initialized successfully';
END
ELSE
BEGIN
    PRINT 'Stock inventory already exists';
END
