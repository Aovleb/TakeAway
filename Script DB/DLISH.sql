USE [master]
GO
/****** Object:  Database [GroffierKhloufi]    Script Date: 12-08-25 16:49:40 ******/
CREATE DATABASE [GroffierKhloufi]
GO
ALTER DATABASE [GroffierKhloufi] SET COMPATIBILITY_LEVEL = 150
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [GroffierKhloufi].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [GroffierKhloufi] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [GroffierKhloufi] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [GroffierKhloufi] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [GroffierKhloufi] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [GroffierKhloufi] SET ARITHABORT OFF 
GO
ALTER DATABASE [GroffierKhloufi] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [GroffierKhloufi] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [GroffierKhloufi] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [GroffierKhloufi] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [GroffierKhloufi] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [GroffierKhloufi] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [GroffierKhloufi] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [GroffierKhloufi] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [GroffierKhloufi] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [GroffierKhloufi] SET  DISABLE_BROKER 
GO
ALTER DATABASE [GroffierKhloufi] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [GroffierKhloufi] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [GroffierKhloufi] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [GroffierKhloufi] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [GroffierKhloufi] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [GroffierKhloufi] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [GroffierKhloufi] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [GroffierKhloufi] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [GroffierKhloufi] SET  MULTI_USER 
GO
ALTER DATABASE [GroffierKhloufi] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [GroffierKhloufi] SET DB_CHAINING OFF 
GO
ALTER DATABASE [GroffierKhloufi] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [GroffierKhloufi] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [GroffierKhloufi] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [GroffierKhloufi] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
ALTER DATABASE [GroffierKhloufi] SET QUERY_STORE = OFF
GO
USE [GroffierKhloufi]
GO
/****** Object:  Table [dbo].[Address]    Script Date: 12-08-25 16:49:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Address](
	[id_address] [int] IDENTITY(1,1) NOT NULL,
	[street_name] [varchar](50) NOT NULL,
	[street_number] [varchar](20) NOT NULL,
	[postal_code] [varchar](20) NOT NULL,
	[city] [varchar](50) NOT NULL,
	[country] [varchar](50) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[id_address] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Client]    Script Date: 12-08-25 16:49:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Client](
	[id_person] [int] NOT NULL,
	[lastName] [varchar](50) NOT NULL,
	[firstName] [varchar](50) NOT NULL,
	[phoneNumber] [varchar](50) NOT NULL,
	[id_address] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[id_person] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ClientOrder]    Script Date: 12-08-25 16:49:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ClientOrder](
	[orderNumber] [int] IDENTITY(1,1) NOT NULL,
	[status] [int] NOT NULL,
	[isDelivery] [bit] NOT NULL,
	[orderDate] [datetime2](7) NOT NULL,
	[id_person] [int] NOT NULL,
	[id_service] [int] NOT NULL,
	[id_restaurant] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[orderNumber] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ClientOrder_Meal]    Script Date: 12-08-25 16:49:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ClientOrder_Meal](
	[orderNumber] [int] NOT NULL,
	[id_meal] [int] NOT NULL,
	[quantity] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[orderNumber] ASC,
	[id_meal] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Dish]    Script Date: 12-08-25 16:49:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Dish](
	[id_meal] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[id_meal] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Meal]    Script Date: 12-08-25 16:49:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Meal](
	[id_meal] [int] IDENTITY(1,1) NOT NULL,
	[name] [varchar](50) NOT NULL,
	[description] [varchar](250) NOT NULL,
	[price] [float] NOT NULL,
	[id_restaurant] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[id_meal] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Meal_Service]    Script Date: 12-08-25 16:49:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Meal_Service](
	[id_service] [int] NOT NULL,
	[id_meal] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[id_service] ASC,
	[id_meal] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Menu]    Script Date: 12-08-25 16:49:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Menu](
	[id_meal] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[id_meal] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Menu_Dish]    Script Date: 12-08-25 16:49:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Menu_Dish](
	[id_menu] [int] NOT NULL,
	[id_dish] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[id_menu] ASC,
	[id_dish] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Person]    Script Date: 12-08-25 16:49:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Person](
	[id_person] [int] IDENTITY(1,1) NOT NULL,
	[email] [varchar](260) NOT NULL,
	[password] [varchar](64) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[id_person] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Restaurant]    Script Date: 12-08-25 16:49:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Restaurant](
	[id_restaurant] [int] IDENTITY(1,1) NOT NULL,
	[name] [varchar](50) NOT NULL,
	[description] [varchar](250) NOT NULL,
	[phoneNumber] [varchar](15) NOT NULL,
	[id_address] [int] NOT NULL,
	[id_person] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[id_restaurant] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[RestaurantOwner]    Script Date: 12-08-25 16:49:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RestaurantOwner](
	[id_person] [int] NOT NULL,
	[name] [varchar](50) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[id_person] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Service]    Script Date: 12-08-25 16:49:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Service](
	[id_service] [int] IDENTITY(1,1) NOT NULL,
	[startTime] [time](7) NOT NULL,
	[endTime] [time](7) NOT NULL,
	[id_restaurant] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[id_service] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[Address] ON 
GO
INSERT [dbo].[Address] ([id_address], [street_name], [street_number], [postal_code], [city], [country]) VALUES (1, N'Peace Street', N'12', N'75001', N'Paris', N'France')
GO
INSERT [dbo].[Address] ([id_address], [street_name], [street_number], [postal_code], [city], [country]) VALUES (2, N'Archives Street', N'25', N'75004', N'Paris', N'France')
GO
INSERT [dbo].[Address] ([id_address], [street_name], [street_number], [postal_code], [city], [country]) VALUES (3, N'Saint-Germain Boulevard', N'50', N'75005', N'Paris', N'France')
GO
INSERT [dbo].[Address] ([id_address], [street_name], [street_number], [postal_code], [city], [country]) VALUES (4, N'Louise Avenue', N'45', N'1050', N'Brussels', N'Belgium')
GO
INSERT [dbo].[Address] ([id_address], [street_name], [street_number], [postal_code], [city], [country]) VALUES (5, N'Royal Street', N'10', N'1000', N'Brussels', N'Belgium')
GO
INSERT [dbo].[Address] ([id_address], [street_name], [street_number], [postal_code], [city], [country]) VALUES (6, N'Waterloo Road', N'200', N'1060', N'Brussels', N'Belgium')
GO
INSERT [dbo].[Address] ([id_address], [street_name], [street_number], [postal_code], [city], [country]) VALUES (7, N'King Street', N'22', N'70173', N'Stuttgart', N'Germany')
GO
INSERT [dbo].[Address] ([id_address], [street_name], [street_number], [postal_code], [city], [country]) VALUES (8, N'Main Street', N'15', N'80331', N'Munich', N'Germany')
GO
INSERT [dbo].[Address] ([id_address], [street_name], [street_number], [postal_code], [city], [country]) VALUES (9, N'Schwab Street', N'30', N'70197', N'Stuttgart', N'Germany')
GO
INSERT [dbo].[Address] ([id_address], [street_name], [street_number], [postal_code], [city], [country]) VALUES (10, N'Rome Street', N'15', N'00184', N'Rome', N'Italy')
GO
INSERT [dbo].[Address] ([id_address], [street_name], [street_number], [postal_code], [city], [country]) VALUES (11, N'Veneto Street', N'40', N'00187', N'Rome', N'Italy')
GO
INSERT [dbo].[Address] ([id_address], [street_name], [street_number], [postal_code], [city], [country]) VALUES (12, N'Spain Square', N'5', N'00187', N'Rome', N'Italy')
GO
INSERT [dbo].[Address] ([id_address], [street_name], [street_number], [postal_code], [city], [country]) VALUES (13, N'Main Street', N'30', N'28013', N'Madrid', N'Spain')
GO
INSERT [dbo].[Address] ([id_address], [street_name], [street_number], [postal_code], [city], [country]) VALUES (14, N'Alcalá Street', N'20', N'28014', N'Madrid', N'Spain')
GO
INSERT [dbo].[Address] ([id_address], [street_name], [street_number], [postal_code], [city], [country]) VALUES (15, N'Prado Promenade', N'10', N'28014', N'Madrid', N'Spain')
GO
INSERT [dbo].[Address] ([id_address], [street_name], [street_number], [postal_code], [city], [country]) VALUES (16, N'Faubourg Street', N'8', N'75008', N'Paris', N'France')
GO
INSERT [dbo].[Address] ([id_address], [street_name], [street_number], [postal_code], [city], [country]) VALUES (17, N'Waterloo Road', N'120', N'1060', N'Brussels', N'Belgium')
GO
INSERT [dbo].[Address] ([id_address], [street_name], [street_number], [postal_code], [city], [country]) VALUES (18, N'Schiller Street', N'5', N'70173', N'Stuttgart', N'Germany')
GO
INSERT [dbo].[Address] ([id_address], [street_name], [street_number], [postal_code], [city], [country]) VALUES (19, N'Corso Street', N'25', N'00186', N'Rome', N'Italy')
GO
INSERT [dbo].[Address] ([id_address], [street_name], [street_number], [postal_code], [city], [country]) VALUES (20, N'Castellana Promenade', N'50', N'28046', N'Madrid', N'Spain')
GO
INSERT [dbo].[Address] ([id_address], [street_name], [street_number], [postal_code], [city], [country]) VALUES (21, N'Rivoli Street', N'14', N'75004', N'Paris', N'France')
GO
INSERT [dbo].[Address] ([id_address], [street_name], [street_number], [postal_code], [city], [country]) VALUES (22, N'Anspach Boulevard', N'33', N'1000', N'Brussels', N'Belgium')
GO
INSERT [dbo].[Address] ([id_address], [street_name], [street_number], [postal_code], [city], [country]) VALUES (23, N'Station Street', N'10', N'80331', N'Munich', N'Germany')
GO
INSERT [dbo].[Address] ([id_address], [street_name], [street_number], [postal_code], [city], [country]) VALUES (24, N'Navona Square', N'7', N'00186', N'Rome', N'Italy')
GO
INSERT [dbo].[Address] ([id_address], [street_name], [street_number], [postal_code], [city], [country]) VALUES (25, N'Grand Way', N'20', N'28013', N'Madrid', N'Spain')
GO
SET IDENTITY_INSERT [dbo].[Address] OFF
GO
INSERT [dbo].[Client] ([id_person], [lastName], [firstName], [phoneNumber], [id_address]) VALUES (6, N'Bonnet', N'Alice', N'0123456780', 16)
GO
INSERT [dbo].[Client] ([id_person], [lastName], [firstName], [phoneNumber], [id_address]) VALUES (7, N'Leroy', N'Bob', N'0987654320', 17)
GO
INSERT [dbo].[Client] ([id_person], [lastName], [firstName], [phoneNumber], [id_address]) VALUES (8, N'Durand', N'Claire', N'0123456781', 18)
GO
INSERT [dbo].[Client] ([id_person], [lastName], [firstName], [phoneNumber], [id_address]) VALUES (9, N'Fournier', N'David', N'0987654321', 19)
GO
INSERT [dbo].[Client] ([id_person], [lastName], [firstName], [phoneNumber], [id_address]) VALUES (10, N'Roux', N'Emma', N'0123456782', 20)
GO
INSERT [dbo].[Client] ([id_person], [lastName], [firstName], [phoneNumber], [id_address]) VALUES (11, N'Girard', N'Felix', N'0987654322', 21)
GO
INSERT [dbo].[Client] ([id_person], [lastName], [firstName], [phoneNumber], [id_address]) VALUES (12, N'Noel', N'Gabrielle', N'0123456783', 22)
GO
INSERT [dbo].[Client] ([id_person], [lastName], [firstName], [phoneNumber], [id_address]) VALUES (13, N'Lambert', N'Hugo', N'0987654323', 23)
GO
INSERT [dbo].[Client] ([id_person], [lastName], [firstName], [phoneNumber], [id_address]) VALUES (14, N'Morin', N'Isabelle', N'0123456784', 24)
GO
INSERT [dbo].[Client] ([id_person], [lastName], [firstName], [phoneNumber], [id_address]) VALUES (15, N'Petit', N'Julien', N'0987654324', 25)
GO
SET IDENTITY_INSERT [dbo].[ClientOrder] ON 
GO
INSERT [dbo].[ClientOrder] ([orderNumber], [status], [isDelivery], [orderDate], [id_person], [id_service], [id_restaurant]) VALUES (1, 2, 1, CAST(N'2023-10-01T00:00:00.0000000' AS DateTime2), 6, 1, 1)
GO
INSERT [dbo].[ClientOrder] ([orderNumber], [status], [isDelivery], [orderDate], [id_person], [id_service], [id_restaurant]) VALUES (2, 0, 0, CAST(N'2023-10-02T00:00:00.0000000' AS DateTime2), 7, 3, 2)
GO
INSERT [dbo].[ClientOrder] ([orderNumber], [status], [isDelivery], [orderDate], [id_person], [id_service], [id_restaurant]) VALUES (3, 2, 1, CAST(N'2023-10-03T00:00:00.0000000' AS DateTime2), 8, 5, 3)
GO
INSERT [dbo].[ClientOrder] ([orderNumber], [status], [isDelivery], [orderDate], [id_person], [id_service], [id_restaurant]) VALUES (4, 0, 0, CAST(N'2023-10-04T00:00:00.0000000' AS DateTime2), 9, 7, 4)
GO
INSERT [dbo].[ClientOrder] ([orderNumber], [status], [isDelivery], [orderDate], [id_person], [id_service], [id_restaurant]) VALUES (5, 0, 1, CAST(N'2023-10-05T00:00:00.0000000' AS DateTime2), 10, 9, 5)
GO
INSERT [dbo].[ClientOrder] ([orderNumber], [status], [isDelivery], [orderDate], [id_person], [id_service], [id_restaurant]) VALUES (6, 0, 0, CAST(N'2023-10-06T00:00:00.0000000' AS DateTime2), 11, 11, 6)
GO
INSERT [dbo].[ClientOrder] ([orderNumber], [status], [isDelivery], [orderDate], [id_person], [id_service], [id_restaurant]) VALUES (7, 0, 1, CAST(N'2023-10-07T00:00:00.0000000' AS DateTime2), 12, 13, 7)
GO
INSERT [dbo].[ClientOrder] ([orderNumber], [status], [isDelivery], [orderDate], [id_person], [id_service], [id_restaurant]) VALUES (8, 0, 0, CAST(N'2023-10-08T00:00:00.0000000' AS DateTime2), 13, 15, 8)
GO
INSERT [dbo].[ClientOrder] ([orderNumber], [status], [isDelivery], [orderDate], [id_person], [id_service], [id_restaurant]) VALUES (9, 0, 1, CAST(N'2023-10-09T00:00:00.0000000' AS DateTime2), 14, 17, 9)
GO
INSERT [dbo].[ClientOrder] ([orderNumber], [status], [isDelivery], [orderDate], [id_person], [id_service], [id_restaurant]) VALUES (10, 0, 0, CAST(N'2023-10-10T00:00:00.0000000' AS DateTime2), 15, 19, 10)
GO
INSERT [dbo].[ClientOrder] ([orderNumber], [status], [isDelivery], [orderDate], [id_person], [id_service], [id_restaurant]) VALUES (11, 0, 1, CAST(N'2023-10-11T00:00:00.0000000' AS DateTime2), 6, 21, 11)
GO
INSERT [dbo].[ClientOrder] ([orderNumber], [status], [isDelivery], [orderDate], [id_person], [id_service], [id_restaurant]) VALUES (12, 0, 0, CAST(N'2023-10-12T00:00:00.0000000' AS DateTime2), 7, 23, 12)
GO
INSERT [dbo].[ClientOrder] ([orderNumber], [status], [isDelivery], [orderDate], [id_person], [id_service], [id_restaurant]) VALUES (13, 0, 1, CAST(N'2023-10-13T00:00:00.0000000' AS DateTime2), 8, 25, 13)
GO
INSERT [dbo].[ClientOrder] ([orderNumber], [status], [isDelivery], [orderDate], [id_person], [id_service], [id_restaurant]) VALUES (14, 0, 0, CAST(N'2023-10-14T00:00:00.0000000' AS DateTime2), 9, 27, 14)
GO
INSERT [dbo].[ClientOrder] ([orderNumber], [status], [isDelivery], [orderDate], [id_person], [id_service], [id_restaurant]) VALUES (15, 0, 1, CAST(N'2023-10-15T00:00:00.0000000' AS DateTime2), 10, 29, 15)
GO
INSERT [dbo].[ClientOrder] ([orderNumber], [status], [isDelivery], [orderDate], [id_person], [id_service], [id_restaurant]) VALUES (16, 2, 0, CAST(N'2025-05-19T16:26:12.3000000' AS DateTime2), 15, 2, 1)
GO
INSERT [dbo].[ClientOrder] ([orderNumber], [status], [isDelivery], [orderDate], [id_person], [id_service], [id_restaurant]) VALUES (1016, 1, 0, CAST(N'2025-08-02T14:13:59.6933333' AS DateTime2), 15, 6, 3)
GO
INSERT [dbo].[ClientOrder] ([orderNumber], [status], [isDelivery], [orderDate], [id_person], [id_service], [id_restaurant]) VALUES (2017, 2, 0, CAST(N'2025-08-12T16:11:05.9400000' AS DateTime2), 15, 2, 1)
GO
INSERT [dbo].[ClientOrder] ([orderNumber], [status], [isDelivery], [orderDate], [id_person], [id_service], [id_restaurant]) VALUES (2019, 0, 0, CAST(N'2025-08-12T16:15:35.6933333' AS DateTime2), 15, 2, 1)
GO
SET IDENTITY_INSERT [dbo].[ClientOrder] OFF
GO
INSERT [dbo].[ClientOrder_Meal] ([orderNumber], [id_meal], [quantity]) VALUES (1, 1, 1)
GO
INSERT [dbo].[ClientOrder_Meal] ([orderNumber], [id_meal], [quantity]) VALUES (1, 31, 1)
GO
INSERT [dbo].[ClientOrder_Meal] ([orderNumber], [id_meal], [quantity]) VALUES (1, 33, 1)
GO
INSERT [dbo].[ClientOrder_Meal] ([orderNumber], [id_meal], [quantity]) VALUES (2, 3, 1)
GO
INSERT [dbo].[ClientOrder_Meal] ([orderNumber], [id_meal], [quantity]) VALUES (2, 37, 1)
GO
INSERT [dbo].[ClientOrder_Meal] ([orderNumber], [id_meal], [quantity]) VALUES (2, 39, 1)
GO
INSERT [dbo].[ClientOrder_Meal] ([orderNumber], [id_meal], [quantity]) VALUES (3, 5, 1)
GO
INSERT [dbo].[ClientOrder_Meal] ([orderNumber], [id_meal], [quantity]) VALUES (3, 43, 1)
GO
INSERT [dbo].[ClientOrder_Meal] ([orderNumber], [id_meal], [quantity]) VALUES (3, 45, 1)
GO
INSERT [dbo].[ClientOrder_Meal] ([orderNumber], [id_meal], [quantity]) VALUES (4, 7, 1)
GO
INSERT [dbo].[ClientOrder_Meal] ([orderNumber], [id_meal], [quantity]) VALUES (4, 49, 1)
GO
INSERT [dbo].[ClientOrder_Meal] ([orderNumber], [id_meal], [quantity]) VALUES (4, 51, 1)
GO
INSERT [dbo].[ClientOrder_Meal] ([orderNumber], [id_meal], [quantity]) VALUES (5, 9, 1)
GO
INSERT [dbo].[ClientOrder_Meal] ([orderNumber], [id_meal], [quantity]) VALUES (5, 55, 1)
GO
INSERT [dbo].[ClientOrder_Meal] ([orderNumber], [id_meal], [quantity]) VALUES (5, 57, 1)
GO
INSERT [dbo].[ClientOrder_Meal] ([orderNumber], [id_meal], [quantity]) VALUES (6, 11, 1)
GO
INSERT [dbo].[ClientOrder_Meal] ([orderNumber], [id_meal], [quantity]) VALUES (6, 61, 1)
GO
INSERT [dbo].[ClientOrder_Meal] ([orderNumber], [id_meal], [quantity]) VALUES (6, 63, 1)
GO
INSERT [dbo].[ClientOrder_Meal] ([orderNumber], [id_meal], [quantity]) VALUES (7, 13, 1)
GO
INSERT [dbo].[ClientOrder_Meal] ([orderNumber], [id_meal], [quantity]) VALUES (7, 67, 1)
GO
INSERT [dbo].[ClientOrder_Meal] ([orderNumber], [id_meal], [quantity]) VALUES (7, 69, 1)
GO
INSERT [dbo].[ClientOrder_Meal] ([orderNumber], [id_meal], [quantity]) VALUES (8, 15, 1)
GO
INSERT [dbo].[ClientOrder_Meal] ([orderNumber], [id_meal], [quantity]) VALUES (8, 73, 1)
GO
INSERT [dbo].[ClientOrder_Meal] ([orderNumber], [id_meal], [quantity]) VALUES (8, 75, 1)
GO
INSERT [dbo].[ClientOrder_Meal] ([orderNumber], [id_meal], [quantity]) VALUES (9, 17, 1)
GO
INSERT [dbo].[ClientOrder_Meal] ([orderNumber], [id_meal], [quantity]) VALUES (9, 79, 1)
GO
INSERT [dbo].[ClientOrder_Meal] ([orderNumber], [id_meal], [quantity]) VALUES (9, 81, 1)
GO
INSERT [dbo].[ClientOrder_Meal] ([orderNumber], [id_meal], [quantity]) VALUES (10, 19, 1)
GO
INSERT [dbo].[ClientOrder_Meal] ([orderNumber], [id_meal], [quantity]) VALUES (10, 85, 1)
GO
INSERT [dbo].[ClientOrder_Meal] ([orderNumber], [id_meal], [quantity]) VALUES (10, 87, 1)
GO
INSERT [dbo].[ClientOrder_Meal] ([orderNumber], [id_meal], [quantity]) VALUES (11, 21, 1)
GO
INSERT [dbo].[ClientOrder_Meal] ([orderNumber], [id_meal], [quantity]) VALUES (11, 91, 1)
GO
INSERT [dbo].[ClientOrder_Meal] ([orderNumber], [id_meal], [quantity]) VALUES (11, 93, 1)
GO
INSERT [dbo].[ClientOrder_Meal] ([orderNumber], [id_meal], [quantity]) VALUES (12, 23, 1)
GO
INSERT [dbo].[ClientOrder_Meal] ([orderNumber], [id_meal], [quantity]) VALUES (12, 97, 1)
GO
INSERT [dbo].[ClientOrder_Meal] ([orderNumber], [id_meal], [quantity]) VALUES (12, 99, 1)
GO
INSERT [dbo].[ClientOrder_Meal] ([orderNumber], [id_meal], [quantity]) VALUES (13, 25, 1)
GO
INSERT [dbo].[ClientOrder_Meal] ([orderNumber], [id_meal], [quantity]) VALUES (13, 103, 1)
GO
INSERT [dbo].[ClientOrder_Meal] ([orderNumber], [id_meal], [quantity]) VALUES (13, 105, 1)
GO
INSERT [dbo].[ClientOrder_Meal] ([orderNumber], [id_meal], [quantity]) VALUES (14, 27, 1)
GO
INSERT [dbo].[ClientOrder_Meal] ([orderNumber], [id_meal], [quantity]) VALUES (14, 109, 1)
GO
INSERT [dbo].[ClientOrder_Meal] ([orderNumber], [id_meal], [quantity]) VALUES (14, 111, 1)
GO
INSERT [dbo].[ClientOrder_Meal] ([orderNumber], [id_meal], [quantity]) VALUES (15, 29, 1)
GO
INSERT [dbo].[ClientOrder_Meal] ([orderNumber], [id_meal], [quantity]) VALUES (15, 115, 1)
GO
INSERT [dbo].[ClientOrder_Meal] ([orderNumber], [id_meal], [quantity]) VALUES (15, 117, 1)
GO
INSERT [dbo].[ClientOrder_Meal] ([orderNumber], [id_meal], [quantity]) VALUES (16, 34, 1)
GO
INSERT [dbo].[ClientOrder_Meal] ([orderNumber], [id_meal], [quantity]) VALUES (1016, 43, 1)
GO
INSERT [dbo].[ClientOrder_Meal] ([orderNumber], [id_meal], [quantity]) VALUES (2017, 2, 1)
GO
INSERT [dbo].[ClientOrder_Meal] ([orderNumber], [id_meal], [quantity]) VALUES (2019, 1, 1)
GO
INSERT [dbo].[ClientOrder_Meal] ([orderNumber], [id_meal], [quantity]) VALUES (2019, 31, 1)
GO
INSERT [dbo].[Dish] ([id_meal]) VALUES (31)
GO
INSERT [dbo].[Dish] ([id_meal]) VALUES (32)
GO
INSERT [dbo].[Dish] ([id_meal]) VALUES (33)
GO
INSERT [dbo].[Dish] ([id_meal]) VALUES (34)
GO
INSERT [dbo].[Dish] ([id_meal]) VALUES (35)
GO
INSERT [dbo].[Dish] ([id_meal]) VALUES (36)
GO
INSERT [dbo].[Dish] ([id_meal]) VALUES (37)
GO
INSERT [dbo].[Dish] ([id_meal]) VALUES (38)
GO
INSERT [dbo].[Dish] ([id_meal]) VALUES (39)
GO
INSERT [dbo].[Dish] ([id_meal]) VALUES (40)
GO
INSERT [dbo].[Dish] ([id_meal]) VALUES (41)
GO
INSERT [dbo].[Dish] ([id_meal]) VALUES (42)
GO
INSERT [dbo].[Dish] ([id_meal]) VALUES (43)
GO
INSERT [dbo].[Dish] ([id_meal]) VALUES (44)
GO
INSERT [dbo].[Dish] ([id_meal]) VALUES (45)
GO
INSERT [dbo].[Dish] ([id_meal]) VALUES (46)
GO
INSERT [dbo].[Dish] ([id_meal]) VALUES (47)
GO
INSERT [dbo].[Dish] ([id_meal]) VALUES (48)
GO
INSERT [dbo].[Dish] ([id_meal]) VALUES (49)
GO
INSERT [dbo].[Dish] ([id_meal]) VALUES (50)
GO
INSERT [dbo].[Dish] ([id_meal]) VALUES (51)
GO
INSERT [dbo].[Dish] ([id_meal]) VALUES (52)
GO
INSERT [dbo].[Dish] ([id_meal]) VALUES (53)
GO
INSERT [dbo].[Dish] ([id_meal]) VALUES (54)
GO
INSERT [dbo].[Dish] ([id_meal]) VALUES (55)
GO
INSERT [dbo].[Dish] ([id_meal]) VALUES (56)
GO
INSERT [dbo].[Dish] ([id_meal]) VALUES (57)
GO
INSERT [dbo].[Dish] ([id_meal]) VALUES (58)
GO
INSERT [dbo].[Dish] ([id_meal]) VALUES (59)
GO
INSERT [dbo].[Dish] ([id_meal]) VALUES (60)
GO
INSERT [dbo].[Dish] ([id_meal]) VALUES (61)
GO
INSERT [dbo].[Dish] ([id_meal]) VALUES (62)
GO
INSERT [dbo].[Dish] ([id_meal]) VALUES (63)
GO
INSERT [dbo].[Dish] ([id_meal]) VALUES (64)
GO
INSERT [dbo].[Dish] ([id_meal]) VALUES (65)
GO
INSERT [dbo].[Dish] ([id_meal]) VALUES (66)
GO
INSERT [dbo].[Dish] ([id_meal]) VALUES (67)
GO
INSERT [dbo].[Dish] ([id_meal]) VALUES (68)
GO
INSERT [dbo].[Dish] ([id_meal]) VALUES (69)
GO
INSERT [dbo].[Dish] ([id_meal]) VALUES (70)
GO
INSERT [dbo].[Dish] ([id_meal]) VALUES (71)
GO
INSERT [dbo].[Dish] ([id_meal]) VALUES (72)
GO
INSERT [dbo].[Dish] ([id_meal]) VALUES (73)
GO
INSERT [dbo].[Dish] ([id_meal]) VALUES (74)
GO
INSERT [dbo].[Dish] ([id_meal]) VALUES (75)
GO
INSERT [dbo].[Dish] ([id_meal]) VALUES (76)
GO
INSERT [dbo].[Dish] ([id_meal]) VALUES (77)
GO
INSERT [dbo].[Dish] ([id_meal]) VALUES (78)
GO
INSERT [dbo].[Dish] ([id_meal]) VALUES (79)
GO
INSERT [dbo].[Dish] ([id_meal]) VALUES (80)
GO
INSERT [dbo].[Dish] ([id_meal]) VALUES (81)
GO
INSERT [dbo].[Dish] ([id_meal]) VALUES (82)
GO
INSERT [dbo].[Dish] ([id_meal]) VALUES (83)
GO
INSERT [dbo].[Dish] ([id_meal]) VALUES (84)
GO
INSERT [dbo].[Dish] ([id_meal]) VALUES (85)
GO
INSERT [dbo].[Dish] ([id_meal]) VALUES (86)
GO
INSERT [dbo].[Dish] ([id_meal]) VALUES (87)
GO
INSERT [dbo].[Dish] ([id_meal]) VALUES (88)
GO
INSERT [dbo].[Dish] ([id_meal]) VALUES (89)
GO
INSERT [dbo].[Dish] ([id_meal]) VALUES (90)
GO
INSERT [dbo].[Dish] ([id_meal]) VALUES (91)
GO
INSERT [dbo].[Dish] ([id_meal]) VALUES (92)
GO
INSERT [dbo].[Dish] ([id_meal]) VALUES (93)
GO
INSERT [dbo].[Dish] ([id_meal]) VALUES (94)
GO
INSERT [dbo].[Dish] ([id_meal]) VALUES (95)
GO
INSERT [dbo].[Dish] ([id_meal]) VALUES (96)
GO
INSERT [dbo].[Dish] ([id_meal]) VALUES (97)
GO
INSERT [dbo].[Dish] ([id_meal]) VALUES (98)
GO
INSERT [dbo].[Dish] ([id_meal]) VALUES (99)
GO
INSERT [dbo].[Dish] ([id_meal]) VALUES (100)
GO
INSERT [dbo].[Dish] ([id_meal]) VALUES (101)
GO
INSERT [dbo].[Dish] ([id_meal]) VALUES (102)
GO
INSERT [dbo].[Dish] ([id_meal]) VALUES (103)
GO
INSERT [dbo].[Dish] ([id_meal]) VALUES (104)
GO
INSERT [dbo].[Dish] ([id_meal]) VALUES (105)
GO
INSERT [dbo].[Dish] ([id_meal]) VALUES (106)
GO
INSERT [dbo].[Dish] ([id_meal]) VALUES (107)
GO
INSERT [dbo].[Dish] ([id_meal]) VALUES (108)
GO
INSERT [dbo].[Dish] ([id_meal]) VALUES (109)
GO
INSERT [dbo].[Dish] ([id_meal]) VALUES (110)
GO
INSERT [dbo].[Dish] ([id_meal]) VALUES (111)
GO
INSERT [dbo].[Dish] ([id_meal]) VALUES (112)
GO
INSERT [dbo].[Dish] ([id_meal]) VALUES (113)
GO
INSERT [dbo].[Dish] ([id_meal]) VALUES (114)
GO
INSERT [dbo].[Dish] ([id_meal]) VALUES (115)
GO
INSERT [dbo].[Dish] ([id_meal]) VALUES (116)
GO
INSERT [dbo].[Dish] ([id_meal]) VALUES (117)
GO
INSERT [dbo].[Dish] ([id_meal]) VALUES (118)
GO
INSERT [dbo].[Dish] ([id_meal]) VALUES (119)
GO
INSERT [dbo].[Dish] ([id_meal]) VALUES (120)
GO
SET IDENTITY_INSERT [dbo].[Meal] ON 
GO
INSERT [dbo].[Meal] ([id_meal], [name], [description], [price], [id_restaurant]) VALUES (1, N'Classic Menu', N'Choice of starter, main course, and dessert.', 25, 1)
GO
INSERT [dbo].[Meal] ([id_meal], [name], [description], [price], [id_restaurant]) VALUES (2, N'Gourmet Menu', N'Premium French selection.', 35, 1)
GO
INSERT [dbo].[Meal] ([id_meal], [name], [description], [price], [id_restaurant]) VALUES (3, N'Bistro Menu', N'Simple and tasty dishes.', 22, 2)
GO
INSERT [dbo].[Meal] ([id_meal], [name], [description], [price], [id_restaurant]) VALUES (4, N'Parisian Menu', N'Modern French cuisine.', 30, 2)
GO
INSERT [dbo].[Meal] ([id_meal], [name], [description], [price], [id_restaurant]) VALUES (5, N'Brasserie Menu', N'Regional specialties.', 28, 3)
GO
INSERT [dbo].[Meal] ([id_meal], [name], [description], [price], [id_restaurant]) VALUES (6, N'Tradition Menu', N'French terroir dishes.', 32, 3)
GO
INSERT [dbo].[Meal] ([id_meal], [name], [description], [price], [id_restaurant]) VALUES (7, N'Belgian Menu', N'Traditional Belgian specialties.', 28, 4)
GO
INSERT [dbo].[Meal] ([id_meal], [name], [description], [price], [id_restaurant]) VALUES (8, N'Fries Menu', N'Belgian meal with fries.', 22, 4)
GO
INSERT [dbo].[Meal] ([id_meal], [name], [description], [price], [id_restaurant]) VALUES (9, N'Modern Menu', N'Revisited Belgian cuisine.', 26, 5)
GO
INSERT [dbo].[Meal] ([id_meal], [name], [description], [price], [id_restaurant]) VALUES (10, N'Walloon Menu', N'Flavors from southern Belgium.', 30, 5)
GO
INSERT [dbo].[Meal] ([id_meal], [name], [description], [price], [id_restaurant]) VALUES (11, N'Brasserie Menu', N'Hearty Belgian dishes.', 27, 6)
GO
INSERT [dbo].[Meal] ([id_meal], [name], [description], [price], [id_restaurant]) VALUES (12, N'Rustic Menu', N'Authentic Belgian cuisine.', 25, 6)
GO
INSERT [dbo].[Meal] ([id_meal], [name], [description], [price], [id_restaurant]) VALUES (13, N'Bavarian Menu', N'Classic German dishes.', 30, 7)
GO
INSERT [dbo].[Meal] ([id_meal], [name], [description], [price], [id_restaurant]) VALUES (14, N'Alpine Menu', N'Mountain flavors.', 32, 7)
GO
INSERT [dbo].[Meal] ([id_meal], [name], [description], [price], [id_restaurant]) VALUES (15, N'Munich Menu', N'Festive Bavarian cuisine.', 29, 8)
GO
INSERT [dbo].[Meal] ([id_meal], [name], [description], [price], [id_restaurant]) VALUES (16, N'Traditional Menu', N'Hearty German dishes.', 31, 8)
GO
INSERT [dbo].[Meal] ([id_meal], [name], [description], [price], [id_restaurant]) VALUES (17, N'Garden Menu', N'German outdoor meal.', 28, 9)
GO
INSERT [dbo].[Meal] ([id_meal], [name], [description], [price], [id_restaurant]) VALUES (18, N'Rustic Menu', N'Simple German cuisine.', 26, 9)
GO
INSERT [dbo].[Meal] ([id_meal], [name], [description], [price], [id_restaurant]) VALUES (19, N'Italian Menu', N'Authentic Italian cuisine.', 27, 10)
GO
INSERT [dbo].[Meal] ([id_meal], [name], [description], [price], [id_restaurant]) VALUES (20, N'Mediterranean Menu', N'Flavors of the Mediterranean.', 29, 10)
GO
INSERT [dbo].[Meal] ([id_meal], [name], [description], [price], [id_restaurant]) VALUES (21, N'Pizza Menu', N'Pizzas and Italian desserts.', 24, 11)
GO
INSERT [dbo].[Meal] ([id_meal], [name], [description], [price], [id_restaurant]) VALUES (22, N'Bella Menu', N'Light Italian cuisine.', 26, 11)
GO
INSERT [dbo].[Meal] ([id_meal], [name], [description], [price], [id_restaurant]) VALUES (23, N'Osteria Menu', N'Refined Italian dishes.', 30, 12)
GO
INSERT [dbo].[Meal] ([id_meal], [name], [description], [price], [id_restaurant]) VALUES (24, N'Roman Menu', N'Flavors of Rome.', 28, 12)
GO
INSERT [dbo].[Meal] ([id_meal], [name], [description], [price], [id_restaurant]) VALUES (25, N'Tapas Menu', N'Assortment of Spanish tapas.', 26, 13)
GO
INSERT [dbo].[Meal] ([id_meal], [name], [description], [price], [id_restaurant]) VALUES (26, N'Paella Menu', N'Paella and dessert.', 31, 13)
GO
INSERT [dbo].[Meal] ([id_meal], [name], [description], [price], [id_restaurant]) VALUES (27, N'Andalusian Menu', N'Tapas and southern dishes.', 27, 14)
GO
INSERT [dbo].[Meal] ([id_meal], [name], [description], [price], [id_restaurant]) VALUES (28, N'Fiesta Menu', N'Festive Spanish cuisine.', 29, 14)
GO
INSERT [dbo].[Meal] ([id_meal], [name], [description], [price], [id_restaurant]) VALUES (29, N'Bodega Menu', N'Traditional Spanish dishes.', 28, 15)
GO
INSERT [dbo].[Meal] ([id_meal], [name], [description], [price], [id_restaurant]) VALUES (30, N'Spanish Menu', N'Iberian flavors.', 30, 15)
GO
INSERT [dbo].[Meal] ([id_meal], [name], [description], [price], [id_restaurant]) VALUES (31, N'Coq au Vin', N'Traditional French dish.', 15, 1)
GO
INSERT [dbo].[Meal] ([id_meal], [name], [description], [price], [id_restaurant]) VALUES (32, N'Boeuf Bourguignon', N'Beef stew with red wine.', 16.5, 1)
GO
INSERT [dbo].[Meal] ([id_meal], [name], [description], [price], [id_restaurant]) VALUES (33, N'Tarte Tatin', N'Caramelized apple dessert.', 6.5, 1)
GO
INSERT [dbo].[Meal] ([id_meal], [name], [description], [price], [id_restaurant]) VALUES (34, N'Crème Brûlée', N'Vanilla custard dessert.', 5.5, 1)
GO
INSERT [dbo].[Meal] ([id_meal], [name], [description], [price], [id_restaurant]) VALUES (35, N'Niçoise Salad', N'Fresh Mediterranean salad.', 12, 1)
GO
INSERT [dbo].[Meal] ([id_meal], [name], [description], [price], [id_restaurant]) VALUES (36, N'Escargots', N'Snails with garlic butter.', 14, 1)
GO
INSERT [dbo].[Meal] ([id_meal], [name], [description], [price], [id_restaurant]) VALUES (37, N'Onglet with Shallots', N'Tender meat with shallot sauce.', 14.5, 2)
GO
INSERT [dbo].[Meal] ([id_meal], [name], [description], [price], [id_restaurant]) VALUES (38, N'Quiche Lorraine', N'Bacon tart.', 10, 2)
GO
INSERT [dbo].[Meal] ([id_meal], [name], [description], [price], [id_restaurant]) VALUES (39, N'Chocolate Mousse', N'Rich and creamy dessert.', 6, 2)
GO
INSERT [dbo].[Meal] ([id_meal], [name], [description], [price], [id_restaurant]) VALUES (40, N'Lemon Tart', N'Tangy dessert.', 5.5, 2)
GO
INSERT [dbo].[Meal] ([id_meal], [name], [description], [price], [id_restaurant]) VALUES (41, N'Lyonnaise Salad', N'Salad with poached egg.', 11.5, 2)
GO
INSERT [dbo].[Meal] ([id_meal], [name], [description], [price], [id_restaurant]) VALUES (42, N'Gratin Dauphinois', N'Creamy potatoes.', 8, 2)
GO
INSERT [dbo].[Meal] ([id_meal], [name], [description], [price], [id_restaurant]) VALUES (43, N'Duck Confit', N'Slow-cooked duck.', 17, 3)
GO
INSERT [dbo].[Meal] ([id_meal], [name], [description], [price], [id_restaurant]) VALUES (44, N'Onion Soup', N'French gratin soup.', 9, 3)
GO
INSERT [dbo].[Meal] ([id_meal], [name], [description], [price], [id_restaurant]) VALUES (45, N'Profiteroles', N'Choux pastry with cream and chocolate.', 7, 3)
GO
INSERT [dbo].[Meal] ([id_meal], [name], [description], [price], [id_restaurant]) VALUES (46, N'Clafoutis', N'Cherry dessert.', 6, 3)
GO
INSERT [dbo].[Meal] ([id_meal], [name], [description], [price], [id_restaurant]) VALUES (47, N'Warm Goat Cheese Salad', N'Salad with melted cheese.', 12.5, 3)
GO
INSERT [dbo].[Meal] ([id_meal], [name], [description], [price], [id_restaurant]) VALUES (48, N'Cassoulet', N'Bean and meat stew.', 16, 3)
GO
INSERT [dbo].[Meal] ([id_meal], [name], [description], [price], [id_restaurant]) VALUES (49, N'Flemish Carbonnade', N'Belgian beer stew.', 18, 4)
GO
INSERT [dbo].[Meal] ([id_meal], [name], [description], [price], [id_restaurant]) VALUES (50, N'Mussels with Fries', N'Marinated mussels with fries.', 16, 4)
GO
INSERT [dbo].[Meal] ([id_meal], [name], [description], [price], [id_restaurant]) VALUES (51, N'Liège Waffle', N'Sweet and crispy dessert.', 5, 4)
GO
INSERT [dbo].[Meal] ([id_meal], [name], [description], [price], [id_restaurant]) VALUES (52, N'Speculoos', N'Spiced Belgian cookie.', 4.5, 4)
GO
INSERT [dbo].[Meal] ([id_meal], [name], [description], [price], [id_restaurant]) VALUES (53, N'Waterzooi', N'Creamy chicken stew.', 15.5, 4)
GO
INSERT [dbo].[Meal] ([id_meal], [name], [description], [price], [id_restaurant]) VALUES (54, N'Stoemp', N'Vegetable mash with sausage.', 13, 4)
GO
INSERT [dbo].[Meal] ([id_meal], [name], [description], [price], [id_restaurant]) VALUES (55, N'Vol-au-vent', N'Belgian puff pastry with filling.', 14.5, 5)
GO
INSERT [dbo].[Meal] ([id_meal], [name], [description], [price], [id_restaurant]) VALUES (56, N'Homemade Fries', N'Crispy Belgian fries.', 6, 5)
GO
INSERT [dbo].[Meal] ([id_meal], [name], [description], [price], [id_restaurant]) VALUES (57, N'Dame Blanche', N'Vanilla ice cream with hot chocolate.', 6.5, 5)
GO
INSERT [dbo].[Meal] ([id_meal], [name], [description], [price], [id_restaurant]) VALUES (58, N'Cuberdon', N'Belgian raspberry candy.', 4, 5)
GO
INSERT [dbo].[Meal] ([id_meal], [name], [description], [price], [id_restaurant]) VALUES (59, N'Chicory Gratin', N'Endives with ham and cheese.', 13.5, 5)
GO
INSERT [dbo].[Meal] ([id_meal], [name], [description], [price], [id_restaurant]) VALUES (60, N'Rabbit in Gueuze', N'Rabbit cooked in beer.', 17, 5)
GO
INSERT [dbo].[Meal] ([id_meal], [name], [description], [price], [id_restaurant]) VALUES (61, N'Shrimp Croquettes', N'Belgian seafood croquettes.', 12, 6)
GO
INSERT [dbo].[Meal] ([id_meal], [name], [description], [price], [id_restaurant]) VALUES (62, N'Eels in Green Sauce', N'Traditional Belgian dish.', 18, 6)
GO
INSERT [dbo].[Meal] ([id_meal], [name], [description], [price], [id_restaurant]) VALUES (63, N'Sugar Tart', N'Sweet Belgian dessert.', 5.5, 6)
GO
INSERT [dbo].[Meal] ([id_meal], [name], [description], [price], [id_restaurant]) VALUES (64, N'Belgian Chocolate', N'Assortment of pralines.', 7, 6)
GO
INSERT [dbo].[Meal] ([id_meal], [name], [description], [price], [id_restaurant]) VALUES (65, N'Asparagus à la Flamande', N'Asparagus with hard-boiled egg.', 14, 6)
GO
INSERT [dbo].[Meal] ([id_meal], [name], [description], [price], [id_restaurant]) VALUES (66, N'White Boudin', N'Light Belgian sausage.', 11.5, 6)
GO
INSERT [dbo].[Meal] ([id_meal], [name], [description], [price], [id_restaurant]) VALUES (67, N'Sauerbraten', N'German marinated roast.', 17, 7)
GO
INSERT [dbo].[Meal] ([id_meal], [name], [description], [price], [id_restaurant]) VALUES (68, N'Bratwurst', N'Grilled sausage with sauerkraut.', 12.5, 7)
GO
INSERT [dbo].[Meal] ([id_meal], [name], [description], [price], [id_restaurant]) VALUES (69, N'Apfelstrudel', N'Apple and cinnamon dessert.', 6, 7)
GO
INSERT [dbo].[Meal] ([id_meal], [name], [description], [price], [id_restaurant]) VALUES (70, N'Pretzel', N'Fresh pretzel.', 3.5, 7)
GO
INSERT [dbo].[Meal] ([id_meal], [name], [description], [price], [id_restaurant]) VALUES (71, N'Schweinehaxe', N'Roasted pork knuckle.', 19, 7)
GO
INSERT [dbo].[Meal] ([id_meal], [name], [description], [price], [id_restaurant]) VALUES (72, N'Kartoffelsalat', N'Potato salad.', 8, 7)
GO
INSERT [dbo].[Meal] ([id_meal], [name], [description], [price], [id_restaurant]) VALUES (73, N'Rouladen', N'Stuffed beef roast.', 16, 8)
GO
INSERT [dbo].[Meal] ([id_meal], [name], [description], [price], [id_restaurant]) VALUES (74, N'Schnitzel', N'Breaded escalope.', 14, 8)
GO
INSERT [dbo].[Meal] ([id_meal], [name], [description], [price], [id_restaurant]) VALUES (75, N'Black Forest Cake', N'Cherry and chocolate cake.', 7.5, 8)
GO
INSERT [dbo].[Meal] ([id_meal], [name], [description], [price], [id_restaurant]) VALUES (76, N'Lebkuchen', N'German gingerbread.', 5, 8)
GO
INSERT [dbo].[Meal] ([id_meal], [name], [description], [price], [id_restaurant]) VALUES (77, N'Spätzle', N'German cheese noodles.', 10.5, 8)
GO
INSERT [dbo].[Meal] ([id_meal], [name], [description], [price], [id_restaurant]) VALUES (78, N'Currywurst', N'Curry sausage.', 9, 8)
GO
INSERT [dbo].[Meal] ([id_meal], [name], [description], [price], [id_restaurant]) VALUES (79, N'Weisswurst', N'Bavarian white sausage.', 11, 9)
GO
INSERT [dbo].[Meal] ([id_meal], [name], [description], [price], [id_restaurant]) VALUES (80, N'Obatzda', N'Bavarian cheese spread.', 8.5, 9)
GO
INSERT [dbo].[Meal] ([id_meal], [name], [description], [price], [id_restaurant]) VALUES (81, N'Kaiserschmarrn', N'Sweet Austrian dessert.', 6.5, 9)
GO
INSERT [dbo].[Meal] ([id_meal], [name], [description], [price], [id_restaurant]) VALUES (82, N'Pretzel', N'Giant pretzel.', 4, 9)
GO
INSERT [dbo].[Meal] ([id_meal], [name], [description], [price], [id_restaurant]) VALUES (83, N'Leberkäse', N'Bavarian meatloaf.', 13.5, 9)
GO
INSERT [dbo].[Meal] ([id_meal], [name], [description], [price], [id_restaurant]) VALUES (84, N'Sauerkraut', N'Fermented cabbage.', 7, 9)
GO
INSERT [dbo].[Meal] ([id_meal], [name], [description], [price], [id_restaurant]) VALUES (85, N'Pizza Margherita', N'Classic Italian pizza.', 12, 10)
GO
INSERT [dbo].[Meal] ([id_meal], [name], [description], [price], [id_restaurant]) VALUES (86, N'Pasta Carbonara', N'Pasta with pancetta and egg.', 14, 10)
GO
INSERT [dbo].[Meal] ([id_meal], [name], [description], [price], [id_restaurant]) VALUES (87, N'Tiramisu', N'Italian mascarpone dessert.', 6.5, 10)
GO
INSERT [dbo].[Meal] ([id_meal], [name], [description], [price], [id_restaurant]) VALUES (88, N'Panna Cotta', N'Vanilla cream dessert.', 5.5, 10)
GO
INSERT [dbo].[Meal] ([id_meal], [name], [description], [price], [id_restaurant]) VALUES (89, N'Lasagna', N'Homemade bolognese lasagna.', 15, 10)
GO
INSERT [dbo].[Meal] ([id_meal], [name], [description], [price], [id_restaurant]) VALUES (90, N'Bruschetta', N'Fresh tomato toast.', 8, 10)
GO
INSERT [dbo].[Meal] ([id_meal], [name], [description], [price], [id_restaurant]) VALUES (91, N'Pizza Quattro Formaggi', N'Four cheese pizza.', 13.5, 11)
GO
INSERT [dbo].[Meal] ([id_meal], [name], [description], [price], [id_restaurant]) VALUES (92, N'Pasta Pesto', N'Pasta with basil and pine nuts.', 12, 11)
GO
INSERT [dbo].[Meal] ([id_meal], [name], [description], [price], [id_restaurant]) VALUES (93, N'Cannoli', N'Crispy Sicilian dessert.', 6, 11)
GO
INSERT [dbo].[Meal] ([id_meal], [name], [description], [price], [id_restaurant]) VALUES (94, N'Gelato', N'Artisanal Italian ice cream.', 5, 11)
GO
INSERT [dbo].[Meal] ([id_meal], [name], [description], [price], [id_restaurant]) VALUES (95, N'Mushroom Risotto', N'Risotto with mushrooms.', 16, 11)
GO
INSERT [dbo].[Meal] ([id_meal], [name], [description], [price], [id_restaurant]) VALUES (96, N'Caprese Salad', N'Tomato and mozzarella salad.', 9.5, 11)
GO
INSERT [dbo].[Meal] ([id_meal], [name], [description], [price], [id_restaurant]) VALUES (97, N'Ossobuco', N'Milanese veal shank.', 18, 12)
GO
INSERT [dbo].[Meal] ([id_meal], [name], [description], [price], [id_restaurant]) VALUES (98, N'Ravioli', N'Ricotta and spinach ravioli.', 14.5, 12)
GO
INSERT [dbo].[Meal] ([id_meal], [name], [description], [price], [id_restaurant]) VALUES (99, N'Zabaione', N'Italian egg dessert.', 6.5, 12)
GO
INSERT [dbo].[Meal] ([id_meal], [name], [description], [price], [id_restaurant]) VALUES (100, N'Amaretti', N'Almond cookies.', 4.5, 12)
GO
INSERT [dbo].[Meal] ([id_meal], [name], [description], [price], [id_restaurant]) VALUES (101, N'Polenta', N'Creamy polenta with herbs.', 10, 12)
GO
INSERT [dbo].[Meal] ([id_meal], [name], [description], [price], [id_restaurant]) VALUES (102, N'Arancini', N'Fried rice balls.', 8.5, 12)
GO
INSERT [dbo].[Meal] ([id_meal], [name], [description], [price], [id_restaurant]) VALUES (103, N'Paella Valenciana', N'Rice with seafood and chicken.', 18, 13)
GO
INSERT [dbo].[Meal] ([id_meal], [name], [description], [price], [id_restaurant]) VALUES (104, N'Spanish Tortilla', N'Potato omelette.', 10, 13)
GO
INSERT [dbo].[Meal] ([id_meal], [name], [description], [price], [id_restaurant]) VALUES (105, N'Churros', N'Fried dessert with chocolate.', 5, 13)
GO
INSERT [dbo].[Meal] ([id_meal], [name], [description], [price], [id_restaurant]) VALUES (106, N'Crema Catalana', N'Orange-flavored dessert cream.', 6, 13)
GO
INSERT [dbo].[Meal] ([id_meal], [name], [description], [price], [id_restaurant]) VALUES (107, N'Croquetas', N'Ham croquettes.', 9, 13)
GO
INSERT [dbo].[Meal] ([id_meal], [name], [description], [price], [id_restaurant]) VALUES (108, N'Patatas Bravas', N'Spicy potatoes.', 7.5, 13)
GO
INSERT [dbo].[Meal] ([id_meal], [name], [description], [price], [id_restaurant]) VALUES (109, N'Garlic Shrimp', N'Shrimp with garlic.', 12, 14)
GO
INSERT [dbo].[Meal] ([id_meal], [name], [description], [price], [id_restaurant]) VALUES (110, N'Albóndigas', N'Meatballs in sauce.', 10.5, 14)
GO
INSERT [dbo].[Meal] ([id_meal], [name], [description], [price], [id_restaurant]) VALUES (111, N'Flan', N'Spanish caramel cream.', 5.5, 14)
GO
INSERT [dbo].[Meal] ([id_meal], [name], [description], [price], [id_restaurant]) VALUES (112, N'Turrón', N'Spanish nougat.', 4.5, 14)
GO
INSERT [dbo].[Meal] ([id_meal], [name], [description], [price], [id_restaurant]) VALUES (113, N'Fried Calamari', N'Crispy calamari.', 11, 14)
GO
INSERT [dbo].[Meal] ([id_meal], [name], [description], [price], [id_restaurant]) VALUES (114, N'Padrón Peppers', N'Grilled peppers.', 8, 14)
GO
INSERT [dbo].[Meal] ([id_meal], [name], [description], [price], [id_restaurant]) VALUES (115, N'Gazpacho', N'Andalusian cold soup.', 7.5, 15)
GO
INSERT [dbo].[Meal] ([id_meal], [name], [description], [price], [id_restaurant]) VALUES (116, N'Asturian Fabada', N'Bean stew.', 13, 15)
GO
INSERT [dbo].[Meal] ([id_meal], [name], [description], [price], [id_restaurant]) VALUES (117, N'Fried Milk', N'Vanilla fried dessert.', 6, 15)
GO
INSERT [dbo].[Meal] ([id_meal], [name], [description], [price], [id_restaurant]) VALUES (118, N'Ensaimada', N'Balearic pastry.', 5, 15)
GO
INSERT [dbo].[Meal] ([id_meal], [name], [description], [price], [id_restaurant]) VALUES (119, N'Chorizo in Wine', N'Chorizo cooked in wine.', 9.5, 15)
GO
INSERT [dbo].[Meal] ([id_meal], [name], [description], [price], [id_restaurant]) VALUES (120, N'Russian Salad', N'Spanish potato salad.', 8, 15)
GO
SET IDENTITY_INSERT [dbo].[Meal] OFF
GO
INSERT [dbo].[Meal_Service] ([id_service], [id_meal]) VALUES (1, 1)
GO
INSERT [dbo].[Meal_Service] ([id_service], [id_meal]) VALUES (1, 2)
GO
INSERT [dbo].[Meal_Service] ([id_service], [id_meal]) VALUES (1, 31)
GO
INSERT [dbo].[Meal_Service] ([id_service], [id_meal]) VALUES (1, 32)
GO
INSERT [dbo].[Meal_Service] ([id_service], [id_meal]) VALUES (1, 33)
GO
INSERT [dbo].[Meal_Service] ([id_service], [id_meal]) VALUES (1, 34)
GO
INSERT [dbo].[Meal_Service] ([id_service], [id_meal]) VALUES (1, 35)
GO
INSERT [dbo].[Meal_Service] ([id_service], [id_meal]) VALUES (1, 36)
GO
INSERT [dbo].[Meal_Service] ([id_service], [id_meal]) VALUES (2, 1)
GO
INSERT [dbo].[Meal_Service] ([id_service], [id_meal]) VALUES (2, 2)
GO
INSERT [dbo].[Meal_Service] ([id_service], [id_meal]) VALUES (2, 31)
GO
INSERT [dbo].[Meal_Service] ([id_service], [id_meal]) VALUES (2, 32)
GO
INSERT [dbo].[Meal_Service] ([id_service], [id_meal]) VALUES (2, 33)
GO
INSERT [dbo].[Meal_Service] ([id_service], [id_meal]) VALUES (2, 34)
GO
INSERT [dbo].[Meal_Service] ([id_service], [id_meal]) VALUES (2, 35)
GO
INSERT [dbo].[Meal_Service] ([id_service], [id_meal]) VALUES (2, 36)
GO
INSERT [dbo].[Meal_Service] ([id_service], [id_meal]) VALUES (3, 3)
GO
INSERT [dbo].[Meal_Service] ([id_service], [id_meal]) VALUES (3, 4)
GO
INSERT [dbo].[Meal_Service] ([id_service], [id_meal]) VALUES (3, 37)
GO
INSERT [dbo].[Meal_Service] ([id_service], [id_meal]) VALUES (3, 38)
GO
INSERT [dbo].[Meal_Service] ([id_service], [id_meal]) VALUES (3, 39)
GO
INSERT [dbo].[Meal_Service] ([id_service], [id_meal]) VALUES (3, 40)
GO
INSERT [dbo].[Meal_Service] ([id_service], [id_meal]) VALUES (3, 41)
GO
INSERT [dbo].[Meal_Service] ([id_service], [id_meal]) VALUES (3, 42)
GO
INSERT [dbo].[Meal_Service] ([id_service], [id_meal]) VALUES (4, 3)
GO
INSERT [dbo].[Meal_Service] ([id_service], [id_meal]) VALUES (4, 4)
GO
INSERT [dbo].[Meal_Service] ([id_service], [id_meal]) VALUES (4, 37)
GO
INSERT [dbo].[Meal_Service] ([id_service], [id_meal]) VALUES (4, 38)
GO
INSERT [dbo].[Meal_Service] ([id_service], [id_meal]) VALUES (4, 39)
GO
INSERT [dbo].[Meal_Service] ([id_service], [id_meal]) VALUES (4, 40)
GO
INSERT [dbo].[Meal_Service] ([id_service], [id_meal]) VALUES (4, 41)
GO
INSERT [dbo].[Meal_Service] ([id_service], [id_meal]) VALUES (4, 42)
GO
INSERT [dbo].[Meal_Service] ([id_service], [id_meal]) VALUES (5, 5)
GO
INSERT [dbo].[Meal_Service] ([id_service], [id_meal]) VALUES (5, 6)
GO
INSERT [dbo].[Meal_Service] ([id_service], [id_meal]) VALUES (5, 43)
GO
INSERT [dbo].[Meal_Service] ([id_service], [id_meal]) VALUES (5, 44)
GO
INSERT [dbo].[Meal_Service] ([id_service], [id_meal]) VALUES (5, 45)
GO
INSERT [dbo].[Meal_Service] ([id_service], [id_meal]) VALUES (5, 46)
GO
INSERT [dbo].[Meal_Service] ([id_service], [id_meal]) VALUES (5, 47)
GO
INSERT [dbo].[Meal_Service] ([id_service], [id_meal]) VALUES (5, 48)
GO
INSERT [dbo].[Meal_Service] ([id_service], [id_meal]) VALUES (6, 5)
GO
INSERT [dbo].[Meal_Service] ([id_service], [id_meal]) VALUES (6, 6)
GO
INSERT [dbo].[Meal_Service] ([id_service], [id_meal]) VALUES (6, 43)
GO
INSERT [dbo].[Meal_Service] ([id_service], [id_meal]) VALUES (6, 44)
GO
INSERT [dbo].[Meal_Service] ([id_service], [id_meal]) VALUES (6, 45)
GO
INSERT [dbo].[Meal_Service] ([id_service], [id_meal]) VALUES (6, 46)
GO
INSERT [dbo].[Meal_Service] ([id_service], [id_meal]) VALUES (6, 47)
GO
INSERT [dbo].[Meal_Service] ([id_service], [id_meal]) VALUES (6, 48)
GO
INSERT [dbo].[Meal_Service] ([id_service], [id_meal]) VALUES (7, 7)
GO
INSERT [dbo].[Meal_Service] ([id_service], [id_meal]) VALUES (7, 8)
GO
INSERT [dbo].[Meal_Service] ([id_service], [id_meal]) VALUES (7, 49)
GO
INSERT [dbo].[Meal_Service] ([id_service], [id_meal]) VALUES (7, 50)
GO
INSERT [dbo].[Meal_Service] ([id_service], [id_meal]) VALUES (7, 51)
GO
INSERT [dbo].[Meal_Service] ([id_service], [id_meal]) VALUES (7, 52)
GO
INSERT [dbo].[Meal_Service] ([id_service], [id_meal]) VALUES (7, 53)
GO
INSERT [dbo].[Meal_Service] ([id_service], [id_meal]) VALUES (7, 54)
GO
INSERT [dbo].[Meal_Service] ([id_service], [id_meal]) VALUES (8, 7)
GO
INSERT [dbo].[Meal_Service] ([id_service], [id_meal]) VALUES (8, 8)
GO
INSERT [dbo].[Meal_Service] ([id_service], [id_meal]) VALUES (8, 49)
GO
INSERT [dbo].[Meal_Service] ([id_service], [id_meal]) VALUES (8, 50)
GO
INSERT [dbo].[Meal_Service] ([id_service], [id_meal]) VALUES (8, 51)
GO
INSERT [dbo].[Meal_Service] ([id_service], [id_meal]) VALUES (8, 52)
GO
INSERT [dbo].[Meal_Service] ([id_service], [id_meal]) VALUES (8, 53)
GO
INSERT [dbo].[Meal_Service] ([id_service], [id_meal]) VALUES (8, 54)
GO
INSERT [dbo].[Meal_Service] ([id_service], [id_meal]) VALUES (9, 9)
GO
INSERT [dbo].[Meal_Service] ([id_service], [id_meal]) VALUES (9, 10)
GO
INSERT [dbo].[Meal_Service] ([id_service], [id_meal]) VALUES (9, 55)
GO
INSERT [dbo].[Meal_Service] ([id_service], [id_meal]) VALUES (9, 56)
GO
INSERT [dbo].[Meal_Service] ([id_service], [id_meal]) VALUES (9, 57)
GO
INSERT [dbo].[Meal_Service] ([id_service], [id_meal]) VALUES (9, 58)
GO
INSERT [dbo].[Meal_Service] ([id_service], [id_meal]) VALUES (9, 59)
GO
INSERT [dbo].[Meal_Service] ([id_service], [id_meal]) VALUES (9, 60)
GO
INSERT [dbo].[Meal_Service] ([id_service], [id_meal]) VALUES (10, 9)
GO
INSERT [dbo].[Meal_Service] ([id_service], [id_meal]) VALUES (10, 10)
GO
INSERT [dbo].[Meal_Service] ([id_service], [id_meal]) VALUES (10, 55)
GO
INSERT [dbo].[Meal_Service] ([id_service], [id_meal]) VALUES (10, 56)
GO
INSERT [dbo].[Meal_Service] ([id_service], [id_meal]) VALUES (10, 57)
GO
INSERT [dbo].[Meal_Service] ([id_service], [id_meal]) VALUES (10, 58)
GO
INSERT [dbo].[Meal_Service] ([id_service], [id_meal]) VALUES (10, 59)
GO
INSERT [dbo].[Meal_Service] ([id_service], [id_meal]) VALUES (10, 60)
GO
INSERT [dbo].[Meal_Service] ([id_service], [id_meal]) VALUES (11, 11)
GO
INSERT [dbo].[Meal_Service] ([id_service], [id_meal]) VALUES (11, 12)
GO
INSERT [dbo].[Meal_Service] ([id_service], [id_meal]) VALUES (11, 61)
GO
INSERT [dbo].[Meal_Service] ([id_service], [id_meal]) VALUES (11, 62)
GO
INSERT [dbo].[Meal_Service] ([id_service], [id_meal]) VALUES (11, 63)
GO
INSERT [dbo].[Meal_Service] ([id_service], [id_meal]) VALUES (11, 64)
GO
INSERT [dbo].[Meal_Service] ([id_service], [id_meal]) VALUES (11, 65)
GO
INSERT [dbo].[Meal_Service] ([id_service], [id_meal]) VALUES (11, 66)
GO
INSERT [dbo].[Meal_Service] ([id_service], [id_meal]) VALUES (12, 11)
GO
INSERT [dbo].[Meal_Service] ([id_service], [id_meal]) VALUES (12, 12)
GO
INSERT [dbo].[Meal_Service] ([id_service], [id_meal]) VALUES (12, 61)
GO
INSERT [dbo].[Meal_Service] ([id_service], [id_meal]) VALUES (12, 62)
GO
INSERT [dbo].[Meal_Service] ([id_service], [id_meal]) VALUES (12, 63)
GO
INSERT [dbo].[Meal_Service] ([id_service], [id_meal]) VALUES (12, 64)
GO
INSERT [dbo].[Meal_Service] ([id_service], [id_meal]) VALUES (12, 65)
GO
INSERT [dbo].[Meal_Service] ([id_service], [id_meal]) VALUES (12, 66)
GO
INSERT [dbo].[Meal_Service] ([id_service], [id_meal]) VALUES (13, 13)
GO
INSERT [dbo].[Meal_Service] ([id_service], [id_meal]) VALUES (13, 14)
GO
INSERT [dbo].[Meal_Service] ([id_service], [id_meal]) VALUES (13, 67)
GO
INSERT [dbo].[Meal_Service] ([id_service], [id_meal]) VALUES (13, 68)
GO
INSERT [dbo].[Meal_Service] ([id_service], [id_meal]) VALUES (13, 69)
GO
INSERT [dbo].[Meal_Service] ([id_service], [id_meal]) VALUES (13, 70)
GO
INSERT [dbo].[Meal_Service] ([id_service], [id_meal]) VALUES (13, 71)
GO
INSERT [dbo].[Meal_Service] ([id_service], [id_meal]) VALUES (13, 72)
GO
INSERT [dbo].[Meal_Service] ([id_service], [id_meal]) VALUES (14, 13)
GO
INSERT [dbo].[Meal_Service] ([id_service], [id_meal]) VALUES (14, 14)
GO
INSERT [dbo].[Meal_Service] ([id_service], [id_meal]) VALUES (14, 67)
GO
INSERT [dbo].[Meal_Service] ([id_service], [id_meal]) VALUES (14, 68)
GO
INSERT [dbo].[Meal_Service] ([id_service], [id_meal]) VALUES (14, 69)
GO
INSERT [dbo].[Meal_Service] ([id_service], [id_meal]) VALUES (14, 70)
GO
INSERT [dbo].[Meal_Service] ([id_service], [id_meal]) VALUES (14, 71)
GO
INSERT [dbo].[Meal_Service] ([id_service], [id_meal]) VALUES (14, 72)
GO
INSERT [dbo].[Meal_Service] ([id_service], [id_meal]) VALUES (15, 15)
GO
INSERT [dbo].[Meal_Service] ([id_service], [id_meal]) VALUES (15, 16)
GO
INSERT [dbo].[Meal_Service] ([id_service], [id_meal]) VALUES (15, 73)
GO
INSERT [dbo].[Meal_Service] ([id_service], [id_meal]) VALUES (15, 74)
GO
INSERT [dbo].[Meal_Service] ([id_service], [id_meal]) VALUES (15, 75)
GO
INSERT [dbo].[Meal_Service] ([id_service], [id_meal]) VALUES (15, 76)
GO
INSERT [dbo].[Meal_Service] ([id_service], [id_meal]) VALUES (15, 77)
GO
INSERT [dbo].[Meal_Service] ([id_service], [id_meal]) VALUES (15, 78)
GO
INSERT [dbo].[Meal_Service] ([id_service], [id_meal]) VALUES (16, 15)
GO
INSERT [dbo].[Meal_Service] ([id_service], [id_meal]) VALUES (16, 16)
GO
INSERT [dbo].[Meal_Service] ([id_service], [id_meal]) VALUES (16, 73)
GO
INSERT [dbo].[Meal_Service] ([id_service], [id_meal]) VALUES (16, 74)
GO
INSERT [dbo].[Meal_Service] ([id_service], [id_meal]) VALUES (16, 75)
GO
INSERT [dbo].[Meal_Service] ([id_service], [id_meal]) VALUES (16, 76)
GO
INSERT [dbo].[Meal_Service] ([id_service], [id_meal]) VALUES (16, 77)
GO
INSERT [dbo].[Meal_Service] ([id_service], [id_meal]) VALUES (16, 78)
GO
INSERT [dbo].[Meal_Service] ([id_service], [id_meal]) VALUES (17, 17)
GO
INSERT [dbo].[Meal_Service] ([id_service], [id_meal]) VALUES (17, 18)
GO
INSERT [dbo].[Meal_Service] ([id_service], [id_meal]) VALUES (17, 79)
GO
INSERT [dbo].[Meal_Service] ([id_service], [id_meal]) VALUES (17, 80)
GO
INSERT [dbo].[Meal_Service] ([id_service], [id_meal]) VALUES (17, 81)
GO
INSERT [dbo].[Meal_Service] ([id_service], [id_meal]) VALUES (17, 82)
GO
INSERT [dbo].[Meal_Service] ([id_service], [id_meal]) VALUES (17, 83)
GO
INSERT [dbo].[Meal_Service] ([id_service], [id_meal]) VALUES (17, 84)
GO
INSERT [dbo].[Meal_Service] ([id_service], [id_meal]) VALUES (18, 17)
GO
INSERT [dbo].[Meal_Service] ([id_service], [id_meal]) VALUES (18, 18)
GO
INSERT [dbo].[Meal_Service] ([id_service], [id_meal]) VALUES (18, 79)
GO
INSERT [dbo].[Meal_Service] ([id_service], [id_meal]) VALUES (18, 80)
GO
INSERT [dbo].[Meal_Service] ([id_service], [id_meal]) VALUES (18, 81)
GO
INSERT [dbo].[Meal_Service] ([id_service], [id_meal]) VALUES (18, 82)
GO
INSERT [dbo].[Meal_Service] ([id_service], [id_meal]) VALUES (18, 83)
GO
INSERT [dbo].[Meal_Service] ([id_service], [id_meal]) VALUES (18, 84)
GO
INSERT [dbo].[Meal_Service] ([id_service], [id_meal]) VALUES (19, 19)
GO
INSERT [dbo].[Meal_Service] ([id_service], [id_meal]) VALUES (19, 20)
GO
INSERT [dbo].[Meal_Service] ([id_service], [id_meal]) VALUES (19, 85)
GO
INSERT [dbo].[Meal_Service] ([id_service], [id_meal]) VALUES (19, 86)
GO
INSERT [dbo].[Meal_Service] ([id_service], [id_meal]) VALUES (19, 87)
GO
INSERT [dbo].[Meal_Service] ([id_service], [id_meal]) VALUES (19, 88)
GO
INSERT [dbo].[Meal_Service] ([id_service], [id_meal]) VALUES (19, 89)
GO
INSERT [dbo].[Meal_Service] ([id_service], [id_meal]) VALUES (19, 90)
GO
INSERT [dbo].[Meal_Service] ([id_service], [id_meal]) VALUES (20, 19)
GO
INSERT [dbo].[Meal_Service] ([id_service], [id_meal]) VALUES (20, 20)
GO
INSERT [dbo].[Meal_Service] ([id_service], [id_meal]) VALUES (20, 85)
GO
INSERT [dbo].[Meal_Service] ([id_service], [id_meal]) VALUES (20, 86)
GO
INSERT [dbo].[Meal_Service] ([id_service], [id_meal]) VALUES (20, 87)
GO
INSERT [dbo].[Meal_Service] ([id_service], [id_meal]) VALUES (20, 88)
GO
INSERT [dbo].[Meal_Service] ([id_service], [id_meal]) VALUES (20, 89)
GO
INSERT [dbo].[Meal_Service] ([id_service], [id_meal]) VALUES (20, 90)
GO
INSERT [dbo].[Meal_Service] ([id_service], [id_meal]) VALUES (21, 21)
GO
INSERT [dbo].[Meal_Service] ([id_service], [id_meal]) VALUES (21, 22)
GO
INSERT [dbo].[Meal_Service] ([id_service], [id_meal]) VALUES (21, 91)
GO
INSERT [dbo].[Meal_Service] ([id_service], [id_meal]) VALUES (21, 92)
GO
INSERT [dbo].[Meal_Service] ([id_service], [id_meal]) VALUES (21, 93)
GO
INSERT [dbo].[Meal_Service] ([id_service], [id_meal]) VALUES (21, 94)
GO
INSERT [dbo].[Meal_Service] ([id_service], [id_meal]) VALUES (21, 95)
GO
INSERT [dbo].[Meal_Service] ([id_service], [id_meal]) VALUES (21, 96)
GO
INSERT [dbo].[Meal_Service] ([id_service], [id_meal]) VALUES (22, 21)
GO
INSERT [dbo].[Meal_Service] ([id_service], [id_meal]) VALUES (22, 22)
GO
INSERT [dbo].[Meal_Service] ([id_service], [id_meal]) VALUES (22, 91)
GO
INSERT [dbo].[Meal_Service] ([id_service], [id_meal]) VALUES (22, 92)
GO
INSERT [dbo].[Meal_Service] ([id_service], [id_meal]) VALUES (22, 93)
GO
INSERT [dbo].[Meal_Service] ([id_service], [id_meal]) VALUES (22, 94)
GO
INSERT [dbo].[Meal_Service] ([id_service], [id_meal]) VALUES (22, 95)
GO
INSERT [dbo].[Meal_Service] ([id_service], [id_meal]) VALUES (22, 96)
GO
INSERT [dbo].[Meal_Service] ([id_service], [id_meal]) VALUES (23, 23)
GO
INSERT [dbo].[Meal_Service] ([id_service], [id_meal]) VALUES (23, 24)
GO
INSERT [dbo].[Meal_Service] ([id_service], [id_meal]) VALUES (23, 97)
GO
INSERT [dbo].[Meal_Service] ([id_service], [id_meal]) VALUES (23, 98)
GO
INSERT [dbo].[Meal_Service] ([id_service], [id_meal]) VALUES (23, 99)
GO
INSERT [dbo].[Meal_Service] ([id_service], [id_meal]) VALUES (23, 100)
GO
INSERT [dbo].[Meal_Service] ([id_service], [id_meal]) VALUES (23, 101)
GO
INSERT [dbo].[Meal_Service] ([id_service], [id_meal]) VALUES (23, 102)
GO
INSERT [dbo].[Meal_Service] ([id_service], [id_meal]) VALUES (24, 23)
GO
INSERT [dbo].[Meal_Service] ([id_service], [id_meal]) VALUES (24, 24)
GO
INSERT [dbo].[Meal_Service] ([id_service], [id_meal]) VALUES (24, 97)
GO
INSERT [dbo].[Meal_Service] ([id_service], [id_meal]) VALUES (24, 98)
GO
INSERT [dbo].[Meal_Service] ([id_service], [id_meal]) VALUES (24, 99)
GO
INSERT [dbo].[Meal_Service] ([id_service], [id_meal]) VALUES (24, 100)
GO
INSERT [dbo].[Meal_Service] ([id_service], [id_meal]) VALUES (24, 101)
GO
INSERT [dbo].[Meal_Service] ([id_service], [id_meal]) VALUES (24, 102)
GO
INSERT [dbo].[Meal_Service] ([id_service], [id_meal]) VALUES (25, 25)
GO
INSERT [dbo].[Meal_Service] ([id_service], [id_meal]) VALUES (25, 26)
GO
INSERT [dbo].[Meal_Service] ([id_service], [id_meal]) VALUES (25, 103)
GO
INSERT [dbo].[Meal_Service] ([id_service], [id_meal]) VALUES (25, 104)
GO
INSERT [dbo].[Meal_Service] ([id_service], [id_meal]) VALUES (25, 105)
GO
INSERT [dbo].[Meal_Service] ([id_service], [id_meal]) VALUES (25, 106)
GO
INSERT [dbo].[Meal_Service] ([id_service], [id_meal]) VALUES (25, 107)
GO
INSERT [dbo].[Meal_Service] ([id_service], [id_meal]) VALUES (25, 108)
GO
INSERT [dbo].[Meal_Service] ([id_service], [id_meal]) VALUES (26, 25)
GO
INSERT [dbo].[Meal_Service] ([id_service], [id_meal]) VALUES (26, 26)
GO
INSERT [dbo].[Meal_Service] ([id_service], [id_meal]) VALUES (26, 103)
GO
INSERT [dbo].[Meal_Service] ([id_service], [id_meal]) VALUES (26, 104)
GO
INSERT [dbo].[Meal_Service] ([id_service], [id_meal]) VALUES (26, 105)
GO
INSERT [dbo].[Meal_Service] ([id_service], [id_meal]) VALUES (26, 106)
GO
INSERT [dbo].[Meal_Service] ([id_service], [id_meal]) VALUES (26, 107)
GO
INSERT [dbo].[Meal_Service] ([id_service], [id_meal]) VALUES (26, 108)
GO
INSERT [dbo].[Meal_Service] ([id_service], [id_meal]) VALUES (27, 27)
GO
INSERT [dbo].[Meal_Service] ([id_service], [id_meal]) VALUES (27, 28)
GO
INSERT [dbo].[Meal_Service] ([id_service], [id_meal]) VALUES (27, 109)
GO
INSERT [dbo].[Meal_Service] ([id_service], [id_meal]) VALUES (27, 110)
GO
INSERT [dbo].[Meal_Service] ([id_service], [id_meal]) VALUES (27, 111)
GO
INSERT [dbo].[Meal_Service] ([id_service], [id_meal]) VALUES (27, 112)
GO
INSERT [dbo].[Meal_Service] ([id_service], [id_meal]) VALUES (27, 113)
GO
INSERT [dbo].[Meal_Service] ([id_service], [id_meal]) VALUES (27, 114)
GO
INSERT [dbo].[Meal_Service] ([id_service], [id_meal]) VALUES (28, 27)
GO
INSERT [dbo].[Meal_Service] ([id_service], [id_meal]) VALUES (28, 28)
GO
INSERT [dbo].[Meal_Service] ([id_service], [id_meal]) VALUES (28, 109)
GO
INSERT [dbo].[Meal_Service] ([id_service], [id_meal]) VALUES (28, 110)
GO
INSERT [dbo].[Meal_Service] ([id_service], [id_meal]) VALUES (28, 111)
GO
INSERT [dbo].[Meal_Service] ([id_service], [id_meal]) VALUES (28, 112)
GO
INSERT [dbo].[Meal_Service] ([id_service], [id_meal]) VALUES (28, 113)
GO
INSERT [dbo].[Meal_Service] ([id_service], [id_meal]) VALUES (28, 114)
GO
INSERT [dbo].[Meal_Service] ([id_service], [id_meal]) VALUES (29, 29)
GO
INSERT [dbo].[Meal_Service] ([id_service], [id_meal]) VALUES (29, 30)
GO
INSERT [dbo].[Meal_Service] ([id_service], [id_meal]) VALUES (29, 115)
GO
INSERT [dbo].[Meal_Service] ([id_service], [id_meal]) VALUES (29, 116)
GO
INSERT [dbo].[Meal_Service] ([id_service], [id_meal]) VALUES (29, 117)
GO
INSERT [dbo].[Meal_Service] ([id_service], [id_meal]) VALUES (29, 118)
GO
INSERT [dbo].[Meal_Service] ([id_service], [id_meal]) VALUES (29, 119)
GO
INSERT [dbo].[Meal_Service] ([id_service], [id_meal]) VALUES (29, 120)
GO
INSERT [dbo].[Meal_Service] ([id_service], [id_meal]) VALUES (30, 29)
GO
INSERT [dbo].[Meal_Service] ([id_service], [id_meal]) VALUES (30, 30)
GO
INSERT [dbo].[Meal_Service] ([id_service], [id_meal]) VALUES (30, 115)
GO
INSERT [dbo].[Meal_Service] ([id_service], [id_meal]) VALUES (30, 116)
GO
INSERT [dbo].[Meal_Service] ([id_service], [id_meal]) VALUES (30, 117)
GO
INSERT [dbo].[Meal_Service] ([id_service], [id_meal]) VALUES (30, 118)
GO
INSERT [dbo].[Meal_Service] ([id_service], [id_meal]) VALUES (30, 119)
GO
INSERT [dbo].[Meal_Service] ([id_service], [id_meal]) VALUES (30, 120)
GO
INSERT [dbo].[Menu] ([id_meal]) VALUES (1)
GO
INSERT [dbo].[Menu] ([id_meal]) VALUES (2)
GO
INSERT [dbo].[Menu] ([id_meal]) VALUES (3)
GO
INSERT [dbo].[Menu] ([id_meal]) VALUES (4)
GO
INSERT [dbo].[Menu] ([id_meal]) VALUES (5)
GO
INSERT [dbo].[Menu] ([id_meal]) VALUES (6)
GO
INSERT [dbo].[Menu] ([id_meal]) VALUES (7)
GO
INSERT [dbo].[Menu] ([id_meal]) VALUES (8)
GO
INSERT [dbo].[Menu] ([id_meal]) VALUES (9)
GO
INSERT [dbo].[Menu] ([id_meal]) VALUES (10)
GO
INSERT [dbo].[Menu] ([id_meal]) VALUES (11)
GO
INSERT [dbo].[Menu] ([id_meal]) VALUES (12)
GO
INSERT [dbo].[Menu] ([id_meal]) VALUES (13)
GO
INSERT [dbo].[Menu] ([id_meal]) VALUES (14)
GO
INSERT [dbo].[Menu] ([id_meal]) VALUES (15)
GO
INSERT [dbo].[Menu] ([id_meal]) VALUES (16)
GO
INSERT [dbo].[Menu] ([id_meal]) VALUES (17)
GO
INSERT [dbo].[Menu] ([id_meal]) VALUES (18)
GO
INSERT [dbo].[Menu] ([id_meal]) VALUES (19)
GO
INSERT [dbo].[Menu] ([id_meal]) VALUES (20)
GO
INSERT [dbo].[Menu] ([id_meal]) VALUES (21)
GO
INSERT [dbo].[Menu] ([id_meal]) VALUES (22)
GO
INSERT [dbo].[Menu] ([id_meal]) VALUES (23)
GO
INSERT [dbo].[Menu] ([id_meal]) VALUES (24)
GO
INSERT [dbo].[Menu] ([id_meal]) VALUES (25)
GO
INSERT [dbo].[Menu] ([id_meal]) VALUES (26)
GO
INSERT [dbo].[Menu] ([id_meal]) VALUES (27)
GO
INSERT [dbo].[Menu] ([id_meal]) VALUES (28)
GO
INSERT [dbo].[Menu] ([id_meal]) VALUES (29)
GO
INSERT [dbo].[Menu] ([id_meal]) VALUES (30)
GO
INSERT [dbo].[Menu_Dish] ([id_menu], [id_dish]) VALUES (1, 31)
GO
INSERT [dbo].[Menu_Dish] ([id_menu], [id_dish]) VALUES (1, 32)
GO
INSERT [dbo].[Menu_Dish] ([id_menu], [id_dish]) VALUES (1, 33)
GO
INSERT [dbo].[Menu_Dish] ([id_menu], [id_dish]) VALUES (2, 31)
GO
INSERT [dbo].[Menu_Dish] ([id_menu], [id_dish]) VALUES (2, 32)
GO
INSERT [dbo].[Menu_Dish] ([id_menu], [id_dish]) VALUES (2, 34)
GO
INSERT [dbo].[Menu_Dish] ([id_menu], [id_dish]) VALUES (3, 37)
GO
INSERT [dbo].[Menu_Dish] ([id_menu], [id_dish]) VALUES (3, 38)
GO
INSERT [dbo].[Menu_Dish] ([id_menu], [id_dish]) VALUES (3, 39)
GO
INSERT [dbo].[Menu_Dish] ([id_menu], [id_dish]) VALUES (4, 37)
GO
INSERT [dbo].[Menu_Dish] ([id_menu], [id_dish]) VALUES (4, 38)
GO
INSERT [dbo].[Menu_Dish] ([id_menu], [id_dish]) VALUES (4, 40)
GO
INSERT [dbo].[Menu_Dish] ([id_menu], [id_dish]) VALUES (5, 43)
GO
INSERT [dbo].[Menu_Dish] ([id_menu], [id_dish]) VALUES (5, 44)
GO
INSERT [dbo].[Menu_Dish] ([id_menu], [id_dish]) VALUES (5, 45)
GO
INSERT [dbo].[Menu_Dish] ([id_menu], [id_dish]) VALUES (6, 43)
GO
INSERT [dbo].[Menu_Dish] ([id_menu], [id_dish]) VALUES (6, 44)
GO
INSERT [dbo].[Menu_Dish] ([id_menu], [id_dish]) VALUES (6, 46)
GO
INSERT [dbo].[Menu_Dish] ([id_menu], [id_dish]) VALUES (7, 49)
GO
INSERT [dbo].[Menu_Dish] ([id_menu], [id_dish]) VALUES (7, 50)
GO
INSERT [dbo].[Menu_Dish] ([id_menu], [id_dish]) VALUES (7, 51)
GO
INSERT [dbo].[Menu_Dish] ([id_menu], [id_dish]) VALUES (8, 49)
GO
INSERT [dbo].[Menu_Dish] ([id_menu], [id_dish]) VALUES (8, 50)
GO
INSERT [dbo].[Menu_Dish] ([id_menu], [id_dish]) VALUES (8, 52)
GO
INSERT [dbo].[Menu_Dish] ([id_menu], [id_dish]) VALUES (9, 55)
GO
INSERT [dbo].[Menu_Dish] ([id_menu], [id_dish]) VALUES (9, 56)
GO
INSERT [dbo].[Menu_Dish] ([id_menu], [id_dish]) VALUES (9, 57)
GO
INSERT [dbo].[Menu_Dish] ([id_menu], [id_dish]) VALUES (10, 55)
GO
INSERT [dbo].[Menu_Dish] ([id_menu], [id_dish]) VALUES (10, 56)
GO
INSERT [dbo].[Menu_Dish] ([id_menu], [id_dish]) VALUES (10, 58)
GO
INSERT [dbo].[Menu_Dish] ([id_menu], [id_dish]) VALUES (11, 61)
GO
INSERT [dbo].[Menu_Dish] ([id_menu], [id_dish]) VALUES (11, 62)
GO
INSERT [dbo].[Menu_Dish] ([id_menu], [id_dish]) VALUES (11, 63)
GO
INSERT [dbo].[Menu_Dish] ([id_menu], [id_dish]) VALUES (12, 61)
GO
INSERT [dbo].[Menu_Dish] ([id_menu], [id_dish]) VALUES (12, 62)
GO
INSERT [dbo].[Menu_Dish] ([id_menu], [id_dish]) VALUES (12, 64)
GO
INSERT [dbo].[Menu_Dish] ([id_menu], [id_dish]) VALUES (13, 67)
GO
INSERT [dbo].[Menu_Dish] ([id_menu], [id_dish]) VALUES (13, 68)
GO
INSERT [dbo].[Menu_Dish] ([id_menu], [id_dish]) VALUES (13, 69)
GO
INSERT [dbo].[Menu_Dish] ([id_menu], [id_dish]) VALUES (14, 67)
GO
INSERT [dbo].[Menu_Dish] ([id_menu], [id_dish]) VALUES (14, 68)
GO
INSERT [dbo].[Menu_Dish] ([id_menu], [id_dish]) VALUES (14, 70)
GO
INSERT [dbo].[Menu_Dish] ([id_menu], [id_dish]) VALUES (15, 73)
GO
INSERT [dbo].[Menu_Dish] ([id_menu], [id_dish]) VALUES (15, 74)
GO
INSERT [dbo].[Menu_Dish] ([id_menu], [id_dish]) VALUES (15, 75)
GO
INSERT [dbo].[Menu_Dish] ([id_menu], [id_dish]) VALUES (16, 73)
GO
INSERT [dbo].[Menu_Dish] ([id_menu], [id_dish]) VALUES (16, 74)
GO
INSERT [dbo].[Menu_Dish] ([id_menu], [id_dish]) VALUES (16, 76)
GO
INSERT [dbo].[Menu_Dish] ([id_menu], [id_dish]) VALUES (17, 79)
GO
INSERT [dbo].[Menu_Dish] ([id_menu], [id_dish]) VALUES (17, 80)
GO
INSERT [dbo].[Menu_Dish] ([id_menu], [id_dish]) VALUES (17, 81)
GO
INSERT [dbo].[Menu_Dish] ([id_menu], [id_dish]) VALUES (18, 80)
GO
INSERT [dbo].[Menu_Dish] ([id_menu], [id_dish]) VALUES (18, 81)
GO
INSERT [dbo].[Menu_Dish] ([id_menu], [id_dish]) VALUES (18, 82)
GO
INSERT [dbo].[Menu_Dish] ([id_menu], [id_dish]) VALUES (19, 85)
GO
INSERT [dbo].[Menu_Dish] ([id_menu], [id_dish]) VALUES (19, 86)
GO
INSERT [dbo].[Menu_Dish] ([id_menu], [id_dish]) VALUES (19, 87)
GO
INSERT [dbo].[Menu_Dish] ([id_menu], [id_dish]) VALUES (20, 85)
GO
INSERT [dbo].[Menu_Dish] ([id_menu], [id_dish]) VALUES (20, 86)
GO
INSERT [dbo].[Menu_Dish] ([id_menu], [id_dish]) VALUES (20, 88)
GO
INSERT [dbo].[Menu_Dish] ([id_menu], [id_dish]) VALUES (21, 91)
GO
INSERT [dbo].[Menu_Dish] ([id_menu], [id_dish]) VALUES (21, 92)
GO
INSERT [dbo].[Menu_Dish] ([id_menu], [id_dish]) VALUES (21, 93)
GO
INSERT [dbo].[Menu_Dish] ([id_menu], [id_dish]) VALUES (22, 91)
GO
INSERT [dbo].[Menu_Dish] ([id_menu], [id_dish]) VALUES (22, 92)
GO
INSERT [dbo].[Menu_Dish] ([id_menu], [id_dish]) VALUES (22, 94)
GO
INSERT [dbo].[Menu_Dish] ([id_menu], [id_dish]) VALUES (23, 97)
GO
INSERT [dbo].[Menu_Dish] ([id_menu], [id_dish]) VALUES (23, 98)
GO
INSERT [dbo].[Menu_Dish] ([id_menu], [id_dish]) VALUES (23, 99)
GO
INSERT [dbo].[Menu_Dish] ([id_menu], [id_dish]) VALUES (24, 97)
GO
INSERT [dbo].[Menu_Dish] ([id_menu], [id_dish]) VALUES (24, 98)
GO
INSERT [dbo].[Menu_Dish] ([id_menu], [id_dish]) VALUES (24, 100)
GO
INSERT [dbo].[Menu_Dish] ([id_menu], [id_dish]) VALUES (25, 103)
GO
INSERT [dbo].[Menu_Dish] ([id_menu], [id_dish]) VALUES (25, 104)
GO
INSERT [dbo].[Menu_Dish] ([id_menu], [id_dish]) VALUES (25, 105)
GO
INSERT [dbo].[Menu_Dish] ([id_menu], [id_dish]) VALUES (26, 103)
GO
INSERT [dbo].[Menu_Dish] ([id_menu], [id_dish]) VALUES (26, 104)
GO
INSERT [dbo].[Menu_Dish] ([id_menu], [id_dish]) VALUES (26, 106)
GO
INSERT [dbo].[Menu_Dish] ([id_menu], [id_dish]) VALUES (27, 109)
GO
INSERT [dbo].[Menu_Dish] ([id_menu], [id_dish]) VALUES (27, 110)
GO
INSERT [dbo].[Menu_Dish] ([id_menu], [id_dish]) VALUES (27, 111)
GO
INSERT [dbo].[Menu_Dish] ([id_menu], [id_dish]) VALUES (28, 109)
GO
INSERT [dbo].[Menu_Dish] ([id_menu], [id_dish]) VALUES (28, 110)
GO
INSERT [dbo].[Menu_Dish] ([id_menu], [id_dish]) VALUES (28, 112)
GO
INSERT [dbo].[Menu_Dish] ([id_menu], [id_dish]) VALUES (29, 115)
GO
INSERT [dbo].[Menu_Dish] ([id_menu], [id_dish]) VALUES (29, 116)
GO
INSERT [dbo].[Menu_Dish] ([id_menu], [id_dish]) VALUES (29, 117)
GO
INSERT [dbo].[Menu_Dish] ([id_menu], [id_dish]) VALUES (30, 115)
GO
INSERT [dbo].[Menu_Dish] ([id_menu], [id_dish]) VALUES (30, 116)
GO
INSERT [dbo].[Menu_Dish] ([id_menu], [id_dish]) VALUES (30, 118)
GO
SET IDENTITY_INSERT [dbo].[Person] ON 
GO
INSERT [dbo].[Person] ([id_person], [email], [password]) VALUES (1, N'jean.dupont@example.com', N'hashed_password_123')
GO
INSERT [dbo].[Person] ([id_person], [email], [password]) VALUES (2, N'marie.leclerc@example.com', N'hashed_password_456')
GO
INSERT [dbo].[Person] ([id_person], [email], [password]) VALUES (3, N'pierre.martin@example.com', N'hashed_password_789')
GO
INSERT [dbo].[Person] ([id_person], [email], [password]) VALUES (4, N'sophie.dubois@example.com', N'hashed_password_012')
GO
INSERT [dbo].[Person] ([id_person], [email], [password]) VALUES (5, N'luc.renaud@example.com', N'hashed_password_345')
GO
INSERT [dbo].[Person] ([id_person], [email], [password]) VALUES (6, N'alice.bonnet@example.com', N'hashed_password_678')
GO
INSERT [dbo].[Person] ([id_person], [email], [password]) VALUES (7, N'bob.leroy@example.com', N'hashed_password_901')
GO
INSERT [dbo].[Person] ([id_person], [email], [password]) VALUES (8, N'claire.durand@example.com', N'hashed_password_234')
GO
INSERT [dbo].[Person] ([id_person], [email], [password]) VALUES (9, N'david.fournier@example.com', N'hashed_password_567')
GO
INSERT [dbo].[Person] ([id_person], [email], [password]) VALUES (10, N'emma.roux@example.com', N'hashed_password_890')
GO
INSERT [dbo].[Person] ([id_person], [email], [password]) VALUES (11, N'felix.girard@example.com', N'hashed_password_1234')
GO
INSERT [dbo].[Person] ([id_person], [email], [password]) VALUES (12, N'gabrielle.noel@example.com', N'hashed_password_5678')
GO
INSERT [dbo].[Person] ([id_person], [email], [password]) VALUES (13, N'hugo.lambert@example.com', N'hashed_password_9012')
GO
INSERT [dbo].[Person] ([id_person], [email], [password]) VALUES (14, N'isabelle.morin@example.com', N'hashed_password_3456')
GO
INSERT [dbo].[Person] ([id_person], [email], [password]) VALUES (15, N'julien.petit@example.com', N'hashed_password_7890')
GO
INSERT [dbo].[Person] ([id_person], [email], [password]) VALUES (16, N'aze@hotmail.com', N'aze')
GO
SET IDENTITY_INSERT [dbo].[Person] OFF
GO
SET IDENTITY_INSERT [dbo].[Restaurant] ON 
GO
INSERT [dbo].[Restaurant] ([id_restaurant], [name], [description], [phoneNumber], [id_address], [id_person]) VALUES (1, N'Chez Jean', N'Traditional French cuisine with fresh ingredients.', N'0123456789', 1, 1)
GO
INSERT [dbo].[Restaurant] ([id_restaurant], [name], [description], [phoneNumber], [id_address], [id_person]) VALUES (2, N'Le Bistrot Parisien', N'Friendly atmosphere and French dishes.', N'0123456790', 2, 1)
GO
INSERT [dbo].[Restaurant] ([id_restaurant], [name], [description], [phoneNumber], [id_address], [id_person]) VALUES (3, N'La Brasserie Dupont', N'French regional specialties.', N'0123456791', 3, 1)
GO
INSERT [dbo].[Restaurant] ([id_restaurant], [name], [description], [phoneNumber], [id_address], [id_person]) VALUES (4, N'La Table Belge', N'Belgian specialties in a warm atmosphere.', N'0987654321', 4, 2)
GO
INSERT [dbo].[Restaurant] ([id_restaurant], [name], [description], [phoneNumber], [id_address], [id_person]) VALUES (5, N'Chez Marie', N'Modern Belgian cuisine.', N'0987654322', 5, 2)
GO
INSERT [dbo].[Restaurant] ([id_restaurant], [name], [description], [phoneNumber], [id_address], [id_person]) VALUES (6, N'Brasserie Leclerc', N'Traditional Belgian dishes.', N'0987654323', 6, 2)
GO
INSERT [dbo].[Restaurant] ([id_restaurant], [name], [description], [phoneNumber], [id_address], [id_person]) VALUES (7, N'Zum Löwen', N'Hearty German dishes and craft beers.', N'0123456792', 7, 3)
GO
INSERT [dbo].[Restaurant] ([id_restaurant], [name], [description], [phoneNumber], [id_address], [id_person]) VALUES (8, N'Der Adler', N'Authentic German cuisine.', N'0123456793', 8, 3)
GO
INSERT [dbo].[Restaurant] ([id_restaurant], [name], [description], [phoneNumber], [id_address], [id_person]) VALUES (9, N'Biergarten Martin', N'Bavarian atmosphere and hearty dishes.', N'0123456794', 9, 3)
GO
INSERT [dbo].[Restaurant] ([id_restaurant], [name], [description], [phoneNumber], [id_address], [id_person]) VALUES (10, N'Trattoria Roma', N'Authentic Italian cuisine.', N'0987654324', 10, 4)
GO
INSERT [dbo].[Restaurant] ([id_restaurant], [name], [description], [phoneNumber], [id_address], [id_person]) VALUES (11, N'Pizzeria Bella', N'Artisanal Italian pizzas.', N'0987654325', 11, 4)
GO
INSERT [dbo].[Restaurant] ([id_restaurant], [name], [description], [phoneNumber], [id_address], [id_person]) VALUES (12, N'Osteria Dubois', N'Refined Italian dishes.', N'0987654326', 12, 4)
GO
INSERT [dbo].[Restaurant] ([id_restaurant], [name], [description], [phoneNumber], [id_address], [id_person]) VALUES (13, N'El Sabor', N'Vibrant Spanish flavors.', N'0123456795', 13, 5)
GO
INSERT [dbo].[Restaurant] ([id_restaurant], [name], [description], [phoneNumber], [id_address], [id_person]) VALUES (14, N'Tapas Renaud', N'Tapas and Andalusian atmosphere.', N'0123456796', 14, 5)
GO
INSERT [dbo].[Restaurant] ([id_restaurant], [name], [description], [phoneNumber], [id_address], [id_person]) VALUES (15, N'La Bodega', N'Festive Spanish cuisine.', N'0123456797', 15, 5)
GO
SET IDENTITY_INSERT [dbo].[Restaurant] OFF
GO
INSERT [dbo].[RestaurantOwner] ([id_person], [name]) VALUES (1, N'Jean Dupont')
GO
INSERT [dbo].[RestaurantOwner] ([id_person], [name]) VALUES (2, N'Marie Leclerc')
GO
INSERT [dbo].[RestaurantOwner] ([id_person], [name]) VALUES (3, N'Pierre Martin')
GO
INSERT [dbo].[RestaurantOwner] ([id_person], [name]) VALUES (4, N'Sophie Dubois')
GO
INSERT [dbo].[RestaurantOwner] ([id_person], [name]) VALUES (5, N'Luc Renaud')
GO
INSERT [dbo].[RestaurantOwner] ([id_person], [name]) VALUES (16, N'aze')
GO
SET IDENTITY_INSERT [dbo].[Service] ON 
GO
INSERT [dbo].[Service] ([id_service], [startTime], [endTime], [id_restaurant]) VALUES (1, CAST(N'12:00:00' AS Time), CAST(N'14:00:00' AS Time), 1)
GO
INSERT [dbo].[Service] ([id_service], [startTime], [endTime], [id_restaurant]) VALUES (2, CAST(N'19:00:00' AS Time), CAST(N'22:00:00' AS Time), 1)
GO
INSERT [dbo].[Service] ([id_service], [startTime], [endTime], [id_restaurant]) VALUES (3, CAST(N'12:30:00' AS Time), CAST(N'14:30:00' AS Time), 2)
GO
INSERT [dbo].[Service] ([id_service], [startTime], [endTime], [id_restaurant]) VALUES (4, CAST(N'18:30:00' AS Time), CAST(N'21:30:00' AS Time), 2)
GO
INSERT [dbo].[Service] ([id_service], [startTime], [endTime], [id_restaurant]) VALUES (5, CAST(N'11:45:00' AS Time), CAST(N'13:45:00' AS Time), 3)
GO
INSERT [dbo].[Service] ([id_service], [startTime], [endTime], [id_restaurant]) VALUES (6, CAST(N'19:15:00' AS Time), CAST(N'22:15:00' AS Time), 3)
GO
INSERT [dbo].[Service] ([id_service], [startTime], [endTime], [id_restaurant]) VALUES (7, CAST(N'11:30:00' AS Time), CAST(N'13:30:00' AS Time), 4)
GO
INSERT [dbo].[Service] ([id_service], [startTime], [endTime], [id_restaurant]) VALUES (8, CAST(N'18:30:00' AS Time), CAST(N'21:30:00' AS Time), 4)
GO
INSERT [dbo].[Service] ([id_service], [startTime], [endTime], [id_restaurant]) VALUES (9, CAST(N'12:15:00' AS Time), CAST(N'14:15:00' AS Time), 5)
GO
INSERT [dbo].[Service] ([id_service], [startTime], [endTime], [id_restaurant]) VALUES (10, CAST(N'19:00:00' AS Time), CAST(N'22:00:00' AS Time), 5)
GO
INSERT [dbo].[Service] ([id_service], [startTime], [endTime], [id_restaurant]) VALUES (11, CAST(N'12:00:00' AS Time), CAST(N'14:00:00' AS Time), 6)
GO
INSERT [dbo].[Service] ([id_service], [startTime], [endTime], [id_restaurant]) VALUES (12, CAST(N'18:45:00' AS Time), CAST(N'21:45:00' AS Time), 6)
GO
INSERT [dbo].[Service] ([id_service], [startTime], [endTime], [id_restaurant]) VALUES (13, CAST(N'12:00:00' AS Time), CAST(N'14:30:00' AS Time), 7)
GO
INSERT [dbo].[Service] ([id_service], [startTime], [endTime], [id_restaurant]) VALUES (14, CAST(N'18:00:00' AS Time), CAST(N'21:00:00' AS Time), 7)
GO
INSERT [dbo].[Service] ([id_service], [startTime], [endTime], [id_restaurant]) VALUES (15, CAST(N'11:30:00' AS Time), CAST(N'13:30:00' AS Time), 8)
GO
INSERT [dbo].[Service] ([id_service], [startTime], [endTime], [id_restaurant]) VALUES (16, CAST(N'18:30:00' AS Time), CAST(N'21:30:00' AS Time), 8)
GO
INSERT [dbo].[Service] ([id_service], [startTime], [endTime], [id_restaurant]) VALUES (17, CAST(N'12:15:00' AS Time), CAST(N'14:15:00' AS Time), 9)
GO
INSERT [dbo].[Service] ([id_service], [startTime], [endTime], [id_restaurant]) VALUES (18, CAST(N'19:00:00' AS Time), CAST(N'22:00:00' AS Time), 9)
GO
INSERT [dbo].[Service] ([id_service], [startTime], [endTime], [id_restaurant]) VALUES (19, CAST(N'12:30:00' AS Time), CAST(N'15:00:00' AS Time), 10)
GO
INSERT [dbo].[Service] ([id_service], [startTime], [endTime], [id_restaurant]) VALUES (20, CAST(N'19:30:00' AS Time), CAST(N'22:30:00' AS Time), 10)
GO
INSERT [dbo].[Service] ([id_service], [startTime], [endTime], [id_restaurant]) VALUES (21, CAST(N'12:00:00' AS Time), CAST(N'14:00:00' AS Time), 11)
GO
INSERT [dbo].[Service] ([id_service], [startTime], [endTime], [id_restaurant]) VALUES (22, CAST(N'19:00:00' AS Time), CAST(N'22:00:00' AS Time), 11)
GO
INSERT [dbo].[Service] ([id_service], [startTime], [endTime], [id_restaurant]) VALUES (23, CAST(N'12:45:00' AS Time), CAST(N'14:45:00' AS Time), 12)
GO
INSERT [dbo].[Service] ([id_service], [startTime], [endTime], [id_restaurant]) VALUES (24, CAST(N'18:45:00' AS Time), CAST(N'21:45:00' AS Time), 12)
GO
INSERT [dbo].[Service] ([id_service], [startTime], [endTime], [id_restaurant]) VALUES (25, CAST(N'13:00:00' AS Time), CAST(N'15:30:00' AS Time), 13)
GO
INSERT [dbo].[Service] ([id_service], [startTime], [endTime], [id_restaurant]) VALUES (26, CAST(N'20:00:00' AS Time), CAST(N'23:00:00' AS Time), 13)
GO
INSERT [dbo].[Service] ([id_service], [startTime], [endTime], [id_restaurant]) VALUES (27, CAST(N'12:30:00' AS Time), CAST(N'14:30:00' AS Time), 14)
GO
INSERT [dbo].[Service] ([id_service], [startTime], [endTime], [id_restaurant]) VALUES (28, CAST(N'19:30:00' AS Time), CAST(N'22:30:00' AS Time), 14)
GO
INSERT [dbo].[Service] ([id_service], [startTime], [endTime], [id_restaurant]) VALUES (29, CAST(N'12:15:00' AS Time), CAST(N'14:15:00' AS Time), 15)
GO
INSERT [dbo].[Service] ([id_service], [startTime], [endTime], [id_restaurant]) VALUES (30, CAST(N'19:15:00' AS Time), CAST(N'22:15:00' AS Time), 15)
GO
SET IDENTITY_INSERT [dbo].[Service] OFF
GO
ALTER TABLE [dbo].[Client]  WITH CHECK ADD FOREIGN KEY([id_address])
REFERENCES [dbo].[Address] ([id_address])
GO
ALTER TABLE [dbo].[Client]  WITH CHECK ADD FOREIGN KEY([id_person])
REFERENCES [dbo].[Person] ([id_person])
GO
ALTER TABLE [dbo].[ClientOrder]  WITH CHECK ADD FOREIGN KEY([id_person])
REFERENCES [dbo].[Client] ([id_person])
GO
ALTER TABLE [dbo].[ClientOrder]  WITH CHECK ADD FOREIGN KEY([id_restaurant])
REFERENCES [dbo].[Restaurant] ([id_restaurant])
GO
ALTER TABLE [dbo].[ClientOrder]  WITH CHECK ADD FOREIGN KEY([id_service])
REFERENCES [dbo].[Service] ([id_service])
GO
ALTER TABLE [dbo].[ClientOrder_Meal]  WITH CHECK ADD FOREIGN KEY([id_meal])
REFERENCES [dbo].[Meal] ([id_meal])
GO
ALTER TABLE [dbo].[ClientOrder_Meal]  WITH CHECK ADD FOREIGN KEY([orderNumber])
REFERENCES [dbo].[ClientOrder] ([orderNumber])
GO
ALTER TABLE [dbo].[Dish]  WITH CHECK ADD FOREIGN KEY([id_meal])
REFERENCES [dbo].[Meal] ([id_meal])
GO
ALTER TABLE [dbo].[Meal]  WITH CHECK ADD FOREIGN KEY([id_restaurant])
REFERENCES [dbo].[Restaurant] ([id_restaurant])
GO
ALTER TABLE [dbo].[Meal_Service]  WITH CHECK ADD FOREIGN KEY([id_meal])
REFERENCES [dbo].[Meal] ([id_meal])
GO
ALTER TABLE [dbo].[Meal_Service]  WITH CHECK ADD FOREIGN KEY([id_service])
REFERENCES [dbo].[Service] ([id_service])
GO
ALTER TABLE [dbo].[Menu]  WITH CHECK ADD FOREIGN KEY([id_meal])
REFERENCES [dbo].[Meal] ([id_meal])
GO
ALTER TABLE [dbo].[Menu_Dish]  WITH CHECK ADD FOREIGN KEY([id_dish])
REFERENCES [dbo].[Dish] ([id_meal])
GO
ALTER TABLE [dbo].[Menu_Dish]  WITH CHECK ADD FOREIGN KEY([id_menu])
REFERENCES [dbo].[Menu] ([id_meal])
GO
ALTER TABLE [dbo].[Restaurant]  WITH CHECK ADD FOREIGN KEY([id_address])
REFERENCES [dbo].[Address] ([id_address])
GO
ALTER TABLE [dbo].[Restaurant]  WITH CHECK ADD FOREIGN KEY([id_person])
REFERENCES [dbo].[RestaurantOwner] ([id_person])
GO
ALTER TABLE [dbo].[RestaurantOwner]  WITH CHECK ADD FOREIGN KEY([id_person])
REFERENCES [dbo].[Person] ([id_person])
GO
ALTER TABLE [dbo].[Service]  WITH CHECK ADD FOREIGN KEY([id_restaurant])
REFERENCES [dbo].[Restaurant] ([id_restaurant])
GO
USE [master]
GO
ALTER DATABASE [GroffierKhloufi] SET  READ_WRITE 
GO
