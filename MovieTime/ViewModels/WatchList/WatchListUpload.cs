using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieTime.ViewModels.WatchList
{
    public class WatchListUpload
    {
        public string Title { get; set; }
        public DateTime ReleaseDate { get; set; }
        public string Director { get; set; }
        public string Description { get; set; }
        public byte? UserRating { get; set; }
        //Genres from imdb formatted as "genre 1, genre 2, genre 3"
        //We will only store first 2 genres
        public string Genres { get; set; }
    }

    public sealed class WatchListUploadMap : ClassMap<WatchListUpload>
    {
        public WatchListUploadMap()
        {
            Map(m => m.Title).Name("Title");
            Map(m => m.ReleaseDate).Name("Release Date");
            Map(m => m.Director).Name("Directors");
            Map(m => m.Description).Name("Description");
            Map(m => m.UserRating).Name("Your Rating");
            Map(m => m.Genres).Name("Genres");
        }
    }
}

//Position,Const,Created,Modified,Description,Title,URL,Title Type, IMDb Rating,Runtime(mins),Year,Genres,Num Votes, Release Date,Directors,Your Rating, Date Rated
