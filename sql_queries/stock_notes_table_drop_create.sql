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

