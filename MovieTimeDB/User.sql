CREATE TABLE [dbo].[user]
(
	[user_id] INT NOT NULL PRIMARY KEY IDENTITY,
	[username] NVARCHAR(20) NOT NULL,
	[password_hash] INT NOT NULL,
	[create_timestamp] DATETIME NOT NULL
)
