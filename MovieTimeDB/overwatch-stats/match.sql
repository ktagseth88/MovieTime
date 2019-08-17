CREATE TABLE [overwatch].[match]
(
	[match_id] INT NOT NULL PRIMARY KEY IDENTITY (1,1),
	[map_id] INT NOT NULL,
	[timestamp] DATETIME DEFAULT GETDATE(),
	[victory] BIT NOT NULL
)
GO


ALTER TABLE [overwatch].[match]
ADD CONSTRAINT FK_match_id_to_map_id FOREIGN KEY ([map_id]) REFERENCES [overwatch].[map]([map_id])
GO