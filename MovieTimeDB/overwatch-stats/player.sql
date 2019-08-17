CREATE TABLE [overwatch].[player]
(
	[player_id] INT PRIMARY KEY IDENTITY (1,1),
	[name] VARCHAR(255) NOT NULL,
	[tank_sr] INT NULL,
	[heal_sr] INT NULL,
	[dps_sr] INT NULL
)
GO
