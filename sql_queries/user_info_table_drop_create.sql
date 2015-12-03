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

