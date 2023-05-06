CREATE TABLE [lol].[match]
(
	[match_id] INT NOT NULL PRIMARY KEY IDENTITY(1,1),
	[riot_match_id] BIGINT NOT NULL,
	[game_duration_in_seconds] BIGINT NOT NULL,
	[season] INT NULL,
	[queue_type] INT NULL,
	[game_version] NVARCHAR(MAX) NULL
)
