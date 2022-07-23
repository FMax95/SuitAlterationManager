CREATE TABLE [System].[User]
(
	[Id] [uniqueidentifier] NOT NULL,
	[Password] [nvarchar](500) NOT NULL,
	[Email] [nvarchar](150) NOT NULL,
	[IsEnabled] [bit] NOT NULL,
	[IsDeleted] [bit] NOT NULL,
	[UpdateDate] [datetimeoffset](7) NOT NULL,
	[DatabaseVersion] [timestamp] NOT NULL,
	[ResetTokenExpirationDate] [datetimeoffset](7) NULL,
	[PasswordResetDate] [datetimeoffset](7) NULL,
	[ResetToken] [nvarchar](500) NULL,
	CONSTRAINT [PK_User] PRIMARY KEY CLUSTERED ([Id] ASC)
)