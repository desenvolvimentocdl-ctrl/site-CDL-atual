using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SiteCDL.Application.Interfaces;
using SiteCDL.Domain.Entities;

namespace SiteCDL.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        private readonly IHomeService _homeService;

        public HomeController(IHomeService homeService)
        {
            _homeService= homeService;
        }

        [HttpGet]
        public async Task<IActionResult> GetHome()
        {
            var model = await _homeService.GetHomeDataAsync();

            return Ok(model);
        } 
    }
}
