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
        private readonly SiteCDL.Services.MaisLidasService _maisLidas;
        public PostController(IHttpClientFactory httpClientFactory, SiteCDL.Services.MaisLidasService maisLidas)
        {
            _httpClient = httpClientFactory.CreateClient("ApiHttpClient");
            _maisLidas = maisLidas;
        }

        /// <summary>
        /// As 10 publicações mais lidas (usadas no campo "Buscar nesta página").
        /// Enquanto a contagem não tiver 10 matérias, completa com as mais recentes.
        /// </summary>
        [HttpGet("publicacoes/mais-lidas")]
        [ResponseCache(Duration = 120)]
        public async Task<IActionResult> MaisLidas()
        {
            const int total = 10;
            var lista = _maisLidas.Top(total);
            if (lista.Count < total)
            {
                try
                {
                    var json = await _httpClient.GetStringAsync("post/page/1");
                    var recentes = JsonSerializer.Deserialize<List<Post>>(json) ?? new();
                    foreach (var p in recentes)
                    {
                        if (lista.Count >= total) break;
                        var rota = p.PostCategoria?.Route ?? "";
                        if (string.IsNullOrEmpty(p.Slug) || lista.Any(i => i.Slug == p.Slug && i.Rota == rota)) continue;
                        lista.Add(new SiteCDL.Services.MaisLidasService.Item { Rota = rota, Slug = p.Slug, Titulo = SiteCDL.Services.MaisLidasService.TextoLimpo(p.Titulo), Data = p.Data });
                    }
                }
                catch (Exception) { /* sem API: devolve só o que já foi contado */ }
            }
            return Json(lista.Select(i => new
            {
                titulo = i.Titulo,
                url = $"/publicacoes/{i.Rota}/{i.Slug}",
                data = i.Data?.ToString("d MMM yyyy").Replace(".", ""),
                visualizacoes = i.Visualizacoes
            }));
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
            _maisLidas.Registrar(pageData?.Noticia, HttpContext);

            return View("Detail", pageData);
        }


        [HttpGet("publicacoes")]
        public async Task<IActionResult> AllPosts( int pagina = 1)
        {
            var response = await _httpClient.GetAsync($"post/page/{pagina}", HttpCompletionOption.ResponseHeadersRead);
             response.EnsureSuccessStatusCode();

            var responseData = await response.Content.ReadAsStringAsync();

            var pageData = JsonSerializer.Deserialize<List<Post>>(responseData);
            ViewBag.categoria = "Conteúdo";

            return View("Index", pageData);
        }
    }
}
