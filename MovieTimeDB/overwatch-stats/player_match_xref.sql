CREATE TABLE [overwatch].[player_match_xref]
(
	[player_match_xref_id] INT NOT NULL PRIMARY KEY IDENTITY (1,1),
	[match_id] INT NOT NULL,
	[player_id] INT NOT NULL,
	[role] VARCHAR(255) NOT NULL
)
GO

ALTER TABLE [overwatch].[player_match_xref]
ADD CONSTRAINT FK_player_match_xref_to_match FOREIGN KEY ([match_id]) REFERENCES [overwatch].[match]([match_id])
GO

ALTER TABLE [overwatch].[player_match_xref]
ADD CONSTRAINT FK_player_match_xref_to_player FOREIGN KEY ([player_id]) REFERENCES [overwatch].[player]([player_id])
GO
