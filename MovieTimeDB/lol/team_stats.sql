CREATE TABLE [lol].[team_stats]
(
	[team_stats_id] INT NOT NULL PRIMARY KEY IDENTITY(1,1),
	[tower_kills] INT NOT NULL,
	[first_blood] BIT NOT NULL,
	[inhibitor_kills] INT NOT NULL,
	[first_dragon] BIT NOT NULL,
	[dragon_kills] INT NOT NULL,
	[first_inhibitor] BIT NOT NULL,
	[first_tower] BIT NOT NULL,
	[first_rift_herald] BIT NOT NULL,
	[team_id] INT NOT NULL, --100 = blue side, 200 = red side
	[win] BIT NOT NULL 
)
