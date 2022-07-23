CREATE TABLE [System].[UserGroup]
(
	[IdUser] [uniqueidentifier] NOT NULL,
	[IdGroup] [uniqueidentifier] NOT NULL,
	CONSTRAINT [PK_UserGroup] PRIMARY KEY CLUSTERED ([IdUser] ASC, [IdGroup] ASC),
	CONSTRAINT [FK_UserGroup_IdUser] FOREIGN KEY([IdUser]) REFERENCES [System].[User] ([Id]),
	CONSTRAINT [FK_UserGroup_IdGroup] FOREIGN KEY([IdGroup]) REFERENCES [System].[Group] ([Id])
)