/****** Object:  Table [dbo].[ServerError]    Script Date: 10/07/2013 15:51:36 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[ServerError](
	[ServerErrorId] [int] IDENTITY(1,1) NOT NULL,
	[ErrorMessage] [varchar](500) NOT NULL,
	[UserId] [uniqueidentifier] NULL,
	[UserName] [varchar](50) NULL,
	[Comments] [varchar](250) NULL,
	[DateCreated] [datetime] NOT NULL,
 CONSTRAINT [PK_ErrorLog_1] PRIMARY KEY CLUSTERED 
(
	[ServerErrorId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

ALTER TABLE [dbo].[ServerError]  WITH CHECK ADD  CONSTRAINT [FK_ServerError_User] FOREIGN KEY([UserId])
REFERENCES [dbo].[User] ([UserId])
ON UPDATE CASCADE
ON DELETE SET NULL
GO

ALTER TABLE [dbo].[ServerError] CHECK CONSTRAINT [FK_ServerError_User]
GO


