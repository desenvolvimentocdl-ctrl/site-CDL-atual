using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using SiteCDL.Models;
using SiteCDL.Services.Api;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SiteCDL.Controllers.Api
{
    [Route("portal/[controller]")] 
    [ApiController]
    public class BannersController : ControllerBase
    {

        private readonly BannerService _bannerService;

        public BannersController(BannerService bannerService)
        {
            _bannerService = bannerService;
        }

        // GET: api/<BannersController>
        [EnableCors("AllowSpecificOrigins")]
        [HttpGet("{token}")]
        public async Task<List<Banner>> Get(string token)
        {

            if (token == "e6aae946-a3fc-4a25-b3c2-d46100d38498")
            {
                try
                {
                    return await _bannerService.GetAllBannersServiceAsync(token);
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

        // GET api/<BannersController>/5
        [EnableCors("AllowSpecificOrigins")]
        [HttpGet("{id}/{token}")]
        public async Task<Banner> Get(int id, string token)
        {

            if (token == "e6aae946-a3fc-4a25-b3c2-d46100d38498")
            {
                try
                {
                    return await _bannerService.GetByIdBannerServiceAsync(id, token);
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

        // POST api/<BannersController>
        [EnableCors("AllowSpecificOrigins")]
        [HttpPost]
        public async Task<HttpResponseMessage> Post([FromForm] Banner banner)
        {
            try
            {
                if (banner.Token == "e6aae946-a3fc-4a25-b3c2-d46100d38498")
                {
                    // Diretório de upload
                    var uploadPath = Path.Combine(Directory.GetCurrentDirectory(), $"wwwroot/wp-content/uploads/banners");
                    if (!Directory.Exists(uploadPath))
                    {
                        Directory.CreateDirectory(uploadPath);
                    }

                    // Salvar arquivos e gerar caminhos
                    var files = Request.Form.Files;
                    string desktopFilePath = string.Empty;
                    string desktopFileName = string.Empty;
                    string mobileFilePath = string.Empty;
                    string mobileFileName = string.Empty;

                    foreach (var file in files)
                    {
                        if (file.Name == "UploadBannerDesktop")
                        {
                            desktopFileName = $"desktop_{Guid.NewGuid()}_{file.FileName}";
                            desktopFilePath = Path.Combine(uploadPath, desktopFileName);


                            string fileNameWebPDesk = $"desktop_{Guid.NewGuid()}_{file.FileName}.webp";
                            string uploadPathWebPDesk = Path.Combine(uploadPath, fileNameWebPDesk);
                             
                            using (var stream = new FileStream(desktopFilePath, FileMode.Create))
                            {
                                await file.CopyToAsync(stream);
                            }

                            Utilities.ConvertImage.ConvertToWebP(desktopFilePath, uploadPathWebPDesk);

                            banner.CaminhoDesktop = fileNameWebPDesk;
                        }
                        else if (file.Name == "UploadBannerMobile")
                        {
                            mobileFileName = $"mobile_{Guid.NewGuid()}_{file.FileName}";
                            mobileFilePath = Path.Combine(uploadPath, mobileFileName);

                            string fileNameWebPMobile = $"mobile_{Guid.NewGuid()}_{file.FileName}.webp";
                            string uploadPathWebPMobile = Path.Combine(uploadPath, fileNameWebPMobile);


                            using (var stream = new FileStream(mobileFilePath, FileMode.Create))
                            {
                                await file.CopyToAsync(stream);
                            }                    
                     
                            Utilities.ConvertImage.ConvertToWebP(mobileFilePath, uploadPathWebPMobile);
                             
                            banner.CaminhoMobile = fileNameWebPMobile;
                        }
                    }

                    if (await _bannerService.CreateBannerServiceAsync(banner, banner.Token) > 0)
                        return Utilities.Response.Api("OK");
                    else
                        return Utilities.Response.Api("BadRequest");
                }
                else
                {
                    return Utilities.Response.Api("BadRequest");
                }
            }
            catch
            {
                return Utilities.Response.Api("BadRequest");
            }
        }

        // PUT api/<BannersController>/5
        [EnableCors("AllowSpecificOrigins")]
        [HttpPut]
        public async Task<HttpResponseMessage> Put([FromForm] Banner banner)
        {
            try
            {
                if (banner.Token == "e6aae946-a3fc-4a25-b3c2-d46100d38498")
                {
                    // Diretório de upload
                    var uploadPath = Path.Combine(Directory.GetCurrentDirectory(), $"wwwroot/wp-content/uploads/banners");
                    if (!Directory.Exists(uploadPath))
                    {
                        Directory.CreateDirectory(uploadPath);
                    }

                    // Salvar arquivos e gerar caminhos
                    var files = Request.Form.Files;
                    string desktopFilePath = string.Empty;
                    string desktopFileName = string.Empty;
                    string mobileFilePath = string.Empty;
                    string mobileFileName = string.Empty;

                    if (files.Count > 0)
                    {
                        foreach (var file in files)
                        {
                            if (file.Name == "UploadBannerDesktop")
                            {
                                desktopFileName = $"desktop_{Guid.NewGuid()}_{file.FileName}";
                                desktopFilePath = Path.Combine(uploadPath, desktopFileName);


                                string fileNameWebPDesk = $"desktop_{Guid.NewGuid()}_{file.FileName}.webp";
                                string uploadPathWebPDesk = Path.Combine(uploadPath, fileNameWebPDesk);

                                using (var stream = new FileStream(desktopFilePath, FileMode.Create))
                                {
                                    await file.CopyToAsync(stream);
                                }

                                Utilities.ConvertImage.ConvertToWebP(desktopFilePath, uploadPathWebPDesk);

                                banner.CaminhoDesktop = fileNameWebPDesk;
                            }
                            else if (file.Name == "UploadBannerMobile")
                            {
                                mobileFileName = $"mobile_{Guid.NewGuid()}_{file.FileName}";
                                mobileFilePath = Path.Combine(uploadPath, mobileFileName);

                                string fileNameWebPMobile = $"mobile_{Guid.NewGuid()}_{file.FileName}.webp";
                                string uploadPathWebPMobile = Path.Combine(uploadPath, fileNameWebPMobile);


                                using (var stream = new FileStream(mobileFilePath, FileMode.Create))
                                {
                                    await file.CopyToAsync(stream);
                                }

                                Utilities.ConvertImage.ConvertToWebP(mobileFilePath, uploadPathWebPMobile);

                                banner.CaminhoMobile = fileNameWebPMobile;
                            }
                        }
                    }

                    if (await _bannerService.UpdateBannerServiceAsync(banner, banner.Token) > 0)
                        return Utilities.Response.Api("OK");
                    else
                        return Utilities.Response.Api("BadRequest");
                }
                else
                {
                    return Utilities.Response.Api("BadRequest");
                }
            }
            catch
            {
                return Utilities.Response.Api("BadRequest");
            }
        }

        // DELETE api/<BannersController>/5
        [EnableCors("AllowSpecificOrigins")]
        [HttpDelete]
        public async Task<HttpResponseMessage> Delete([FromBody] Banner banner)
        {
            try
            {
                if (banner.Token == "e6aae946-a3fc-4a25-b3c2-d46100d38498")
                {
                    if (await _bannerService.DeletePostsServiceAsync(banner, banner.Token) > 0)
                        return Utilities.Response.Api("OK");
                    else
                        return Utilities.Response.Api("BadRequest");
                }
                else
                {
                    return Utilities.Response.Api("BadRequest");
                }
            }
            catch
            {
                return Utilities.Response.Api("BadRequest");
            }
        }
    }
}
