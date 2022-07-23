CREATE TABLE [System].[GroupPermission] 
(
    [IdGroup] uniqueidentifier NOT NULL,
    [IdAction] uniqueidentifier NOT NULL,
    CONSTRAINT [PK_GroupPermission] PRIMARY KEY CLUSTERED ([IdGroup], [IdAction] ASC),
	CONSTRAINT [FK_GroupPermission_IdGroup] FOREIGN KEY([IdGroup]) REFERENCES [System].[Group] ([Id]),
	CONSTRAINT [FK_GroupPermission_IdAction] FOREIGN KEY([IdAction]) REFERENCES [System].[Action] ([Id])

)