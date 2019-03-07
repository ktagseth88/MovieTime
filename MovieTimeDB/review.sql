CREATE TABLE [dbo].[review]
(
	[review_id] INT NOT NULL PRIMARY KEY IDENTITY,
	[user_id] INT NOT NULL,
	[movie_id] INT NOT NULL,
	[review_text] NVARCHAR(MAX) NOT NULL,
	[rating] TINYINT NOT NULL DEFAULT 0,
	[create_timestamp] DATETIME NOT NULL,
	[modify_timestamp] DATETIME NULL
)
GO

ALTER TABLE [dbo].[review]
ADD CONSTRAINT FK_review_to_user FOREIGN KEY ([user_id]) REFERENCES [dbo].[user]([user_id])
GO

ALTER TABLE [dbo].[review]
ADD CONSTRAINT FK_review_to_movie FOREIGN KEY ([movie_id]) REFERENCES dbo.movie([movie_id])
GO
