CREATE TABLE [overwatch].[map]
(
	[map_id] INT NOT NULL PRIMARY KEY IDENTITY(1,1),
	[name] VARCHAR(255) NOT NULL,
	[map_type_id] INT NOT NULL
)
GO 


ALTER TABLE [overwatch].[map]
ADD CONSTRAINT FK_map_id_to_map_type_id FOREIGN KEY ([map_type_id]) REFERENCES [overwatch].[map_type]([map_type_id])
GO