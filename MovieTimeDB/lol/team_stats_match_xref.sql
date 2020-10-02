CREATE TABLE [lol].[team_stats_match_xref]
(
	[team_stats_match_xref_id] INT NOT NULL PRIMARY KEY IDENTITY(1,1),
	[team_stats_id] INT NOT NULL,
	[match_id] INT NOT NULL
)
