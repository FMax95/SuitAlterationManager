CREATE TABLE [System].[Action] 
(
    [Id] uniqueidentifier NOT NULL,
    [IdContext] uniqueidentifier NOT NULL,
    [Name] nvarchar(50) NOT NULL,
    [UpdateDate] datetimeoffset(7) NOT NULL,
    [DatabaseVersion] timestamp NOT NULL,
    CONSTRAINT [PK_Action] PRIMARY KEY CLUSTERED ([Id] ASC),
	CONSTRAINT [FK_Action_IdContext] FOREIGN KEY([IdContext]) REFERENCES [System].[Context] ([Id])
)