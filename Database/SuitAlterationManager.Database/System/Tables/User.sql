CREATE TABLE [System].[User]
(
	[Id] [uniqueidentifier] NOT NULL,
	[Password] [nvarchar](500) NOT NULL,
	[Email] [nvarchar](150) NOT NULL,
	CONSTRAINT [PK_User] PRIMARY KEY CLUSTERED ([Id] ASC),
)