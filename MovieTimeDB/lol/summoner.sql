CREATE TABLE [lol].[summoner]
(
	[summoner_id] INT NOT NULL PRIMARY KEY IDENTITY(1,1),
	[account_id] NVARCHAR(56) NOT NULL,
	[name] NVARCHAR(255) NOT NULL,
	[riot_summoner_id] NVARCHAR(63) NOT NULL,
	[puuid] NVARCHAR(78) NOT NULL
)
