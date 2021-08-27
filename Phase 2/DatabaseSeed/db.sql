USE [master]
GO
/****** Object:  Database [PezzaDb]    Script Date: 2021/01/04 14:13:39 ******/
CREATE DATABASE [PezzaDb]
GO
USE [PezzaDb]
GO
/****** Object:  Table [dbo].[Customer]    Script Date: 2021/01/04 14:13:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Customer](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](100) NOT NULL,
	[Address] [varchar](500) NOT NULL,
	[City] [varchar](100) NOT NULL,
	[Province] [varchar](100) NOT NULL,
	[PostalCode] [varchar](8) NOT NULL,
	[Phone] [varchar](20) NOT NULL,
	[Email] [varchar](200) NOT NULL,
	[ContactPerson] [varchar](200) NOT NULL,
	[DateCreated] [datetime] NOT NULL,
 CONSTRAINT [PK__Customer__3214EC07CE5A6856] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Notify]    Script Date: 2021/01/04 14:13:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Notify](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[CustomerId] [int] NOT NULL,
	[Email] [nvarchar](max) NOT NULL,
	[Sent] [bit] NOT NULL,
	[Retry] [int] NOT NULL,
	[DateSent] [datetime] NOT NULL,
 CONSTRAINT [PK_Notify] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Order]    Script Date: 2021/01/04 14:13:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Order](
	[Id] [int] NOT NULL,
	[CustomerId] [int] NOT NULL,
	[RestaurantId] [int] NOT NULL,
	[Amount] [decimal](17, 2) NOT NULL,
	[DateCreated] [datetime] NOT NULL,
	[Completed] [bit] NOT NULL
 CONSTRAINT [PK_Order] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[OrderItem]    Script Date: 2021/01/04 14:13:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[OrderItem](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[OrderId] [int] NOT NULL,
	[ProductId] [int] NOT NULL,
	[Quantity] [int] NOT NULL,
 CONSTRAINT [PK_OrderItem] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Product]    Script Date: 2021/01/04 14:13:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Product](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](150) NOT NULL,
	[Description] [varchar](1000) NOT NULL,
	[PictureUrl] [varchar](1000) NOT NULL,
	[Price] [decimal](17, 2) NOT NULL,
	[Special] [bit] NOT NULL,
	[OfferEndDate] [datetime] NULL,
	[OfferPrice] [decimal](17, 2) NULL,
	[IsActive] [bit] NOT NULL,
	[DateCreated] [datetime] NOT NULL,
 CONSTRAINT [PK_Product] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Restaurant]    Script Date: 2021/01/04 14:13:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Restaurant](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](100) NOT NULL,
	[Description] [varchar](1000) NOT NULL,
	[PictureUrl] [varchar](1000) NOT NULL,
	[Address] [varchar](500) NOT NULL,
	[City] [varchar](100) NOT NULL,
	[Province] [varchar](100) NOT NULL,
	[PostalCode] [varchar](8) NOT NULL,
	[IsActive] [bit] NOT NULL,
	[DateCreated] [datetime] NOT NULL,
 CONSTRAINT [PK_Restaurant] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Stock]    Script Date: 2021/01/04 14:13:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Stock](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](100) NOT NULL,
	[UnitOfMeasure] [varchar](20) NULL,
	[ValueOfMeasure] [decimal](18, 2) NULL,
	[Quantity] [int] NOT NULL,
	[ExpiryDate] [datetime] NULL,
	[DateCreated] [datetime] NOT NULL,
	[Comment] [varchar](1000) NULL,
 CONSTRAINT [PK_Stock] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[Product] ON 

INSERT [dbo].[Product]([Id], [Name], [Description],[PictureUrl],[Price],[Special],[OfferEndDate],[OfferPrice],[IsActive],[DateCreated]) VALUES (1, N'Hawaiin', N'Tomato Base, Ham & Pineapple', N'2020-12-26_09-18-33-216.jpg', CAST(69.00 AS Decimal(17, 2)), 0, NULL, NULL, 1, CAST(N'2020-12-26T09:18:33.237' AS DateTime))
INSERT [dbo].[Product]([Id], [Name], [Description],[PictureUrl],[Price],[Special],[OfferEndDate],[OfferPrice],[IsActive],[DateCreated]) VALUES (2, N'Cheese Speciality',N'Tomato Base, 3 Secret Cheeses','2020-12-26_09-18-33-216.jpg',CAST(69.00 AS Decimal(17, 2)),1,null,null,1,'2021-01-07 09:18:33.217')
INSERT [dbo].[Product]([Id], [Name], [Description],[PictureUrl],[Price],[Special],[OfferEndDate],[OfferPrice],[IsActive],[DateCreated]) VALUES (3, N'Peperoni',N'Tomato Base, Pepperoni & Garlic','2020-12-26_09-18-33-216.jpg',CAST(69.00 AS Decimal(17, 2)),0,null,null,1,'2021-01-07 09:18:33.217')
INSERT [dbo].[Product]([Id], [Name], [Description],[PictureUrl],[Price],[Special],[OfferEndDate],[OfferPrice],[IsActive],[DateCreated]) VALUES (4, N'BBQ Chicken',N'Barbeque Base, Chicken & Mushroom','2020-12-26_09-18-33-216.jpg',CAST(72.00 AS Decimal(17, 2)),0,null,null,1,'2021-01-07 09:18:33.217')
INSERT [dbo].[Product]([Id], [Name], [Description],[PictureUrl],[Price],[Special],[OfferEndDate],[OfferPrice],[IsActive],[DateCreated]) VALUES (5, N'Quattro',N'Tomato Base, Mushroom, Ham, Anchovy & Olives','2020-12-26_09-18-33-216.jpg',CAST(72.00 AS Decimal(17, 2)),0,null,null,1,'2021-01-07 09:18:33.217')
INSERT [dbo].[Product]([Id], [Name], [Description],[PictureUrl],[Price],[Special],[OfferEndDate],[OfferPrice],[IsActive],[DateCreated]) VALUES (6, N'Suppreme',N'Tomato Base, Pepperoni, Mushroom, & Onion','2020-12-26_09-18-33-216.jpg',CAST(69.00 AS Decimal(17, 2)),0,null,null,1,'2021-01-07 09:18:33.217')
SET IDENTITY_INSERT [dbo].[Product] OFF
SET IDENTITY_INSERT [dbo].[Restaurant] ON 

INSERT [dbo].[Restaurant] ([Id], [Name], [Description], [PictureUrl], [Address], [City], [Province], [PostalCode], [IsActive], [DateCreated]) VALUES (2, N'Melrose Arch', N'Melrose Pezza', N'2020-12-23_23-09-45-254.svg', N'51 South Street', N'Johannesburg', N'Gauteng', N'0018', 1, CAST(N'2020-12-23T14:15:23.473' AS DateTime))
INSERT [dbo].[Restaurant] ([Id], [Name], [Description], [PictureUrl], [Address], [City], [Province], [PostalCode], [IsActive], [DateCreated]) VALUES (3, N'The Club', N'The Club Pezza', N'2020-12-23_23-09-45-254.svg', N'9 East Road', N'Pretoria', N'Gauteng', N'2479', 1, CAST(N'2020-12-23T14:15:23.473' AS DateTime))
INSERT [dbo].[Restaurant] ([Id], [Name], [Description], [PictureUrl], [Address], [City], [Province], [PostalCode], [IsActive], [DateCreated]) VALUES (4, N'V&ampA Waterfront', N'V&A Pezza', N'2020-12-23_23-09-45-254.svg', N'11 West Boulevard', N'Cape Town', N'Eastern Cape', N'2305', 1, CAST(N'2020-12-23T14:15:23.473' AS DateTime))
SET IDENTITY_INSERT [dbo].[Restaurant] OFF
SET IDENTITY_INSERT [dbo].[Stock] ON 

INSERT [dbo].[Stock] ([Id], [Name], [UnitOfMeasure], [ValueOfMeasure], [Quantity], [ExpiryDate], [DateCreated], [Comment]) VALUES (10, N'Flour', N'Kg', CAST(50.00 AS Decimal(18, 2)), 50, NULL, CAST(N'2020-12-20T14:37:23.717' AS DateTime), NULL)
SET IDENTITY_INSERT [dbo].[Stock] OFF
ALTER TABLE [dbo].[Customer] ADD  CONSTRAINT [DF_Customer_DateCreated]  DEFAULT (getdate()) FOR [DateCreated]
GO
ALTER TABLE [dbo].[Notify] ADD  CONSTRAINT [DF_Notify_Sent]  DEFAULT ((0)) FOR [Sent]
GO
ALTER TABLE [dbo].[Notify] ADD  CONSTRAINT [DF_Notify_Retry]  DEFAULT ((0)) FOR [Retry]
GO
ALTER TABLE [dbo].[Order] ADD  CONSTRAINT [DF_Order_DateCreated]  DEFAULT (getdate()) FOR [DateCreated]
GO
ALTER TABLE [dbo].[Product] ADD  CONSTRAINT [DF_Product_Special]  DEFAULT ((0)) FOR [Special]
GO
ALTER TABLE [dbo].[Product] ADD  CONSTRAINT [DF_Product_IsActive]  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[Product] ADD  CONSTRAINT [DF_Product_DateCreated]  DEFAULT (getdate()) FOR [DateCreated]
GO
ALTER TABLE [dbo].[Restaurant] ADD  CONSTRAINT [DF_Restaurant_IsActive]  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[Restaurant] ADD  CONSTRAINT [DF_Restaurant_DateCreated]  DEFAULT (getdate()) FOR [DateCreated]
GO
ALTER TABLE [dbo].[Stock] ADD  CONSTRAINT [DF_Stock_DateCreated]  DEFAULT (getdate()) FOR [DateCreated]
GO
ALTER TABLE [dbo].[Order]  WITH CHECK ADD  CONSTRAINT [FK_Order_Customer] FOREIGN KEY([CustomerId])
REFERENCES [dbo].[Customer] ([Id])
GO
ALTER TABLE [dbo].[Order] CHECK CONSTRAINT [FK_Order_Customer]
GO
ALTER TABLE [dbo].[Order]  WITH CHECK ADD  CONSTRAINT [FK_Order_Restaurant] FOREIGN KEY([RestaurantId])
REFERENCES [dbo].[Restaurant] ([Id])
GO
ALTER TABLE [dbo].[Order] CHECK CONSTRAINT [FK_Order_Restaurant]
GO
ALTER TABLE [dbo].[OrderItem]  WITH CHECK ADD  CONSTRAINT [FK_OrderItem_Order] FOREIGN KEY([OrderId])
REFERENCES [dbo].[Order] ([Id])
GO
ALTER TABLE [dbo].[OrderItem] CHECK CONSTRAINT [FK_OrderItem_Order]
GO
ALTER TABLE [dbo].[OrderItem]  WITH CHECK ADD  CONSTRAINT [FK_OrderItem_Product] FOREIGN KEY([ProductId])
REFERENCES [dbo].[Product] ([Id])
GO
ALTER TABLE [dbo].[OrderItem] CHECK CONSTRAINT [FK_OrderItem_Product]
GO
USE [master]
GO
ALTER DATABASE [PezzaDb] SET  READ_WRITE 
GO
