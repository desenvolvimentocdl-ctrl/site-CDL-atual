using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SiteCDL.Application.Interfaces;

namespace SiteCDL.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PageController : ControllerBase
    {
        private readonly IPageService _pageService;

        public PageController(IPageService pageService)
        {
            _pageService = pageService;
        }

        [HttpGet("{slug}")]
        public async Task<IActionResult> GetPage(string slug)
        {

            var modelo = await _pageService.GetPageAsync(slug);

            return Ok(modelo);
        }
    }
}
