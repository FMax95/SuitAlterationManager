CREATE TABLE [Retail].[Alteration]
(
	[Id] [uniqueidentifier] NOT NULL,
	[CustomerEmail] [nvarchar](150) NOT NULL,
	[Type] [nvarchar](100) NOT NULL,
	[Direction] [nvarchar](50) NOT NULL,
	[Status] [nvarchar](50) NOT NULL,
	[MeasureCM] int NOT NULL,
	[CreateDate] [datetime] NOT NULL,
	[UpdateDate] [datetime] NULL,
	[DatabaseVersion] [timestamp] NOT NULL,
	CONSTRAINT [PK_Alteration] PRIMARY KEY CLUSTERED ([Id] ASC)
)