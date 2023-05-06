CREATE TABLE [lol].[team_bans]
(
	[team_ban_id] INT NOT NULL PRIMARY KEY IDENTITY(1,1),
	[team_stats_id] INT NOT NULL,
	[champion_id] INT NOT NULL,
	[pick_turn] INT NOT NULL
)
