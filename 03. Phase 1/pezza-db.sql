USE [master]
GO
/****** Object:  Database [PezzaDb]    Script Date: 2020/08/03 00:51:29 ******/
CREATE DATABASE [PezzaDb]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'PezzaDb', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL14.MSSQLSERVER\MSSQL\DATA\PezzaDb.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'PezzaDb_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL14.MSSQLSERVER\MSSQL\DATA\PezzaDb_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
GO
ALTER DATABASE [PezzaDb] SET COMPATIBILITY_LEVEL = 140
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [PezzaDb].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [PezzaDb] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [PezzaDb] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [PezzaDb] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [PezzaDb] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [PezzaDb] SET ARITHABORT OFF 
GO
ALTER DATABASE [PezzaDb] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [PezzaDb] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [PezzaDb] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [PezzaDb] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [PezzaDb] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [PezzaDb] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [PezzaDb] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [PezzaDb] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [PezzaDb] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [PezzaDb] SET  DISABLE_BROKER 
GO
ALTER DATABASE [PezzaDb] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [PezzaDb] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [PezzaDb] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [PezzaDb] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [PezzaDb] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [PezzaDb] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [PezzaDb] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [PezzaDb] SET RECOVERY FULL 
GO
ALTER DATABASE [PezzaDb] SET  MULTI_USER 
GO
ALTER DATABASE [PezzaDb] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [PezzaDb] SET DB_CHAINING OFF 
GO
ALTER DATABASE [PezzaDb] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [PezzaDb] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [PezzaDb] SET DELAYED_DURABILITY = DISABLED 
GO
EXEC sys.sp_db_vardecimal_storage_format N'PezzaDb', N'ON'
GO
ALTER DATABASE [PezzaDb] SET QUERY_STORE = OFF
GO
USE [PezzaDb]
GO
/****** Object:  Table [dbo].[Customer]    Script Date: 2020/08/03 00:51:29 ******/
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
	[ZipCode] [varchar](8) NOT NULL,
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
/****** Object:  Table [dbo].[Notify]    Script Date: 2020/08/03 00:51:29 ******/
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
/****** Object:  Table [dbo].[Order]    Script Date: 2020/08/03 00:51:29 ******/
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
 CONSTRAINT [PK_Order] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[OrderItem]    Script Date: 2020/08/03 00:51:30 ******/
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
/****** Object:  Table [dbo].[Product]    Script Date: 2020/08/03 00:51:30 ******/
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
/****** Object:  Table [dbo].[Restaurant]    Script Date: 2020/08/03 00:51:30 ******/
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
/****** Object:  Table [dbo].[Stock]    Script Date: 2020/08/03 00:51:30 ******/
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
ALTER TABLE [dbo].[OrderItem]  WITH CHECK ADD  CONSTRAINT [FK_OrderItem_Product] FOREIGN KEY([ProductId])
REFERENCES [dbo].[Product] ([Id])
GO
ALTER TABLE [dbo].[OrderItem] CHECK CONSTRAINT [FK_OrderItem_Product]
GO
USE [master]
GO
ALTER DATABASE [PezzaDb] SET  READ_WRITE 
GO
