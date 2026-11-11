using Microsoft.AspNetCore.Mvc;
using SiteCDL.Models;
using System.Text.Json;

namespace SiteCDL.Web.Controllers
{
    public class PageController : Controller
    {
        private readonly HttpClient _httpClient;
        public PageController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("ApiHttpClient");

        }
         

        public async Task<IActionResult> Index(string slug)
        {
            var response = await _httpClient.GetAsync($"page/{slug}"); // 'home' corresponde ao endpoint, ex: https://localhost:5001/api/home
            response.EnsureSuccessStatusCode();

            // Lê e desserializa a resposta JSON
            var responseData = await response.Content.ReadAsStringAsync();

            var pageData = JsonSerializer.Deserialize<Paginas>(responseData);

            // Passa o modelo para a View
            ViewBag.metaDescription = "Garanta sua certificação digital com a CDL Uberlândia. Soluções seguras para empresas e profissionais com suporte especializado.";
            if (pageData.Slug != null)
                return View(pageData);
            else
                return View("Error");

        }

    }
}
