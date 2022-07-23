CREATE TABLE [System].[RefreshToken]
(
	[Id] [uniqueidentifier] NOT NULL,
	[IdUser] [uniqueidentifier] NOT NULL,
	[Token] [nvarchar](500) NOT NULL,
	[ExpirationDate] [datetimeoffset](7) NOT NULL,
	[CreationDate] [datetimeoffset](7) NOT NULL,
	[CreatedByIp] [varchar](45) NOT NULL,
	[RevocationDate] [datetimeoffset](7) NULL,
	[RevokedByIp] [varchar](45) NULL,
	[ReplacedByToken] [nvarchar](500) NULL,
	CONSTRAINT [PK_RefreshToken] PRIMARY KEY CLUSTERED ([Id] ASC),
	CONSTRAINT [FK_RefreshToken_IdUser] FOREIGN KEY([IdUser]) REFERENCES [System].[User] ([Id])
)