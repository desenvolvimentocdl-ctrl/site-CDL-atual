using System.Net;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc;
using SiteCDL.Models;
using SiteCDL.Services;

namespace SiteCDL.Web.Controllers
{
    /// <summary>
    /// GEO (Generative Engine Optimization) + SEO técnico:
    ///   /robots.txt  — libera os robôs de busca e de IA (ChatGPT, Claude, Perplexity, Gemini, Copilot...) e indica o sitemap;
    ///   /sitemap.xml — páginas fixas (GeoCatalogo) + todas as publicações vindas da API;
    ///   /llms.txt    — resumo do site em Markdown, no padrão llmstxt.org, para assistentes de IA citarem a CDL corretamente.
    /// </summary>
    public class GeoController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _config;

        private static readonly SemaphoreSlim _trava = new(1, 1);
        private static (DateTime quando, List<Post> posts)? _cachePosts;

        public GeoController(IHttpClientFactory httpClientFactory, IConfiguration config)
        {
            _httpClient = httpClientFactory.CreateClient("ApiHttpClient");
            _config = config;
        }

        private string Site => (_config["SiteUrl"] ?? GeoCatalogo.SiteUrlPadrao).TrimEnd('/');

        // Robôs de IA liberados explicitamente (além do "*"), para deixar a intenção clara.
        private static readonly string[] RobosIA =
        {
            "GPTBot", "OAI-SearchBot", "ChatGPT-User",          // OpenAI
            "ClaudeBot", "Claude-SearchBot", "Claude-User",      // Anthropic
            "PerplexityBot", "Perplexity-User",                  // Perplexity
            "Google-Extended", "Googlebot", "Bingbot",           // Gemini / Google, Copilot / Bing
            "Applebot", "Applebot-Extended", "DuckAssistBot", "meta-externalagent", "CCBot"
        };

        private static readonly string[] Bloqueados = { "/painel-associado", "/api/", "/buscar", "/Home/Error", "/Analise" };

        [HttpGet("robots.txt")]
        [ResponseCache(Duration = 3600)]
        public IActionResult Robots()
        {
            var sb = new StringBuilder();
            sb.AppendLine("# CDL Uberlândia — robots.txt");
            sb.AppendLine("# Conteúdo público liberado para buscadores e assistentes de IA. Resumo para IA: " + Site + "/llms.txt");
            sb.AppendLine();
            foreach (var bot in RobosIA.Append("*"))
            {
                sb.AppendLine("User-agent: " + bot);
                sb.AppendLine("Allow: /");
                foreach (var b in Bloqueados) sb.AppendLine("Disallow: " + b);
                sb.AppendLine();
            }
            sb.AppendLine("Sitemap: " + Site + "/sitemap.xml");
            return Content(sb.ToString(), "text/plain; charset=utf-8");
        }

