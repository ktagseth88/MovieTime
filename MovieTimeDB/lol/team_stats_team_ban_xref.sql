CREATE TABLE [dbo].[team_stats_team_ban_xref]
(
	[team_stats_team_ban_xref_id] INT NOT NULL PRIMARY KEY IDENTITY(1,1),
	[team_ban_id] INT NOT NULL,
	[team_stats_id] INT NOT NULL
)
