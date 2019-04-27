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
        private MovieTimeContext _movieTimeDb;
        public MovieService(MovieTimeContext movieTimeDb)
        {
            _movieTimeDb = movieTimeDb;
        }

        public void UpsertWatchList(WatchList watchList)
        {
            var genres = watchList.Movies.Select(x => x.Movie.Genre).Concat(watchList.Movies.Select(x => x.Movie.SubGenre)).Where(x => x != null).Distinct().ToList();
            InsertGenres(genres);

            var directors = watchList.Movies.Select(x => x.Movie.Director).Distinct();
            InsertDirectors(directors);

            InsertMovies(watchList.Movies.Select(x => x.Movie));

            var userWatchList = _movieTimeDb.User.FirstOrDefault(x => x.Username == watchList.UserName).Review;

            var newUserMovies = from m in watchList.Movies
                       where _movieTimeDb.Movie.Any(x => x.Director.Name == m.Movie.Director && x.Name == m.Movie.Title)
                       select m;

            var user = _movieTimeDb.User.FirstOrDefault(x => x.Username == watchList.UserName);

            var newUserReviews = newUserMovies.Select(x => new Review
            {
                User = user,
                UserId = user.UserId,
                MovieId = _movieTimeDb.Movie.FirstOrDefault(y => y.Name == x.Movie.Title && y.Director.Name == x.Movie.Director).MovieId,
                Rating = x.Rating,
                ReviewText = "",
                CreateTimestamp = DateTime.Now
            });

            _movieTimeDb.AddRange(newUserReviews);
            _movieTimeDb.SaveChanges();
        }

        public IEnumerable<UserMovieDetails> GetUserMovieList(string username)
        {
            var userMovieList = _movieTimeDb.Review.Where(x => x.User.Username == username);

            var userMovieListDetails = userMovieList.Select(x => new UserMovieDetails
            {
                MovieTitle = x.Movie.Name,
                Rating = x.Rating,
                Director = x.Movie.Director.Name,
                Genre = x.Movie.Genre.Name
            });

            return userMovieListDetails;
        }

        public void InsertMovies(IEnumerable<Models.Movie> movies)
        {
            var newMovies = movies.Where(x => !_movieTimeDb.Movie.Any(y => y.Name == x.Title && y.Director.Name == x.Director));

            _movieTimeDb.Movie.AddRange(newMovies.Select(x => new Entities.Movie
            {
                Name = x.Title,
                ReleaseDate = x.ReleaseDate,
                GenreId = _movieTimeDb.Genre.FirstOrDefault(y => y.Name == x.Genre)?.GenreId,
                SubGenreId = _movieTimeDb.Genre.FirstOrDefault(y => y.Name == x.SubGenre)?.GenreId,
                DirectorId = _movieTimeDb.Director.FirstOrDefault(y => y.Name == x.Director)?.DirectorId
            }));

            _movieTimeDb.SaveChanges();
        }

        public void InsertGenres(IEnumerable<string> genres)
        {
            var newGenres = genres.Except(_movieTimeDb.Genre.Select(x => x.Name));

            _movieTimeDb.Genre.AddRange(newGenres.Select(x => new Genre { Name = x }));
            _movieTimeDb.SaveChanges();
        }

        public void InsertDirectors(IEnumerable<string> directors)
        {
            var newDirectors = directors.Except(_movieTimeDb.Director.Select(x => x.Name));

            _movieTimeDb.Director.AddRange(newDirectors.Select(x => new Director { Name = x }));
            _movieTimeDb.SaveChanges();
        }

        public void UpdateReview(UserReviewModel userReview)
        {
            if(_movieTimeDb.Review.Any(x => x.User.Username == userReview.Username && x.MovieId == userReview.movie_id))
            {
                var dbReview = _movieTimeDb.Review.FirstOrDefault(x => x.User.Username == userReview.Username && x.MovieId == userReview.movie_id);
                dbReview.Rating = userReview.Rating;
                dbReview.ReviewText = userReview.Description;
            }
            else
            {
                var dbReview = new Review
                {
                    Rating = userReview.Rating,
                    MovieId = userReview.movie_id,
                    UserId = _movieTimeDb.User.FirstOrDefault(x => x.Username == userReview.Username).UserId,
                    ReviewText = userReview.Description
                };
            }

            _movieTimeDb.SaveChanges();
        }
    }
}
