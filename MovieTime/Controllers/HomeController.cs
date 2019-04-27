using Microsoft.AspNetCore.Mvc;
using MovieTime.Models;
using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.IO;
using CsvHelper;
using MovieTime.ViewModels.WatchList;
using System.Collections.Generic;
using System.Linq;
using CsvHelper.Configuration;
using System;
using MovieTime.Services;

namespace MovieTime.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private MovieService _watchListService;
        public HomeController(MovieService watchListService)
        {
            _watchListService = watchListService;
        }

        public IActionResult Index()
        {
            var userMovieDetails = _watchListService.GetUserMovieList(HttpContext.User.Identity.Name).Select(x => new ViewModels.UserMovieDetailViewModel
            {
                Rating = x.Rating,
                Director = x.Director,
                MovieTitle = x.MovieTitle,
                Genre = x.Genre,
                ReviewId = x.ReviewId
            });

            return View(userMovieDetails);
        }

        [HttpPost]
        public async Task<IActionResult> UploadWatchList(IFormFile watchlistEntry)
        {
            var contents = new List<WatchListUpload>();

            using(var reader = new StreamReader(watchlistEntry.OpenReadStream(), System.Text.Encoding.UTF8))
            {
                //fileContents = await reader.ReadToEndAsync();
                var csvReader = new CsvReader(reader);
                csvReader.Configuration.RegisterClassMap<WatchListUploadMap>();
                contents = csvReader.GetRecords<WatchListUpload>().ToList() ;
            }

            var watchlist = new WatchList
            {
                Movies = contents.Select(x => (Movie: new Movie
                {
                    Title = x.Title,
                    Director = x.Director,
                    ReleaseDate = x.ReleaseDate,
                    Genre = x.Genres.Split(',').FirstOrDefault()?.Trim(),
                    SubGenre = x.Genres.Split(',').Skip(1)?.FirstOrDefault()?.Trim()
                }, rating: x.UserRating)),
                UserName = HttpContext.User.Identity.Name
            };

            _watchListService.UpsertWatchList(watchlist);

            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult EditUserReview(int reviewId)
        {
            var reviewModel = _watchListService.GetReview(reviewId);

            var reviewViewModel = new UserReviewViewModel
            {
                MovieId = reviewModel.movie_id,
                MovieName = reviewModel.MovieTitle,
                Rating = reviewModel.Rating,
                ReviewText = reviewModel.Description
            };
            return View(reviewViewModel);
        }

        [HttpPost]
        public IActionResult EditUserReview(UserReviewViewModel reviewViewModel)
        {
            var reviewModel = new UserReviewModel
            {
                movie_id = reviewViewModel.MovieId,
                Description = reviewViewModel.ReviewText,
                Rating = reviewViewModel.Rating,
                Username = HttpContext.User.Identity.Name,
                MovieTitle = reviewViewModel.MovieName
            };

            _watchListService.UpdateReview(reviewModel);

            return RedirectToAction("Index");
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
