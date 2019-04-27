CREATE TABLE [dbo].[user_watch_party_xref]
(
	[id] INT NOT NULL PRIMARY KEY IDENTITY(1,1),
	[user_id] INT NOT NULL,
	[watch_party_id] INT NOT NULL
)
GO


ALTER TABLE dbo.user_watch_party_xref
ADD CONSTRAINT FK_user_user_watch_party_xref FOREIGN KEY ([user_id]) REFERENCES [dbo].[user]([user_id])
GO

ALTER TABLE dbo.user_watch_party_xref
ADD CONSTRAINT FK_watch_party_user_watch_party_xref FOREIGN KEY ([watch_party_id]) REFERENCES [dbo].[watch_party]([watch_party_id])
GO
