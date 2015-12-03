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

