/****** Object:  Table [dbo].[ServerAction]    Script Date: 10/07/2013 15:51:05 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[ServerAction](
	[ServerActionId] [int] IDENTITY(1,1) NOT NULL,
	[Function] [varchar](50) NOT NULL,
	[UserId] [uniqueidentifier] NULL,
	[UserName] [varchar](50) NULL,
	[EntityChanged] [varchar](50) NULL,
	[FieldChanged] [varchar](50) NULL,
	[OriginalValue] [varchar](250) NULL,
	[NewValue] [varchar](250) NULL,
	[Comments] [varchar](250) NULL,
	[DateCreated] [datetime] NOT NULL,
 CONSTRAINT [PK_ServerAction] PRIMARY KEY CLUSTERED 
(
	[ServerActionId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

ALTER TABLE [dbo].[ServerAction]  WITH CHECK ADD  CONSTRAINT [FK_ServerAction_User] FOREIGN KEY([UserId])
REFERENCES [dbo].[User] ([UserId])
ON UPDATE CASCADE
ON DELETE SET NULL
GO

ALTER TABLE [dbo].[ServerAction] CHECK CONSTRAINT [FK_ServerAction_User]
GO


