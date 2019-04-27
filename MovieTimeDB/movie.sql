CREATE TABLE [dbo].[movie]
(
	[movie_id] INT NOT NULL PRIMARY KEY IDENTITY(1,1),
	[name] NVARCHAR(255) NOT NULL,
	[release_date] DATETIME NULL,
	[director_id] INT NULL,
	[genre_id] INT NULL,
	[sub_genre_id] INT NULL
)
GO

ALTER TABLE dbo.movie
ADD CONSTRAINT FK_movie_to_genre FOREIGN KEY ([genre_id]) REFERENCES [dbo].[genre]([genre_id])
GO

ALTER TABLE dbo.movie
ADD CONSTRAINT FK_movie_to_sub_genre FOREIGN KEY ([genre_id]) REFERENCES [dbo].[genre]([genre_id])
GO

ALTER TABLE dbo.movie
ADD CONSTRAINT FK_movie_to_director FOREIGN KEY ([director_id]) REFERENCES [dbo].[director]([director_id])
