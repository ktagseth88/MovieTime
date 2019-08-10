using Microsoft.EntityFrameworkCore;
using MovieTime.DAL.Interfaces;
using MovieTime.DAL.EFCore.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MovieTime.Common.Models;

namespace MovieTime.DAL.EFCore
{
    class GenreRepositoryEFCore : IGenreRepository
    {
        private readonly MovieTimeContext _movieTimeDb;
       
        public GenreRepositoryEFCore(MovieTimeContext movieTimeDbContext)
        {
            _movieTimeDb = movieTimeDbContext;
        }

        public async Task InsertGenre()
        {
            throw new NotImplementedException();
        }

        public async Task<GenreDTO> GetGenreAsync(string name)
        {
            var genreEntity = await _movieTimeDb.Genre.FirstOrDefaultAsync(x => x.Name == name);
            return genreEntity.ToDTO();
        }
    }
}
