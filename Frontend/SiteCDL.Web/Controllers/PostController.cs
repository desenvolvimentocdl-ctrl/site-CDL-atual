using Microsoft.AspNetCore.Mvc;
using SiteCDL.Models;
using SiteCDL.Utilities;
using System.Net.Http;
using System.Text.Json;

namespace SiteCDL.Web.Controllers
{
    public class PostController : Controller
    {

        private readonly HttpClient _httpClient;
        public PostController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("ApiHttpClient");

        }
     

        [HttpGet("publicacoes/{route}")]
        public async Task<IActionResult> Index(string route,int pagina=1)
        {
            var response = await _httpClient.GetAsync($"post/{route}/page/{pagina}");
            // categoria sem publicações: a API responde 404 — mostra a lista vazia em vez de erro 500
            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                return View("Index", new List<Post>());
            response.EnsureSuccessStatusCode();

            var responseData = await response.Content.ReadAsStringAsync();

            var pageData = JsonSerializer.Deserialize<List<Post>>(responseData);
            return View("Index", pageData);
        }

        [HttpGet("publicacoes/{route}/{slug}")]
        public async Task<IActionResult> Detail(string route, string slug)
        {
            var response = await _httpClient.GetAsync($"post/{route}/{slug}", HttpCompletionOption.ResponseHeadersRead);
            var responseData = await response.Content.ReadAsStringAsync();

            var pageData = JsonSerializer.Deserialize<DetalhePostGeral>(responseData);

            return View("Detail", pageData);
        }


        [HttpGet("publicacoes")]
        public async Task<IActionResult> AllPosts( int pagina = 1)
        {
            var response = await _httpClient.GetAsync($"post/page/{pagina}", HttpCompletionOption.ResponseHeadersRead);
             response.EnsureSuccessStatusCode();

            var responseData = await response.Content.ReadAsStringAsync();

            var pageData = JsonSerializer.Deserialize<List<Post>>(responseData);
            ViewBag.categoria = "Publicações";

            return View("Index", pageData);
        }
    }
}
