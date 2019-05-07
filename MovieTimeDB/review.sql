CREATE TABLE [dbo].[review]
(
	[review_id] INT NOT NULL PRIMARY KEY IDENTITY(1,1),
	[user_id] INT NOT NULL,
	[movie_id] INT NOT NULL,
	[review_text] NVARCHAR(MAX) NULL,
	[rating] TINYINT NULL,
	[create_timestamp] DATETIME NOT NULL DEFAULT GETDATE(),
	[modify_timestamp] DATETIME NULL
)
GO

CREATE NONCLUSTERED INDEX ix_review_user_id on [dbo].[review] (user_id)
GO

CREATE NONCLUSTERED INDEX ix_review_movie_id on [dbo].[review] (movie_id)
GO

ALTER TABLE [dbo].[review]
ADD CONSTRAINT FK_review_to_user FOREIGN KEY ([user_id]) REFERENCES [dbo].[user]([user_id])
GO

ALTER TABLE [dbo].[review]
ADD CONSTRAINT FK_review_to_movie FOREIGN KEY ([movie_id]) REFERENCES dbo.movie([movie_id])
GO
