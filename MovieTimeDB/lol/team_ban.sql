CREATE TABLE [lol].[team_bans]
(
	[team_ban_id] INT NOT NULL PRIMARY KEY IDENTITY(1,1),
	[champion_id] INT NOT NULL,
	[pick_turn] INT NOT NULL
)
