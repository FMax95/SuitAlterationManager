CREATE TABLE [System].[Group] 
(
    [Id] uniqueidentifier NOT NULL,
    [Name] nvarchar(50) NOT NULL,
    [Description] nvarchar(200) NOT NULL,
    [IsEnabled] bit  NOT NULL,
    [UpdateDate] datetimeoffset(7) NOT NULL,
    [DatabaseVersion] timestamp NOT NULL,
    CONSTRAINT [PK_Group] PRIMARY KEY CLUSTERED ([Id] ASC)
)