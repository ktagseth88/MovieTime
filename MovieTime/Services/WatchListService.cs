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
            genres = genres.Distinct().ToList();
            InsertGenres(genres);

            var directors = watchList.Movies.Select(x => x.Movie.Director).Distinct();
            InsertDirectors(directors);

            InsertMovies(watchList.Movies.Select(x => x.Movie));
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
    }
}
