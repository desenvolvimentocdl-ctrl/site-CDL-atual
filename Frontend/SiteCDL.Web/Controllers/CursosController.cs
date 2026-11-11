using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace SiteCDL.Controllers
{

    public class CursosController : Controller
    {
        private readonly HttpClient _httpClient;

        public CursosController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("ApiHttpClient");
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var url = "https://loja.cdludi.org.br/api/cursos?token=UbnOYqfkmVxKtINnTloeDqVIbPdvw";

                using (var httpClient = new HttpClient())
                {
                    var response = await httpClient.GetStringAsync(url);
                    var cursos = JsonConvert.DeserializeObject<List<dynamic>>(response);

                    ViewBag.Cursos = cursos;
                }

                return View();
            }
            catch(Exception e)
            {
                ViewBag.Cursos = new List<dynamic>();

                return View();
            }
        }
    }
}