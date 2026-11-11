using Microsoft.EntityFrameworkCore;
using SiteCDL.Data;
using SiteCDL.Models;

namespace SiteCDL.Repository.Api
{
    public class CategoryRepository
    {

        private DataContext _context;

        public CategoryRepository(DataContext context)
        {
            _context = context;
        }


        public async Task<List<PostCategoria>> GetAllPostCategoriaRepositoryAsync(string token)
        {

            if (token == "e6aae946-a3fc-4a25-b3c2-d46100d38498")
            {
                try
                {
                    return await _context.PostCategoria.ToListAsync();
                }
                catch
                {
                    return new List<PostCategoria>();
                }
            }
            else
            {
                return new List<PostCategoria>();
            }

        }


    }
}
