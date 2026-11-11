using Microsoft.EntityFrameworkCore;
using SiteCDL.Models;
using SiteCDL.Repository.Api;
using System.Reflection;
using System.Web;

namespace SiteCDL.Services.Api
{
    public class BannerService
    {
        private readonly BannerRepository _bannerRepository;

        public BannerService(BannerRepository bannerRepository)
        {
            _bannerRepository = bannerRepository;
        }

        public async Task<List<Banner>> GetAllBannersServiceAsync(string token)
        {
            if (token == "e6aae946-a3fc-4a25-b3c2-d46100d38498")
            {
                try
                {
                    return await _bannerRepository.GetAllBannersRepositoryAsync(token);
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


        public async Task<Banner> GetByIdBannerServiceAsync(int id, string token)
        {


            if (token == "e6aae946-a3fc-4a25-b3c2-d46100d38498")
            {
                try
                {
                    return await _bannerRepository.GetByIdBannerRepositoryAsync(id, token);

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

        public async Task<int> CreateBannerServiceAsync(Banner banner, string token)
        {
            if (token == "e6aae946-a3fc-4a25-b3c2-d46100d38498")
            {
                try
                {
                    Banner obj = new();

                    obj.Nome = banner.Nome;
                    obj.DataInicio = banner.DataInicio;
                    obj.DataFim = banner.DataFim;
                    obj.Link = banner.Link;
                    obj.CaminhoDesktop = banner.CaminhoDesktop;
                    obj.CaminhoMobile = banner.CaminhoMobile;
                    obj.IdUsuarioPortal = banner.IdUsuarioPortal;
                    obj.Status = true;

                    return await _bannerRepository.CreateBannerRepositoryAsync(obj, token);

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

        public async Task<int> UpdateBannerServiceAsync(Banner banner, string token)
        {
            if (token == "e6aae946-a3fc-4a25-b3c2-d46100d38498")
            {
                try
                {
                    var updateBanner = await _bannerRepository.FindBannerRepositoryAsync(banner, token);

                    if (updateBanner != null)
                    {
                        updateBanner.Nome = banner.Nome;
                        updateBanner.DataInicio = banner.DataInicio;
                        updateBanner.DataFim = banner.DataFim;
                        updateBanner.Link = banner.Link;
                        updateBanner.IdUsuarioPortalUpdate = banner.IdUsuarioPortalUpdate;

                        if (!string.IsNullOrWhiteSpace(banner.CaminhoDesktop))
                        {
                            updateBanner.CaminhoDesktop = banner.CaminhoDesktop;
                        }

                        if (!string.IsNullOrWhiteSpace(banner.CaminhoMobile))
                        {
                            updateBanner.CaminhoMobile = banner.CaminhoMobile;
                        }

                        return await _bannerRepository.UpdateBannerRepositoryAsync(updateBanner, token);
                    }
                    else
                    {
                        return 0;
                    }
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

        public async Task<int> DeletePostsServiceAsync(Banner banner, string token)
        {
            if (token == "e6aae946-a3fc-4a25-b3c2-d46100d38498")
            {
                try
                {
                    Banner deleteBanner = await _bannerRepository.FindBannerRepositoryAsync(banner, token);

                    if (deleteBanner != null)
                    {
                        deleteBanner.Status = false;
                        deleteBanner.IdUsuarioPortalDelete = banner.IdUsuarioPortalDelete;

                        return await _bannerRepository.UpdateBannerRepositoryAsync(deleteBanner, token);
                    }
                    else
                    {
                        return 0;
                    }
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
