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