        [HttpGet("sitemap.xml")]
        [ResponseCache(Duration = 3600)]
        public async Task<IActionResult> Sitemap()
        {
            var sb = new StringBuilder();
            sb.AppendLine("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
            sb.AppendLine("<urlset xmlns=\"http://www.sitemaps.org/schemas/sitemap/0.9\">");
            void Url(string caminho, DateTime? alterado = null, string freq = "monthly", string prio = "0.6")
            {
                sb.Append("  <url><loc>").Append(WebUtility.HtmlEncode(Site + caminho)).Append("</loc>");
                if (alterado != null) sb.Append("<lastmod>").Append(alterado.Value.ToString("yyyy-MM-dd")).Append("</lastmod>");
                sb.Append("<changefreq>").Append(freq).Append("</changefreq><priority>").Append(prio).AppendLine("</priority></url>");
            }

            Url("/", freq: "daily", prio: "1.0");
            var vistos = new HashSet<string>(StringComparer.OrdinalIgnoreCase) { "/" };
            foreach (var g in GeoCatalogo.Grupos)
                foreach (var p in g.Paginas)
                    if (vistos.Add(p.Caminho))
                        Url(p.Caminho, freq: p.Caminho.StartsWith("/publicacoes") || p.Caminho.StartsWith("/loja") ? "daily" : "monthly",
                            prio: p.Caminho.Count(c => c == '/') == 1 ? "0.8" : "0.7");

            foreach (var p in await Publicacoes())
            {
                var caminho = CaminhoPost(p);
                if (caminho != null && vistos.Add(caminho)) Url(caminho, p.Data, "yearly", "0.5");
            }

            sb.AppendLine("</urlset>");
            return Content(sb.ToString(), "application/xml; charset=utf-8");
        }

        [HttpGet("llms.txt")]
        [ResponseCache(Duration = 3600)]
        public async Task<IActionResult> Llms()
        {
            var sb = new StringBuilder();
            sb.AppendLine("# CDL Uberlândia");
            sb.AppendLine();
            sb.AppendLine("> " + GeoCatalogo.Resumo);
            sb.AppendLine();
            sb.AppendLine("## Dados da entidade");
            sb.AppendLine();
            sb.AppendLine("- Nome: CDL Uberlândia — Câmara de Dirigentes Lojistas de Uberlândia (CDL UDI)");
            sb.AppendLine("- Natureza: associação privada sem fins lucrativos, integrante do Sistema CNDL, ao lado da FCDL-MG e da Fundação CDL (FUNDECS)");
            sb.AppendLine("- Fundação: 1977");
            sb.AppendLine("- Endereço: Av. Belo Horizonte, 1290 - Osvaldo Rezende, Uberlândia - MG, CEP 38400-454");
            sb.AppendLine("- Telefone: (34) 3239-3400 · WhatsApp: (34) 3239-3457");
            sb.AppendLine("- E-mail: comercial@cdludi.org.br");
            sb.AppendLine("- Atendimento: segunda a sexta, das 8h às 18h");
            sb.AppendLine("- Site: " + Site + " · Loja (inscrições e compras): https://loja.cdludi.org.br");
            sb.AppendLine("- Redes: Instagram @cdluberlandia · facebook.com/cdluberlandia · LinkedIn cdl-uberl-ndia · YouTube");
            sb.AppendLine();
            sb.AppendLine("## Perguntas frequentes");
            sb.AppendLine();
            sb.AppendLine("- **Como consultar se meu nome está no SPC em Uberlândia?** A consulta para pessoa física é gratuita na sede da CDL Uberlândia (Av. Belo Horizonte, 1290); também pode ser feita pela loja online do SPC Brasil. Detalhes: " + Site + "/spc-consulta");
            sb.AppendLine("- **Como minha empresa se associa à CDL Uberlândia?** Pelo formulário em " + Site + "/associe-se ou pelo telefone (34) 3239-3400.");
            sb.AppendLine("- **A CDL Uberlândia emite certificado digital?** Sim, e-CPF e e-CNPJ: " + Site + "/certificado-digital");
            sb.AppendLine("- **A CDL aluga espaços para eventos?** Sim: auditório para até 448 pessoas, foyer e salas para 20, 60 e 120 pessoas: " + Site + "/cdl-convencoes-e-eventos");
            sb.AppendLine();

            foreach (var g in GeoCatalogo.Grupos)
            {
                sb.AppendLine("## " + g.Nome);
                sb.AppendLine();
                foreach (var p in g.Paginas)
                    sb.AppendLine($"- [{p.Titulo}]({Site}{p.Caminho}): {p.Descricao}");
                sb.AppendLine();
            }

            var recentes = (await Publicacoes()).Where(p => CaminhoPost(p) != null).Take(15).ToList();
            if (recentes.Count > 0)
            {
                sb.AppendLine("## Publicações recentes");
                sb.AppendLine();
                foreach (var p in recentes)
                {
                    var data = p.Data?.ToString("dd/MM/yyyy");
                    var sub = TextoLimpo(p.Subtitulo);
                    sb.AppendLine($"- [{TextoLimpo(p.Titulo)}]({Site}{CaminhoPost(p)})" + (data != null ? $" ({data})" : "") + (sub.Length > 0 ? ": " + sub : ""));
                }
                sb.AppendLine();
            }

            sb.AppendLine("## Optional");
            sb.AppendLine();
            sb.AppendLine($"- [Mapa do site (XML)]({Site}/sitemap.xml): lista completa de páginas e publicações");
            sb.AppendLine($"- [Política de Privacidade e LGPD]({Site}/lgpd)");
            return Content(sb.ToString(), "text/markdown; charset=utf-8");
        }

        // ------------------------------------------------------------------

        private static string? CaminhoPost(Post p)
        {
            var rota = p.PostCategoria?.Route;
            return string.IsNullOrWhiteSpace(rota) || string.IsNullOrWhiteSpace(p.Slug) ? null : $"/publicacoes/{rota}/{p.Slug}";
        }

        private static string TextoLimpo(string? html)
        {
            if (string.IsNullOrWhiteSpace(html)) return "";
            var t = WebUtility.HtmlDecode(Regex.Replace(html, "<[^>]+>", " "));
            return Regex.Replace(t, @"\s+", " ").Trim();
        }

        /// <summary>Todas as publicações (mais recentes primeiro), lidas da API página a página; cache de 1 hora. Sem API: lista vazia.</summary>
        private async Task<List<Post>> Publicacoes()
        {
            if (_cachePosts is { } c && DateTime.Now - c.quando < TimeSpan.FromHours(1)) return c.posts;
            await _trava.WaitAsync();
            try
            {
                if (_cachePosts is { } c2 && DateTime.Now - c2.quando < TimeSpan.FromHours(1)) return c2.posts;
                var todos = new List<Post>();
                try
                {
                    int total = 1;
                    for (int pagina = 1; pagina <= total && pagina <= 200; pagina++)
                    {
                        var resp = await _httpClient.GetAsync($"post/page/{pagina}");
                        if (!resp.IsSuccessStatusCode) break;
                        var lista = JsonSerializer.Deserialize<List<Post>>(await resp.Content.ReadAsStringAsync()) ?? new();
                        if (lista.Count == 0) break;
                        todos.AddRange(lista);
                        total = Math.Max(total, lista[0].TotalPages);
                    }
                }
                catch (Exception) { /* API fora do ar: segue com o que conseguiu ler */ }
                todos = todos.OrderByDescending(p => p.Data ?? DateTime.MinValue).ToList();
                if (todos.Count > 0) _cachePosts = (DateTime.Now, todos);
                return todos;
            }
            finally { _trava.Release(); }
        }
    }
}
