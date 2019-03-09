CREATE TABLE [dbo].[movie]
(
	[movie_id] INT NOT NULL PRIMARY KEY IDENTITY(1,1),
	[name] NVARCHAR(50) NOT NULL,
	[release_year] SMALLINT NOT NULL,
	[director_id] INT NOT NULL,
	[genre_id] INT NOT NULL,
	[sub_genre_id] INT NOT NULL
)
GO

ALTER TABLE dbo.movie
ADD CONSTRAINT FK_movie_to_genre FOREIGN KEY ([genre_id]) REFERENCES [dbo].[genre]([genre_id])
GO

ALTER TABLE dbo.movie
ADD CONSTRAINT FK_movie_to_sub_genre FOREIGN KEY ([genre_id]) REFERENCES [dbo].[genre]([genre_id])
