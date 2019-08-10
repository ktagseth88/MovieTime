using System;
using System.Collections.Generic;
using System.Text;
using MovieTime.Common.Models;

namespace MovieTime.DAL.EFCore.Entities
{
    public static class GenreExtensions
    {
        public static GenreDTO ToDTO(this Genre genre)
        {
            var dto = new GenreDTO
            {
                Id = genre.GenreId,
                Name = genre.Name
            };

            return dto;
        }
    }
}
