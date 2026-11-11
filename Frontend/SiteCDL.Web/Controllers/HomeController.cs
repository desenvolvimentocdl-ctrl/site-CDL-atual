using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SiteCDL.Data;
using SiteCDL.Models;
using SiteCDL.Services;
using System.Diagnostics;
using System.Text.Json;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;
using static System.Net.WebRequestMethods;

namespace SiteCDL.Controllers
{


    public class HomeController : Controller
    {
        private readonly HttpClient _httpClient;

        public HomeController(IHttpClientFactory httpClientFactory)
        {

            _httpClient = httpClientFactory.CreateClient("ApiHttpClient");
        }


        public async Task<IActionResult> Index()
        {            
            var response = await _httpClient.GetAsync("home"); // 'home' corresponde ao endpoint, ex: https://localhost:5001/api/home
            response.EnsureSuccessStatusCode();

            var responseData = await response.Content.ReadAsStringAsync();

            var homeData = System.Text.Json.JsonSerializer.Deserialize<Home>(responseData);

            try
            {
                var urlCursos = "https://loja.cdludi.org.br/api/cursos?token=UbnOYqfkmVxKtINnTloeDqVIbPdvw";

                using (var httpClient = new HttpClient())
                {
                    var responseCursos = await httpClient.GetStringAsync(urlCursos);
                    var cursos = JsonConvert.DeserializeObject<List<dynamic>>(responseCursos);
                    ViewBag.Cursos = cursos;

                }
            }
            catch (Exception e)
            {
                ViewBag.Cursos = new List<dynamic>();
            }

            try
            {
                var urlEventos = "https://loja.cdludi.org.br/api/eventos?token=UbnOYqfkmVxKtINnTloeDqVIbPdvw";

                using (var httpClient = new HttpClient())
                {
                    var responseEventos = await httpClient.GetStringAsync(urlEventos);
                    var eventos = JsonConvert.DeserializeObject<List<dynamic>>(responseEventos);
                   
                    ViewBag.Eventos = eventos;


                }
            }
            catch (Exception e)
            {
                ViewBag.eventos = new List<dynamic>();
            }
   
            
            return View(homeData);
        }


        public IActionResult Cabecalho(string d)
        {
            return PartialView(d);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
