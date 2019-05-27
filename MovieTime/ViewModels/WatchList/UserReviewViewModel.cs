using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.DataAnnotations;

namespace MovieTime.ViewModels.WatchList
{
    public class UserReviewViewModel
    {
        [Display(Name = "Rating")]
        [Range(0, 10, ErrorMessage = "Maximum rating of 10")]
        public byte? Rating { get; set; }

        [Display(Name = "Title")]
        public string MovieName { get; set; }

        [Display(Name = "Review")]
        [DataType(DataType.MultilineText)]
        public string ReviewText { get; set; } 

        public int MovieId { get; set; }

        [Display(Name = "Rewatch?")]
        public bool WouldRewatch { get; set; }
    }
}
