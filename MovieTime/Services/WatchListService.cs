﻿using Microsoft.EntityFrameworkCore;
using MovieTime.Entities;
using MovieTime.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieTime.Services
{
    public class MovieService
    {
        private readonly MovieTimeContext _movieTimeDb;
        public MovieService(MovieTimeContext movieTimeDb)
        {
            _movieTimeDb = movieTimeDb;
        }

        public async Task UpsertWatchList(WatchList watchList)
        {
            var genres = watchList.Movies.Select(x => x.Movie.Genre?.Trim())
                                         .Concat(watchList.Movies.Select(x => x.Movie.SubGenre?.Trim()))
                                         .Where(x => x != null)
                                         .Distinct()
                                         .ToList();

            await InsertGenres(genres);

            var directors = watchList.Movies.Select(x => x.Movie.Director.Trim()).Distinct();
            await InsertDirectors(directors);

            await InsertMovies(watchList.Movies.Select(x => x.Movie));

            var newUserMovies = from m in watchList.Movies
                                where _movieTimeDb.Movie.Any(x => x.Director.Name == m.Movie.Director && x.Name == m.Movie.Title)
                                select m;

            var user = await _movieTimeDb.User.FirstOrDefaultAsync(x => x.Username == watchList.UserName);

            var newUserReviews = newUserMovies.Select(x => new Review
            {
                User = user,
                UserId = user.UserId,
                MovieId =  _movieTimeDb.Movie.FirstOrDefault(y => y.Name == x.Movie.Title && y.Director.Name == x.Movie.Director).MovieId,
                Rating = x.Rating,
                ReviewText = "",
                CreateTimestamp = DateTime.Now,
                //Rewatch = x.Rating != null ? (x.Rating >= 8) : (bool?)null
            });

            var userWatchList = (await _movieTimeDb.User.Include(x => x.Review).FirstOrDefaultAsync(x => x.Username == watchList.UserName)).Review.ToList();
            newUserReviews = newUserReviews.Where(x => !userWatchList.Select(y => y.MovieId).Contains(x.MovieId));

            _movieTimeDb.AddRange(newUserReviews);
            await _movieTimeDb.SaveChangesAsync();
        }

        public async Task<IEnumerable<UserMovieDetails>> GetUserMovieList(string username)
        {
            var userMovieList = _movieTimeDb.Review.Where(x => x.User.Username == username);

            var userMovieListDetails = await userMovieList.Select(x => new UserMovieDetails
            {
                MovieTitle = x.Movie.Name,
                Rating = x.Rating,
                Director = x.Movie.Director.Name,
                Genre = x.Movie.Genre.Name,
                ReviewId = x.ReviewId
            }).ToListAsync();

            return userMovieListDetails;
        }

        public async Task InsertMovies(IEnumerable<Models.Movie> movies)
        {
            var newMovies = movies.Where(x => !_movieTimeDb.Movie.Any(y => y.Name.Trim() == x.Title.Trim() && y.Director.Name.Trim() == x.Director.Trim()));

            _movieTimeDb.Movie.AddRange(newMovies.Select(x => new Entities.Movie
            {
                Name = x.Title.Trim(),
                ReleaseDate = x.ReleaseDate,
                GenreId = _movieTimeDb.Genre.FirstOrDefault(y => y.Name == x.Genre)?.GenreId,
                SubGenreId =  _movieTimeDb.Genre.FirstOrDefault(y => y.Name == x.SubGenre)?.GenreId,
                DirectorId =  _movieTimeDb.Director.FirstOrDefault(y => y.Name == x.Director)?.DirectorId
            }));

            await _movieTimeDb.SaveChangesAsync();
        }

        public async Task InsertGenres(IEnumerable<string> genres)
        {
            genres = genres.Select(x => x.Trim());
            var newGenres = genres.Except(_movieTimeDb.Genre.Select(x => x.Name));

            _movieTimeDb.Genre.AddRange(newGenres.Select(x => new Genre { Name = x }));
            await _movieTimeDb.SaveChangesAsync();
        }

        public async Task InsertDirectors(IEnumerable<string> directors)
        {
            var newDirectors = directors.Except(await _movieTimeDb.Director.Select(x => x.Name).ToListAsync());

            _movieTimeDb.Director.AddRange(newDirectors.Select(x => new Director { Name = x }));
            await _movieTimeDb.SaveChangesAsync();
        }

        public async Task UpdateReview(UserReview userReview)
        {
            if(_movieTimeDb.Review.Any(x => x.User.Username == userReview.Username && x.MovieId == userReview.MovieId))
            {
                var dbReview = await _movieTimeDb.Review.FirstOrDefaultAsync(x => x.User.Username == userReview.Username && x.MovieId == userReview.MovieId);
                dbReview.Rating = userReview.Rating;
                dbReview.ReviewText = userReview.Description;
                dbReview.Rewatch = userReview.WouldRewatch;
            }
            else
            {
                var dbReview = new Review
                {
                    Rating = userReview.Rating,
                    MovieId = userReview.MovieId,
                    UserId = (await _movieTimeDb.User.FirstOrDefaultAsync(x => x.Username == userReview.Username)).UserId,
                    ReviewText = userReview.Description,
                    Rewatch = userReview.WouldRewatch
                };
            }

            await _movieTimeDb.SaveChangesAsync();
        }

        public async Task<UserReview> GetReview(int reviewId)
        {
            var dbReview = await _movieTimeDb.Review.Include(x => x.Movie).Include(x => x.User).FirstOrDefaultAsync(x => x.ReviewId == reviewId);

            return new UserReview
            {
                Rating = dbReview.Rating,
                Description = dbReview.ReviewText,
                Username = dbReview.User.Username,
                MovieId = dbReview.MovieId,
                MovieTitle = dbReview.Movie.Name,
                WouldRewatch = dbReview.Rewatch
            };
        }
    }
}
