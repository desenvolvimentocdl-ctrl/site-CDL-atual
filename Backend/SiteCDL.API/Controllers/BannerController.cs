using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace SiteCDL.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BannerController : ControllerBase
    {

        [HttpGet]
        public async Task<IActionResult> GetBanner()
        {

            return Ok();
        }
    }
}
