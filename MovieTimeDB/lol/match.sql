CREATE TABLE [lol].[match]
(
	[MatchId] INT NOT NULL PRIMARY KEY IDENTITY(1,1),
	[RiotMatchId] BIGINT NOT NULL,
	[GameDurationInSeconds] BIGINT NOT NULL
)
