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

