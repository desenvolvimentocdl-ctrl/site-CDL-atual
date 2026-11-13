using System.Xml.Linq;

namespace SiteCDL.Services
{
    /// <summary>
    /// Últimos vídeos do canal da CDL no YouTube (TV CDL Uberlândia), lidos do feed RSS público e oficial do YouTube.
    /// Guarda o resultado por 30 minutos; se o YouTube não responder em 3 s, devolve a última lista boa (ou vazia).
    /// Usado em Views/Paginas/redes-sociais.cshtml.
    /// </summary>
    public static class YoutubeFeed
    {
        public const string CanalId = "UCGmeCRd9vgxMd8x6CJjsQHA";
        public record Video(string Id, string Titulo, DateTime Publicado);

        private static readonly HttpClient Http = new() { Timeout = TimeSpan.FromSeconds(3) };
        private static List<Video> _cache = new();
        private static DateTime _validoAte = DateTime.MinValue;
        private static readonly SemaphoreSlim Trava = new(1, 1);

        public static async Task<List<Video>> Ultimos(int qtd = 6)
        {
            if (DateTime.UtcNow < _validoAte) return _cache.Take(qtd).ToList();
            await Trava.WaitAsync();
            try
            {
                if (DateTime.UtcNow < _validoAte) return _cache.Take(qtd).ToList();
                var xml = await Http.GetStringAsync($"https://www.youtube.com/feeds/videos.xml?channel_id={CanalId}");
                XNamespace a = "http://www.w3.org/2005/Atom", yt = "http://www.youtube.com/xml/schemas/2015";
                _cache = XDocument.Parse(xml).Root!.Elements(a + "entry")
                    .Select(e => new Video(
                        (string?)e.Element(yt + "videoId") ?? "",
                        (string?)e.Element(a + "title") ?? "",
                        DateTime.TryParse((string?)e.Element(a + "published"), out var d) ? d : DateTime.MinValue))
                    .Where(v => System.Text.RegularExpressions.Regex.IsMatch(v.Id, "^[A-Za-z0-9_-]{6,20}$"))
                    .OrderByDescending(v => v.Publicado).ToList();
                _validoAte = DateTime.UtcNow.AddMinutes(30);
            }
            catch
            {
                _validoAte = DateTime.UtcNow.AddMinutes(2); // tenta de novo em 2 min, mantendo a última lista boa
            }
            finally { Trava.Release(); }
            return _cache.Take(qtd).ToList();
        }
    }
}
