CREATE TABLE [lol].[champion]
(
	[champion_id] INT NOT NULL PRIMARY KEY IDENTITY(1,1),
	[riot_champion_id] INT NOT NULL,
	[name] NVARCHAR(255) NOT NULL
)
