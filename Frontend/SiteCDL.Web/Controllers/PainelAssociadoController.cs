using System.Security.Claims;
using System.Text.Json;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SiteCDL.Models;
using SiteCDL.Services;

namespace SiteCDL.Web.Controllers
{
    /// <summary>
    /// Painel do Associado — DEMONSTRAÇÃO (usuário de teste em PainelDemoService).
    /// Login por cookie; nada é gravado em banco. Cursos/eventos e comunicados vêm das APIs reais do site.
    /// </summary>
    [Route("painel-associado")]
    public class PainelAssociadoController : Controller
    {
        public static readonly (string Id, string Nome)[] Secoes =
        {
            ("inicio", "Visão geral"), ("beneficios", "Benefícios"), ("boletos", "2ª via de boletos"),
            ("spc", "Consultas SPC"), ("cursos", "Cursos e eventos"), ("empresa", "Dados da empresa"), ("atendimento", "Atendimento"),
        };

        private readonly PainelDemoService _demo;
        private readonly HttpClient _api;
        public PainelAssociadoController(PainelDemoService demo, IHttpClientFactory http)
        {
            _demo = demo;
            _api = http.CreateClient("ApiHttpClient");
        }

        [HttpGet("")]
        public IActionResult Index() => User.Identity?.IsAuthenticated == true ? Redirect("/painel-associado/inicio") : Redirect("/painel-associado/entrar");

        [HttpGet("entrar")]
        public IActionResult Entrar(string? returnUrl = null)
        {
            if (User.Identity?.IsAuthenticated == true) return Redirect(Destino(returnUrl));
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost("entrar")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Entrar(string? login, string? senha, bool lembrar = false, string? returnUrl = null)
        {
            if (!_demo.Validar(login, senha))
            {
                ViewBag.Erro = "E-mail/CNPJ ou senha incorretos.";
                ViewBag.Login = login;
                ViewBag.ReturnUrl = returnUrl;
                return View();
            }
            var a = _demo.Dados();
            var id = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Name, a.Responsavel),
                new Claim(ClaimTypes.Email, a.Email),
                new Claim("empresa", a.NomeFantasia),
                new Claim("cnpj", a.Cnpj),
            }, CookieAuthenticationDefaults.AuthenticationScheme);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id),
                new AuthenticationProperties { IsPersistent = lembrar });
            return Redirect(Destino(returnUrl));
        }

        [HttpPost("sair")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Sair()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Redirect("/painel-associado/entrar");
        }

        [Authorize]
        [HttpGet("{secao}")]
        public async Task<IActionResult> Secao(string secao)
        {
            if (!Secoes.Any(s => s.Id == secao)) return NotFound();
            ViewBag.Secao = secao;
            ViewBag.Associado = _demo.Dados();

            if (secao is "inicio" or "cursos")
            {
                try
                {
                    ViewBag.Agenda = (await LojaController.Buscar("eventos")).Concat(await LojaController.Buscar("cursos"))
                        .OrderBy(i => i.Inicio ?? DateTime.MaxValue).Take(secao == "inicio" ? 3 : 6).ToList();
                }
                catch { ViewBag.Agenda = new List<LojaItem>(); }
            }
            if (secao is "inicio" or "atendimento")
            {
                try
                {
                    var json = await _api.GetStringAsync("post/comunicados/page/1");
                    ViewBag.Comunicados = (JsonSerializer.Deserialize<List<Post>>(json) ?? new()).Take(4).ToList();
                }
                catch { ViewBag.Comunicados = new List<Post>(); }
            }
            return View("Painel");
        }

        // Formulários do painel: na demonstração só confirmam o envio, sem gravar nada.
        [Authorize]
        [HttpPost("empresa")]
        [ValidateAntiForgeryToken]
        public IActionResult SalvarEmpresa()
        {
            TempData["Ok"] = "Dados recebidos. (Demonstração: nenhuma alteração foi gravada.)";
            return Redirect("/painel-associado/empresa");
        }

        [Authorize]
        [HttpPost("atendimento")]
        [ValidateAntiForgeryToken]
        public IActionResult AbrirChamado(string? assunto)
        {
            TempData["Ok"] = $"Solicitação \"{(string.IsNullOrWhiteSpace(assunto) ? "sem assunto" : assunto.Trim())}\" registrada. (Demonstração: nada foi enviado.)";
            return Redirect("/painel-associado/atendimento");
        }

        private string Destino(string? returnUrl) =>
            !string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl) && returnUrl.StartsWith("/painel-associado") ? returnUrl : "/painel-associado/inicio";
    }
}
