USE [master]
GO

/****** Object:  Database [hodge_podge]    Script Date: 12/2/2015 6:39:54 PM ******/
DROP DATABASE [hodge_podge]
GO

/****** Object:  Database [hodge_podge]    Script Date: 12/2/2015 6:39:54 PM ******/
CREATE DATABASE [hodge_podge]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'hodge_podge', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL12.MSSQLSERVER\MSSQL\DATA\hodge_podge.mdf' , SIZE = 4096KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'hodge_podge_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL12.MSSQLSERVER\MSSQL\DATA\hodge_podge_log.ldf' , SIZE = 1024KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
GO

ALTER DATABASE [hodge_podge] SET COMPATIBILITY_LEVEL = 120
GO

IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [hodge_podge].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO

ALTER DATABASE [hodge_podge] SET ANSI_NULL_DEFAULT OFF 
GO

ALTER DATABASE [hodge_podge] SET ANSI_NULLS OFF 
GO

ALTER DATABASE [hodge_podge] SET ANSI_PADDING OFF 
GO

ALTER DATABASE [hodge_podge] SET ANSI_WARNINGS OFF 
GO

ALTER DATABASE [hodge_podge] SET ARITHABORT OFF 
GO

ALTER DATABASE [hodge_podge] SET AUTO_CLOSE OFF 
GO

ALTER DATABASE [hodge_podge] SET AUTO_SHRINK OFF 
GO

ALTER DATABASE [hodge_podge] SET AUTO_UPDATE_STATISTICS ON 
GO

ALTER DATABASE [hodge_podge] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO

ALTER DATABASE [hodge_podge] SET CURSOR_DEFAULT  GLOBAL 
GO

ALTER DATABASE [hodge_podge] SET CONCAT_NULL_YIELDS_NULL OFF 
GO

ALTER DATABASE [hodge_podge] SET NUMERIC_ROUNDABORT OFF 
GO

ALTER DATABASE [hodge_podge] SET QUOTED_IDENTIFIER OFF 
GO

ALTER DATABASE [hodge_podge] SET RECURSIVE_TRIGGERS OFF 
GO

ALTER DATABASE [hodge_podge] SET  DISABLE_BROKER 
GO

ALTER DATABASE [hodge_podge] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO

ALTER DATABASE [hodge_podge] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO

ALTER DATABASE [hodge_podge] SET TRUSTWORTHY OFF 
GO

ALTER DATABASE [hodge_podge] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO

ALTER DATABASE [hodge_podge] SET PARAMETERIZATION SIMPLE 
GO

ALTER DATABASE [hodge_podge] SET READ_COMMITTED_SNAPSHOT OFF 
GO

ALTER DATABASE [hodge_podge] SET HONOR_BROKER_PRIORITY OFF 
GO

ALTER DATABASE [hodge_podge] SET RECOVERY FULL 
GO

ALTER DATABASE [hodge_podge] SET  MULTI_USER 
GO

ALTER DATABASE [hodge_podge] SET PAGE_VERIFY CHECKSUM  
GO

ALTER DATABASE [hodge_podge] SET DB_CHAINING OFF 
GO

ALTER DATABASE [hodge_podge] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO

ALTER DATABASE [hodge_podge] SET TARGET_RECOVERY_TIME = 0 SECONDS 
GO

ALTER DATABASE [hodge_podge] SET DELAYED_DURABILITY = DISABLED 
GO

ALTER DATABASE [hodge_podge] SET  READ_WRITE 
GO


USE [hodge_podge]
GO

/****** Object:  Table [dbo].[user_info_table]    Script Date: 12/2/2015 6:50:27 PM ******/
DROP TABLE [dbo].[user_info_table]
GO

/****** Object:  Table [dbo].[user_info_table]    Script Date: 12/2/2015 6:50:27 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[user_info_table](
	[user_ID] [nvarchar](60) NOT NULL,
	[Facebook_email] [nvarchar](60) NOT NULL,
 CONSTRAINT [PK_user_info_table] PRIMARY KEY CLUSTERED 
(
	[user_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

USE [hodge_podge]
GO

ALTER TABLE [dbo].[stock_notes_table] DROP CONSTRAINT [FK_stock_notes_table_user_info_table]
GO

/****** Object:  Table [dbo].[stock_notes_table]    Script Date: 12/2/2015 6:49:34 PM ******/
DROP TABLE [dbo].[stock_notes_table]
GO

