using System.Collections.Concurrent;
using System.Text.Json;
using System.Text.RegularExpressions;
using SiteCDL.Models;

namespace SiteCDL.Services
{
    /// <summary>
    /// Conta as visualizações das publicações (abertura da matéria) e devolve as mais lidas.
    /// Os números ficam em App_Data/visualizacoes.json no servidor do site — sem alterar o banco.
    /// Robôs são ignorados e a mesma pessoa (IP) só conta uma vez por matéria a cada 30 minutos.
    /// </summary>
    public class MaisLidasService
    {
        public class Item
        {
            public string Rota { get; set; } = "";
            public string Slug { get; set; } = "";
            public string Titulo { get; set; } = "";
            public DateTime? Data { get; set; }
            public int Visualizacoes { get; set; }
        }

        private static readonly Regex Robo = new("bot|crawl|spider|slurp|preview|facebookexternalhit|whatsapp|curl|wget|python|headless", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        private static readonly TimeSpan JanelaRepeticao = TimeSpan.FromMinutes(30);

        private readonly string _arquivo;
        private readonly object _trava = new();
        private readonly Dictionary<string, Item> _itens;
        private readonly ConcurrentDictionary<string, DateTime> _recentes = new();
        private readonly ILogger<MaisLidasService> _log;

        public MaisLidasService(IWebHostEnvironment env, ILogger<MaisLidasService> log)
        {
            _log = log;
            var pasta = Path.Combine(env.ContentRootPath, "App_Data");
            Directory.CreateDirectory(pasta);
            _arquivo = Path.Combine(pasta, "visualizacoes.json");
            _itens = Carregar();
        }

        public void Registrar(Post? post, HttpContext ctx)
        {
            if (post == null || string.IsNullOrWhiteSpace(post.Slug)) return;
            var ua = ctx.Request.Headers.UserAgent.ToString();
            if (ua == "" || Robo.IsMatch(ua)) return;

            var rota = post.PostCategoria?.Route ?? "";
            var chave = (rota + "/" + post.Slug).ToLowerInvariant();

            // mesma pessoa abrindo/atualizando a mesma matéria em sequência conta uma vez só
            var quem = ctx.Connection.RemoteIpAddress?.ToString() ?? "?";
            var agora = DateTime.UtcNow;
            var marca = quem + "|" + chave;
            if (_recentes.TryGetValue(marca, out var ultima) && agora - ultima < JanelaRepeticao) return;
            _recentes[marca] = agora;
            if (_recentes.Count > 5000)
                foreach (var k in _recentes.Where(x => agora - x.Value > JanelaRepeticao).Select(x => x.Key).ToList())
                    _recentes.TryRemove(k, out _);

            lock (_trava)
            {
                if (!_itens.TryGetValue(chave, out var item))
                    _itens[chave] = item = new Item { Rota = rota, Slug = post.Slug! };
                item.Titulo = TextoLimpo(post.Titulo);
                item.Data = post.Data;
                item.Visualizacoes++;
                Salvar();
            }
        }

        public List<Item> Top(int quantidade)
        {
            lock (_trava)
                return _itens.Values.OrderByDescending(i => i.Visualizacoes).ThenByDescending(i => i.Data)
                             .Take(quantidade).Select(i => new Item { Rota = i.Rota, Slug = i.Slug, Titulo = i.Titulo, Data = i.Data, Visualizacoes = i.Visualizacoes })
                             .ToList();
        }

        public static string TextoLimpo(string? html) =>
            System.Net.WebUtility.HtmlDecode(Regex.Replace(html ?? "", "<[^>]+>", "")).Trim();

        private Dictionary<string, Item> Carregar()
        {
            try
            {
                if (File.Exists(_arquivo))
                    return JsonSerializer.Deserialize<Dictionary<string, Item>>(File.ReadAllText(_arquivo)) ?? new();
            }
            catch (Exception e) { _log.LogWarning(e, "Não foi possível ler {Arquivo}; começando a contagem do zero.", _arquivo); }
            return new();
        }

        private void Salvar()
        {
            try
            {
                var tmp = _arquivo + ".tmp";
                File.WriteAllText(tmp, JsonSerializer.Serialize(_itens));
                File.Move(tmp, _arquivo, overwrite: true);
            }
            catch (Exception e) { _log.LogWarning(e, "Não foi possível gravar {Arquivo}.", _arquivo); }
        }
    }
}
