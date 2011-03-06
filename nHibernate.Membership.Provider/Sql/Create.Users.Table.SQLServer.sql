/****** Object:  Table [dbo].[Users]    Script Date: 01/26/2011 21:53:13 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[User](
	[ID] [uniqueidentifier] NOT NULL,
	[Username] [nvarchar](255) NOT NULL,
	[ApplicationName] [nvarchar](255) NOT NULL,
	[Email] [nvarchar](128) NOT NULL,
	[Comment] [nvarchar](255) NULL,
	[Password] [nvarchar](128) NOT NULL,
	[PasswordQuestion] [nvarchar](255) NULL,
	[PasswordAnswer] [nvarchar](255) NULL,
	[IsApproved] [bit] NULL,
	[LastActivityDate] [datetime] NULL,
	[LastLoginDate] [datetime] NULL,
	[LastPasswordChangedDate] [datetime] NULL,
	[CreationDate] [datetime] NULL,
	[IsOnLine] [bit] NULL,
	[IsLockedOut] [bit] NULL,
	[LastLockedOutDate] [datetime] NULL,
	[FailedPasswordAttemptCount] [int] NULL,
	[FailedPasswordAttemptWindowStart] [datetime] NULL,
	[FailedPasswordAnswerAttemptCount] [int] NULL,
	[FailedPasswordAnswerAttemptWindowStart] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[PKID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

