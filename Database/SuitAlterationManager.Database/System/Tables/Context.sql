CREATE TABLE [System].[Context] 
(
    [Id] uniqueidentifier NOT NULL,
    [Name] nvarchar(50) NOT NULL,
    [UpdateDate] datetimeoffset(7) NOT NULL,
    [DatabaseVersion] timestamp NOT NULL,
    CONSTRAINT [PK_Context] PRIMARY KEY CLUSTERED ([Id] ASC)
)