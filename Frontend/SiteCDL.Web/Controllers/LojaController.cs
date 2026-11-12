using System.Globalization;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace SiteCDL.Web.Controllers
{
    /// <summary>
    /// Vitrine da Loja CDL no visual novo (/loja...). Eventos e cursos vêm da API da Loja;
    /// demais telas são as páginas do protótipo (Views/Loja/Paginas). Compra, inscrição e login
    /// continuam no sistema oficial (loja.cdludi.org.br).
    /// </summary>
    public class LojaController : Controller
    {
        public const string LojaUrl = "https://loja.cdludi.org.br";
        private const string Token = "UbnOYqfkmVxKtINnTloeDqVIbPdvw"; // mesmo token já usado em HomeController/CursosController
        private static readonly Dictionary<string, (DateTime quando, List<LojaItem> itens)> _cache = new();
        private readonly ICompositeViewEngine _viewEngine;

        public LojaController(ICompositeViewEngine viewEngine) { _viewEngine = viewEngine; }

        [HttpGet("loja")]
        public async Task<IActionResult> Index()
        {
            ViewBag.Agenda = (await Buscar("eventos")).Concat(await Buscar("cursos")).OrderBy(i => i.Inicio ?? DateTime.MaxValue).ToList();
            return Pagina("loja");
        }

        [HttpGet("loja/eventos")]
        public async Task<IActionResult> Eventos() { ViewBag.Itens = await Buscar("eventos"); return Pagina("loja-eventos"); }

        [HttpGet("loja/cursos")]
        public async Task<IActionResult> Cursos() { ViewBag.Itens = await Buscar("cursos"); return Pagina("loja-cursos"); }

        [HttpGet("loja/menor-preco")]
        public IActionResult MenorPreco() => Pagina("loja-menorpreco");

        [HttpGet("loja/menor-preco/manual")]
        public IActionResult MenorPrecoManual() => Pagina("loja-menorpreco-manual");

        [HttpGet("loja/{secao}")]
        public IActionResult Secao(string secao) => Pagina("loja-" + secao);

        [HttpGet("loja/{secao}/{id}")]
        public IActionResult Detalhe(string secao, string id)
        {
            var prefixo = secao switch { "cursos-ead" => "loja-ead-", "descontos" => "loja-desconto-", "parceiros" => "loja-parceiro-", _ => null };
            return prefixo == null ? View("Error") : Pagina(prefixo + id);
        }

        [HttpGet("loja/produto/{slug}")]
        public async Task<IActionResult> Produto(string slug)
        {
            // produtos com página completa no protótipo (com descrição)
            var codigo = Regex.Match(slug ?? "", @"-(\d+)$").Groups[1].Value;
            if (codigo != "" && Existe("loja-produto-" + codigo)) return Pagina("loja-produto-" + codigo);

            var item = (await Buscar("eventos")).Concat(await Buscar("cursos"))
                .FirstOrDefault(i => string.Equals(i.Slug, slug, StringComparison.OrdinalIgnoreCase));
            if (item == null) return Redirect($"{LojaUrl}/Compras/Produto/{Uri.EscapeDataString(slug ?? "")}");
            ViewData["Title"] = item.Titulo;
            return View("~/Views/Loja/Produto.cshtml", item);
        }

        // ------------------------------------------------------------------
        // Imagens da Loja/Portal repassadas pelo próprio site.
        // Na rede interna, loja.cdludi.org.br e portal.cdludi.org.br apontam para um IP privado (192.168.1.8);
        // quem abre o site pelo IP da rede (http://134.x...) tem essas imagens bloqueadas pelo Chrome
        // ("Private/Local Network Access"). Passando pelo site, o navegador só fala com o servidor do site.
        private static readonly HttpClient _imgHttp = new(new HttpClientHandler { AllowAutoRedirect = true }) { Timeout = TimeSpan.FromSeconds(20) };
        private static readonly System.Collections.Concurrent.ConcurrentDictionary<string, (byte[] dados, string tipo)> _imgCache = new();

        public static string ImagemServico(string codigo, string arquivo) =>
            string.IsNullOrWhiteSpace(arquivo) ? "" : $"/loja/imagem/servicos/{Uri.EscapeDataString(codigo)}/{Uri.EscapeDataString(arquivo)}";

        [HttpGet("loja/imagem/{origem}/{codigo}/{arquivo}")]
        [ResponseCache(Duration = 86400, Location = ResponseCacheLocation.Any)]
        public async Task<IActionResult> Imagem(string origem, string codigo, string arquivo)
        {
            var baseUrl = origem switch
            {
                "servicos" => $"{LojaUrl}/Anexos/servicos/",
                "ead" => "https://portal.cdludi.org.br/Anexos/ImagensCursosEAD/",
                _ => null
            };
            if (baseUrl == null || !Regex.IsMatch(codigo ?? "", @"^\d{1,9}$") || !Regex.IsMatch(arquivo ?? "", @"^[\w\-. ]{1,200}$") || arquivo!.Contains(".."))
                return NotFound();

            var chave = $"{origem}/{codigo}/{arquivo}";
            if (!_imgCache.TryGetValue(chave, out var img))
            {
                try
                {
                    using var resp = await _imgHttp.GetAsync(baseUrl + codigo + "/" + Uri.EscapeDataString(arquivo));
                    var tipo = resp.Content.Headers.ContentType?.MediaType ?? "";
                    if (!resp.IsSuccessStatusCode || !tipo.StartsWith("image/")) return NotFound();
                    img = (await resp.Content.ReadAsByteArrayAsync(), tipo);
                    if (_imgCache.Count > 500) _imgCache.Clear(); // limite simples de memória
                    _imgCache[chave] = img;
                }
                catch { return NotFound(); }
            }
            return File(img.dados, img.tipo);
        }

        private bool Existe(string nome) =>
            Regex.IsMatch(nome, "^[a-z0-9-]+$") && _viewEngine.GetView(null, $"~/Views/Loja/Paginas/{nome}.cshtml", true).Success;

        private IActionResult Pagina(string nome) =>
            Existe(nome) ? View($"~/Views/Loja/Paginas/{nome}.cshtml") : View("Error");

        /// <summary>Eventos ou cursos da API da Loja (cache de 5 minutos; lista vazia se a Loja não responder).</summary>
        public static async Task<List<LojaItem>> Buscar(string tipo)
        {
            lock (_cache)
                if (_cache.TryGetValue(tipo, out var c) && DateTime.Now - c.quando < TimeSpan.FromMinutes(5)) return c.itens;
            try
            {
                using var http = new HttpClient { Timeout = TimeSpan.FromSeconds(15) };
                var json = await http.GetStringAsync($"{LojaUrl}/api/{tipo}?token={Token}");
                var itens = (JsonConvert.DeserializeObject<List<JObject>>(json) ?? new()).Select(o => LojaItem.De(o, tipo)).ToList();
                lock (_cache) _cache[tipo] = (DateTime.Now, itens);
                return itens;
            }
            catch { return new List<LojaItem>(); }
        }
    }

    public class LojaItem
    {
        public string Codigo = "", Tipo = "", Titulo = "", Slug = "", Imagem = "", Instrutor = "", CargaHoraria = "", Valor = "", Landing = "";
        public string DataIni = "", DataFim = "", HoraIni = "", HoraFim = "";
        public bool Gratuito;

        public static LojaItem De(JObject o, string tipo)
        {
            string S(string k) => (o[k]?.Type == JTokenType.Null ? "" : o[k]?.ToString() ?? "").Trim();
            var suf = tipo == "cursos" ? "Curso" : "Evento";
            var i = new LojaItem
            {
                Codigo = S("codigo"), Tipo = suf, Titulo = S("titulo"), Slug = S("slug"), Instrutor = S("instrutor"),
                CargaHoraria = S("cargahoraria"), Valor = S("valorunitario"), Landing = S("urlLandingPage"),
                DataIni = S("dataInicio" + suf), DataFim = S("dataFim" + suf), HoraIni = S("horaInicio" + suf), HoraFim = S("horaFim" + suf),
                Gratuito = S("gratuito") == "S" || S("valorunitario") is "" or "0,00",
            };
            var img = S("imagem");
            i.Imagem = LojaController.ImagemServico(i.Codigo, img);
            return i;
        }

        public DateTime? Inicio => DateTime.TryParseExact(DataIni, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out var d) ? d : null;
        public string Periodo => DataFim == "" || DataFim == DataIni ? DataIni : $"{DataIni} à {DataFim}";
        public string Horario => HoraIni == "" ? "" : (HoraFim == "" || HoraFim == HoraIni ? HoraIni : $"{HoraIni} às {HoraFim}");
        public string UrlPagina => "/loja/produto/" + Slug;
        // Pago ou gratuito, o botão leva direto ao checkout da loja oficial. Antes o gratuito abria a
        // página do produto na loja antiga e a pessoa tinha que clicar em "inscrever" de novo.
        public string UrlCompra => $"{LojaController.LojaUrl}/checkout/link?s={Uri.EscapeDataString(Slug ?? "")}";
        public string TextoCompra => Gratuito ? "Inscreva-se" : "Comprar";
    }
}
