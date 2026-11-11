using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SiteCDL.Data;
using SiteCDL.Models;
using System.Reflection;
using System.Web;

namespace SiteCDL.Repository.Api
{
    public class PaginasRepository
    {

        private DataContext _context;

        public PaginasRepository(DataContext context)
        {
            _context = context;
        }


        public async Task<Paginas> FindRepositoryAsync(Paginas model, string token)
        {
            if (token == "e6aae946-a3fc-4a25-b3c2-d46100d38498")
            {
                try
                {
                    return await _context.Paginas.FindAsync(model.Codigo);
                }
                catch
                {
                    return new Paginas();
                }
            }
            else
            {
                return new Paginas();
            }

        }


        public async Task<List<Paginas>> GetAllRepositoryAsync(string token)
        {
            if (token == "e6aae946-a3fc-4a25-b3c2-d46100d38498")
            {
                try
                {
                    return await _context.Paginas.Where(a => a.Status == true).ToListAsync();
                }
                catch
                {
                    return new List<Paginas>();
                }
            }
            else
            {
                return new List<Paginas>();
            }

        }


        public async Task<Paginas> GetByIdRepositoryAsync(int id, string token)
        {

            if (token == "e6aae946-a3fc-4a25-b3c2-d46100d38498")
            {
                try
                {
                    return await _context.Paginas.Where(a => a.Status == true && a.Codigo == id).SingleOrDefaultAsync();
                }
                catch
                {
                    return new Paginas();
                }
            }
            else
            {
                return new Paginas();
            }

        }

        public async Task<int> GetQuantityBySlugRepositoryAsync(string slug, string token)
        {

            if (token == "e6aae946-a3fc-4a25-b3c2-d46100d38498")
            {
                try
                {
                    return await _context.Paginas.Where(a => EF.Functions.Like(a.Slug, $"{slug}%") && a.Status).CountAsync();
                }
                catch
                {
                    return 0;
                }
            }
            else
            {
                return 0;
            }

        }


        public async Task<int> CreateRepositoryAsync(Paginas model, string token)
        {
            if (token == "e6aae946-a3fc-4a25-b3c2-d46100d38498")
            {
                try
                {
                    _context.Paginas.Add(model);
                    return await _context.SaveChangesAsync();
                }
                catch
                {
                    return 0;
                }
            }
            else
            {
                return 0;
            }

        }


        public async Task<int> UpdateRepositoryAsync(Paginas model, string token)
        {

            if (token == "e6aae946-a3fc-4a25-b3c2-d46100d38498")
            {
                try
                {
                    _context.Paginas.Update(model);
                    return await _context.SaveChangesAsync();
                }
                catch
                {
                    return 0;
                }
            }
            else
            {
                return 0;
            }

        }


        public List<string> ListControllers(string token)
        {

            if (token == "e6aae946-a3fc-4a25-b3c2-d46100d38498")
            {
                try
                {
                    return Assembly.GetExecutingAssembly()
                                                 .GetTypes()
                                                 .Where(type => typeof(ControllerBase).IsAssignableFrom(type))
                                                 .Select(type => type.Name)
                                                 .ToList();
                }
                catch
                {
                    return new List<string>();
                }
            }
            else
            {
                return new List<string>();
            }


        }


    }
}
