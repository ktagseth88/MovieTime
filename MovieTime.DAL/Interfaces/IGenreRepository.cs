using MovieTime.DAL.EFCore.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MovieTime.Common.Models;

namespace MovieTime.DAL.Interfaces
{
    interface IGenreRepository
    {
        Task InsertGenre();

        Task<GenreDTO> GetGenreAsync(string name);
    }
}
