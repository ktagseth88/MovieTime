CREATE TABLE [dbo].[user]
(
	[user_id] INT NOT NULL PRIMARY KEY IDENTITY(1,1),
	[username] NVARCHAR(20) NOT NULL,
	[password_hash] NVARCHAR(MAX) NOT NULL,
	[create_timestamp] DATETIME NOT NULL DEFAULT GETDATE()
)
GO
