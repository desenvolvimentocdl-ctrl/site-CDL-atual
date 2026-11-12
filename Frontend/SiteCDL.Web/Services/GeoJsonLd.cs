using System.Text.Encodings.Web;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace SiteCDL.Services
{
    /// <summary>
    /// GEO: dados estruturados (schema.org em JSON-LD) colocados no &lt;head&gt; de todas as páginas pelo _Layout.
    /// Buscadores e assistentes de IA usam esses dados para entender quem é a CDL, do que trata cada página e quando cada matéria foi publicada.
    ///
    /// Nas views, opcionalmente:
    ///   ViewData["Breadcrumb"] = new List&lt;(string nome, string caminho)&gt; { ("Publicações", "/publicacoes"), ... }  // trilha própria
    ///   ViewData["JsonLd"]     = objeto (Dictionary) extra para o @graph, ex.: GeoJsonLd.Artigo(...)
    ///   ViewData["OgType"], ViewData["OgImage"], ViewData["Canonical"], ViewData["NoIndex"] = true
    /// </summary>
    public static class GeoJsonLd
    {
        private static readonly JsonSerializerOptions Opcoes = new()
        {
            // escapa < > & (seguro dentro de <script>), mantém acentos legíveis
            Encoder = JavaScriptEncoder.Create(System.Text.Unicode.UnicodeRanges.All),
            DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull,
        };

        public static string OrgId(string site) => site + "/#organizacao";
        public static string WebSiteId(string site) => site + "/#site";

        public static string Montar(string site, string urlPagina, string titulo, string? descricao, ViewDataDictionary viewData, bool home)
        {
            var grafo = new List<object>
            {
                Organizacao(site),
                new Dictionary<string, object?>
                {
                    ["@type"] = "WebSite",
                    ["@id"] = WebSiteId(site),
                    ["url"] = site + "/",
                    ["name"] = "CDL Uberlândia",
                    ["inLanguage"] = "pt-BR",
                    ["publisher"] = new Dictionary<string, object?> { ["@id"] = OrgId(site) },
                    ["potentialAction"] = new Dictionary<string, object?>
                    {
                        ["@type"] = "SearchAction",
                        ["target"] = new Dictionary<string, object?> { ["@type"] = "EntryPoint", ["urlTemplate"] = site + "/buscar?id={search_term_string}" },
                        ["query-input"] = "required name=search_term_string",
                    },
                },
                new Dictionary<string, object?>
                {
                    ["@type"] = "WebPage",
                    ["@id"] = urlPagina + "#pagina",
                    ["url"] = urlPagina,
                    ["name"] = titulo,
                    ["description"] = descricao,
                    ["inLanguage"] = "pt-BR",
                    ["isPartOf"] = new Dictionary<string, object?> { ["@id"] = WebSiteId(site) },
                    ["about"] = home ? new Dictionary<string, object?> { ["@id"] = OrgId(site) } : null,
                },
            };

            if (!home)
            {
                var trilha = viewData["Breadcrumb"] as List<(string nome, string caminho)>
                             ?? new List<(string nome, string caminho)> { (titulo, urlPagina) };
                var itens = new List<object> { ItemTrilha(1, "Início", site + "/") };
                int pos = 2;
                foreach (var (nome, caminho) in trilha)
                    itens.Add(ItemTrilha(pos++, nome, caminho.StartsWith("http") ? caminho : site + caminho));
                grafo.Add(new Dictionary<string, object?> { ["@type"] = "BreadcrumbList", ["itemListElement"] = itens });
            }

            if (viewData["JsonLd"] is { } extra) grafo.Add(extra);

            return JsonSerializer.Serialize(new Dictionary<string, object?> { ["@context"] = "https://schema.org", ["@graph"] = grafo }, Opcoes);
        }

        private static object ItemTrilha(int pos, string nome, string url) =>
            new Dictionary<string, object?> { ["@type"] = "ListItem", ["position"] = pos, ["name"] = nome, ["item"] = url };

        public static Dictionary<string, object?> Organizacao(string site) => new()
        {
            ["@type"] = new[] { "Organization", "LocalBusiness" },
            ["@id"] = OrgId(site),
            ["name"] = "CDL Uberlândia",
            ["legalName"] = "Câmara de Dirigentes Lojistas de Uberlândia",
            ["alternateName"] = new[] { "CDL UDI", "Câmara de Dirigentes Lojistas de Uberlândia", "CDL Uberlandia" },
            ["description"] = GeoCatalogo.Resumo,
            ["url"] = site + "/",
            ["logo"] = new Dictionary<string, object?> { ["@type"] = "ImageObject", ["url"] = site + "/img/logo.png" },
            ["image"] = site + "/img/logo.png",
            ["telephone"] = "+55 34 3239-3400",
            ["email"] = "comercial@cdludi.org.br",
            ["foundingDate"] = "1977",
            ["address"] = new Dictionary<string, object?>
            {
                ["@type"] = "PostalAddress",
                ["streetAddress"] = "Av. Belo Horizonte, 1290 - Osvaldo Rezende",
                ["addressLocality"] = "Uberlândia",
                ["addressRegion"] = "MG",
                ["postalCode"] = "38400-454",
                ["addressCountry"] = "BR",
            },
            ["areaServed"] = new Dictionary<string, object?> { ["@type"] = "City", ["name"] = "Uberlândia, MG" },
            ["contactPoint"] = new object[]
            {
                new Dictionary<string, object?> { ["@type"] = "ContactPoint", ["contactType"] = "customer service", ["telephone"] = "+55 34 3239-3400", ["email"] = "comercial@cdludi.org.br", ["availableLanguage"] = "Portuguese", ["areaServed"] = "BR" },
                new Dictionary<string, object?> { ["@type"] = "ContactPoint", ["contactType"] = "WhatsApp", ["telephone"] = "+55 34 3239-3457", ["availableLanguage"] = "Portuguese" },
            },
            ["openingHoursSpecification"] = new object[]
            {
                new Dictionary<string, object?> { ["@type"] = "OpeningHoursSpecification", ["dayOfWeek"] = new[] { "Monday", "Tuesday", "Wednesday", "Thursday", "Friday" }, ["opens"] = "08:00", ["closes"] = "18:00" },
            },
            ["parentOrganization"] = new Dictionary<string, object?> { ["@type"] = "Organization", ["name"] = "CNDL — Confederação Nacional de Dirigentes Lojistas", ["url"] = "https://cndl.org.br/" },
            ["knowsAbout"] = new[] { "SPC Brasil", "consulta de CPF e CNPJ", "certificado digital", "plano de saúde empresarial", "varejo", "comércio de Uberlândia", "cursos para empresários", "locação de auditório e salas para eventos" },
            ["sameAs"] = new[]
            {
                "https://www.instagram.com/cdluberlandia/",
                "https://www.facebook.com/cdluberlandia",
                "https://www.linkedin.com/company/cdl-uberl-ndia/",
                "https://www.youtube.com/channel/UCGmeCRd9vgxMd8x6CJjsQHA/",
                "https://twitter.com/CDLUBERLANDIA",
            },
        };

        /// <summary>Matéria (NewsArticle) para a página de uma publicação.</summary>
        public static Dictionary<string, object?> Artigo(string site, string url, string titulo, string? resumo, string? imagem, DateTime? publicado, string? secao, string? corpoTexto)
        {
            string? Iso(DateTime? d) => d?.ToString("yyyy-MM-dd'T'HH:mm:ss") + (d != null ? "-03:00" : null);
            return new()
            {
                ["@type"] = "NewsArticle",
                ["@id"] = url + "#materia",
                ["mainEntityOfPage"] = new Dictionary<string, object?> { ["@id"] = url + "#pagina" },
                ["headline"] = titulo.Length > 110 ? titulo[..110] : titulo,
                ["description"] = resumo,
                ["image"] = string.IsNullOrEmpty(imagem) ? null : new[] { imagem },
                ["datePublished"] = Iso(publicado),
                ["dateModified"] = Iso(publicado),
                ["articleSection"] = secao,
                ["inLanguage"] = "pt-BR",
                ["author"] = new Dictionary<string, object?> { ["@id"] = OrgId(site) },
                ["publisher"] = new Dictionary<string, object?> { ["@id"] = OrgId(site) },
                ["wordCount"] = string.IsNullOrWhiteSpace(corpoTexto) ? null : corpoTexto.Split((char[]?)null, StringSplitOptions.RemoveEmptyEntries).Length,
            };
        }
    }
}
