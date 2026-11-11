using Microsoft.AspNetCore.Mvc;
using SiteCDL.Services;

namespace SiteCDL.Controllers
{
	public class SearchController : Controller
	{

		private readonly SearchService _searchService;
		public SearchController(SearchService searchService)
		{
			_searchService = searchService;
		}

		[HttpGet("buscar")]
		public async ValueTask<IActionResult> Index(string id)
		{

			if(id!=null && id.Length >2)
				return View(await _searchService.GetSearchPostServiceAsync(id));
			else
			    return View();

        }
    }
}
