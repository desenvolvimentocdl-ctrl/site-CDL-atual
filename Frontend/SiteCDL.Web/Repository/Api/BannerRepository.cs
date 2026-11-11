using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using SiteCDL.Data;
using SiteCDL.Models;
using System.Web;

namespace SiteCDL.Repository.Api
{
    public class BannerRepository
    {

        private BannerDbContext _context;

        public BannerRepository(BannerDbContext context)
        {
            _context = context;
        }


        public async Task<Banner> FindBannerRepositoryAsync(Banner banner, string token)
        {

            if (token == "e6aae946-a3fc-4a25-b3c2-d46100d38498")
            {
                try
                {
                    return await _context.Banners.FindAsync(banner.Codigo);
                }
                catch
                {
                    return new Banner();
                }
            }
            else
            {
                return new Banner();
            }

        }


        public async Task<List<Banner>> GetAllBannersRepositoryAsync(string token)
        {
            if (token == "e6aae946-a3fc-4a25-b3c2-d46100d38498")
            {
                try
                {
                    return await _context.Banners.Where(a => a.Status == true).ToListAsync();
                }
                catch
                {
                    return new List<Banner>();
                }
            }
            else
            {
                return new List<Banner>();
            }

        }


        public async Task<Banner> GetByIdBannerRepositoryAsync(int id, string token)
        {

            if (token == "e6aae946-a3fc-4a25-b3c2-d46100d38498")
            {
                try
                {
                    return await _context.Banners.Where(a => a.Status == true && a.Codigo == id).SingleOrDefaultAsync();
                }
                catch
                {
                    return new Banner();
                }
            }
            else
            {
                return new Banner();
            }

        }


        public async Task<int> CreateBannerRepositoryAsync(Banner banner, string token)
        {
            if (token == "e6aae946-a3fc-4a25-b3c2-d46100d38498")
            {
                try
                {
                    _context.Banners.Add(banner);
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


        public async Task<int> UpdateBannerRepositoryAsync(Banner banner, string token)
        {

            if (token == "e6aae946-a3fc-4a25-b3c2-d46100d38498")
            {
                try
                {
                    _context.Banners.Update(banner);
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


    }
}
