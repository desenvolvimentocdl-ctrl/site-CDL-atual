using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using System.Text.RegularExpressions;
using SiteCDL.Models;
using System.Text.Json;

namespace SiteCDL.Web.Controllers
{
    public class PageController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly ICompositeViewEngine _viewEngine;
        public PageController(IHttpClientFactory httpClientFactory, ICompositeViewEngine viewEngine)
        {
            _httpClient = httpClientFactory.CreateClient("ApiHttpClient");
            _viewEngine = viewEngine;
        }


        public async Task<IActionResult> Index(string slug)
        {
            // Visual novo: páginas institucionais com tela própria em Views/Paginas/{slug}.cshtml.
            // Se não existir tela para o slug, segue mostrando o conteúdo do banco (Paginas), como antes.
            var nome = (slug ?? "").ToLowerInvariant();
            if (Regex.IsMatch(nome, "^[a-z0-9-]+$"))
            {
                var caminho = $"~/Views/Paginas/{nome}.cshtml";
                if (_viewEngine.GetView(null, caminho, isMainPage: true).Success)
                    return View(caminho);
            }

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
