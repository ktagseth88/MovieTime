﻿CREATE TABLE [lol].[match_participant]
(
	[match_participant_id] INT NOT NULL PRIMARY KEY IDENTITY(1,1),
	[match_id] INT NOT NULL,
	[champion_id] INT NOT NULL,
	[role] NVARCHAR(255) NOT NULL,
	[lane] NVARCHAR(255) NOT NULL,
	[summoner_spell_1] INT NOT NULL,
	[summoner_spell_2] INT NOT NULL,
	[team_id] INT NOT NULL,
	[gold_earned] INT NOT NULL,
	[total_damage_taken] INT NOT NULL,
	[champ_level] INT NOT NULL,
	[creep_score] INT NOT NULL,
	[kills] INT NOT NULL,
	[deaths] INT NOT NULL,
	[assists] INT NOT NULL,
	[crowd_control_dealt_duration] INT NOT NULL,
	[total_damage_dealt] INT NOT NULL,
	[vision_wards] INT NOT NULL,
	[turret_kills] INT NOT NULL,
	[vision_score] BIGINT NOT NULL,
	[first_blood_kill] BIT NOT NULL,
	[first_blood_assist] BIT NOT NULL,
	[first_tower_kill] BIT NOT NULL,
	[first_tower_assist] BIT NOT NULL,
	[enemy_jungle_minions_killed] INT NOT NULL,
	[item_0_id] INT NOT NULL,
	[item_1_id] INT NOT NULL,
	[item_3_id] INT NOT NULL,
	[item_4_id] INT NOT NULL,
	[item_5_id] INT NOT NULL,
)
