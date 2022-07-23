CREATE TABLE [System].[UserInformation]
(
	[IdUser] [uniqueidentifier] NOT NULL,
	[FirstName] [varchar] (200) NULL,
	[LastName] [varchar] (200) NULL,
	[Image] [varchar] (200) NULL,
	[BirthDate] datetime NULL, 
    CONSTRAINT [PK_UserInformation] PRIMARY KEY CLUSTERED ([IdUser] ASC),
	CONSTRAINT [FK_UserInformation_IdUser] FOREIGN KEY([IdUser]) REFERENCES [System].[User] ([Id])
)