/****** Object:  Table [dbo].[stock_notes_table]    Script Date: 12/2/2015 6:49:34 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[stock_notes_table](
	[note_ID] [int] NOT NULL,
	[note_content] [nvarchar](255) NOT NULL,
	[ticker_symbol] [nvarchar](6) NOT NULL,
	[user_ID] [nvarchar](60) NOT NULL,
 CONSTRAINT [PK_stock_notes_table] PRIMARY KEY CLUSTERED 
(
	[note_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[stock_notes_table]  WITH CHECK ADD  CONSTRAINT [FK_stock_notes_table_user_info_table] FOREIGN KEY([user_ID])
REFERENCES [dbo].[user_info_table] ([user_ID])
GO

ALTER TABLE [dbo].[stock_notes_table] CHECK CONSTRAINT [FK_stock_notes_table_user_info_table]
GO

USE [hodge_podge]
GO

ALTER TABLE [dbo].[log_table] DROP CONSTRAINT [FK_log_table_user_info_table]
GO

/****** Object:  Table [dbo].[log_table]    Script Date: 12/2/2015 6:49:50 PM ******/
DROP TABLE [dbo].[log_table]
GO

/****** Object:  Table [dbo].[log_table]    Script Date: 12/2/2015 6:49:50 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[log_table](
	[log_ID] [int] NOT NULL,
	[login_datetime] [datetime] NOT NULL,
	[logout_datetime] [datetime] NOT NULL,
	[user_ID] [nvarchar](60) NOT NULL,
 CONSTRAINT [PK_log_table] PRIMARY KEY CLUSTERED 
(
	[log_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[log_table]  WITH CHECK ADD  CONSTRAINT [FK_log_table_user_info_table] FOREIGN KEY([user_ID])
REFERENCES [dbo].[user_info_table] ([user_ID])
GO

ALTER TABLE [dbo].[log_table] CHECK CONSTRAINT [FK_log_table_user_info_table]
GO

USE [hodge_podge]
GO

ALTER TABLE [dbo].[event_table] DROP CONSTRAINT [FK_event_table_user_info_table]
GO

/****** Object:  Table [dbo].[event_table]    Script Date: 12/2/2015 6:50:12 PM ******/
DROP TABLE [dbo].[event_table]
GO

/****** Object:  Table [dbo].[event_table]    Script Date: 12/2/2015 6:50:12 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[event_table](
	[event_ID] [int] NOT NULL,
	[event_name] [nvarchar](20) NOT NULL,
	[start_datetime] [datetime] NOT NULL,
	[end_datetime] [datetime] NOT NULL,
	[event_description] [nvarchar](255) NULL,
	[user_ID] [nvarchar](60) NOT NULL,
 CONSTRAINT [PK_event_table] PRIMARY KEY CLUSTERED 
(
	[event_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[event_table]  WITH CHECK ADD  CONSTRAINT [FK_event_table_user_info_table] FOREIGN KEY([user_ID])
REFERENCES [dbo].[user_info_table] ([user_ID])
GO

ALTER TABLE [dbo].[event_table] CHECK CONSTRAINT [FK_event_table_user_info_table]
GO

USE [hodge_podge]
GO

/****** Object:  Table [dbo].[chat_table]    Script Date: 12/2/2015 6:48:51 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[chat_table](
	[message_ID] [int] NOT NULL,
	[message_content] [nvarchar](255) NOT NULL,
	[message_timestamp] [datetime] NOT NULL,
	[user_ID] [nvarchar](60) NOT NULL,
 CONSTRAINT [PK_chat_table] PRIMARY KEY CLUSTERED 
(
	[message_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[chat_table]  WITH CHECK ADD  CONSTRAINT [FK_chat_table_user_info_table] FOREIGN KEY([user_ID])
REFERENCES [dbo].[user_info_table] ([user_ID])
GO

ALTER TABLE [dbo].[chat_table] CHECK CONSTRAINT [FK_chat_table_user_info_table]
GO

USE [hodge_podge]
GO

ALTER TABLE [dbo].[stock_transaction_table] DROP CONSTRAINT [FK_stock_transaction_table_user_info_table]
GO

/****** Object:  Table [dbo].[stock_transaction_table]    Script Date: 12/2/2015 6:49:14 PM ******/
DROP TABLE [dbo].[stock_transaction_table]
GO

/****** Object:  Table [dbo].[stock_transaction_table]    Script Date: 12/2/2015 6:49:14 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[stock_transaction_table](
	[transaction_ID] [int] NOT NULL,
	[ticker_symbol] [nvarchar](6) NOT NULL,
	[price] [numeric](18, 0) NOT NULL,
	[number_of_shares] [int] NOT NULL,
	[transaction_datetime] [datetime] NOT NULL,
	[user_ID] [nvarchar](60) NOT NULL,
 CONSTRAINT [PK_stock_transaction_table] PRIMARY KEY CLUSTERED 
(
	[transaction_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[stock_transaction_table]  WITH CHECK ADD  CONSTRAINT [FK_stock_transaction_table_user_info_table] FOREIGN KEY([user_ID])
REFERENCES [dbo].[user_info_table] ([user_ID])
GO

ALTER TABLE [dbo].[stock_transaction_table] CHECK CONSTRAINT [FK_stock_transaction_table_user_info_table]
GO

