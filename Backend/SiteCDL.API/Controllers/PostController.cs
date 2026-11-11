using Microsoft.AspNetCore.Mvc;
using SiteCDL.Application.Interfaces;

namespace SiteCDL.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostController : ControllerBase
    {
        private readonly IPostService _postService;

        public PostController(IPostService postService)
        {
            _postService = postService;
        }

        //[HttpGet()]
        //public async Task<IActionResult> GetPostsByCategoryAsync([FromBody] List<int> categories)
        //{
        //    var modelo = await _postService.GetPostsByCategoryAsync(categories);
        //    return Ok(modelo);
        //}
        //#region Notícias 

        //[HttpGet("noticias")]
        //public async Task<IActionResult> GetNoticias()
        //{
        //    var categories = new List<int> { 12 }; // Exemplo de categoria para "noticias"
        //    var posts = await _postService.GetPostsByCategoryAsync(categories, true);
        //    return Ok(posts);
        //}
        //[HttpGet("noticias/{slug}")]
        //public async Task<IActionResult> GetNoticias(string slug)
        //{
        //    var posts = await _postService.GetPostsBySlugAsync(slug);
        //    return Ok(posts);
        //}
        //#endregion
        //#region Cursos 
        //[HttpGet("cursos")]
        //public async Task<IActionResult> GetCursos()
        //{
        //    var categories = new List<int> { 7,13 }; // Exemplo de categoria para "comunicados"
        //    var posts = await _postService.GetPostsByCategoryAsync(categories, true);
        //    return Ok(posts);
        //}

        //[HttpGet("cursos/{slug}")]
        //public async Task<IActionResult> GetCurso(string slug)
        //{
        //    var posts = await _postService.GetPostsBySlugAsync(slug);
        //    return Ok(posts);
        //}
        //#endregion
        //#region Comunicados 
        //[HttpGet("comunicados")]
        //public async Task<IActionResult> GetComunicados()
        //{
        //    var categories = new List<int> { 6 }; // Exemplo de categoria para "comunicados"
        //    var posts = await _postService.GetPostsByCategoryAsync(categories, true);
        //    return Ok(posts);
        //}

        //[HttpGet("comunicados/{slug}")]
        //public async Task<IActionResult> GetComunicado(string slug)
        //{
        //    var posts = await _postService.GetPostsBySlugAsync(slug);
        //    return Ok(posts);
        //}
        //#endregion
        //#region CAE 
        //[HttpGet("cae")]
        //public async Task<IActionResult> GetCAE()
        //{
        //    var categories = new List<int> {3 }; // Exemplo de categoria para "comunicados"
        //    var posts = await _postService.GetPostsByCategoryAsync(categories, true);
        //    return Ok(posts);
        //}

        //[HttpGet("cae/{slug}")]
        //public async Task<IActionResult> GetCAE(string slug)
        //{
        //    var posts = await _postService.GetPostsBySlugAsync(slug);
        //    return Ok(posts);
        //}
        //#endregion

        //#region CDLDireto 
        //[HttpGet("cdldireto")]
        //public async Task<IActionResult> GetCDLDireto()
        //{
        //    var categories = new List<int> {4 }; // Exemplo de categoria para "comunicados"
        //    var posts = await _postService.GetPostsByCategoryAsync(categories, true);
        //    return Ok(posts);
        //}

        //[HttpGet("cdldireto/{slug}")]
        //public async Task<IActionResult> GetCDLDireto(string slug)
        //{
        //    var posts = await _postService.GetPostsBySlugAsync(slug);
        //    return Ok(posts);
        //}
        //#endregion

        //#region CDLDireto 
        //[HttpGet("pesquisas")]
        //public async Task<IActionResult> GetPesquisas()
        //{
        //    var categories = new List<int> { 9 }; // Exemplo de categoria para "comunicados"
        //    var posts = await _postService.GetPostsByCategoryAsync(categories, true);
        //    return Ok(posts);
        //}

        //[HttpGet("pesquisas/{slug}")]
        //public async Task<IActionResult> GetPesquisa(string slug)
        //{
        //    var posts = await _postService.GetPostsBySlugAsync(slug);
        //    return Ok(posts);
        //}
        //#endregion

        //#region CDLAcao 
        //[HttpGet("cdlacao")]
        //public async Task<IActionResult> GetCDLAcao()
        //{
        //    var categories = new List<int> { 5 }; // Exemplo de categoria para "comunicados"
        //    var posts = await _postService.GetPostsByCategoryAsync(categories, true);
        //    return Ok(posts);
        //}

        //[HttpGet("cdlacao/{slug}")]
        //public async Task<IActionResult> GetCDLAcao(string slug)
        //{
        //    var posts = await _postService.GetPostsBySlugAsync(slug);
        //    return Ok(posts);
        //}
        //#endregion

        //#region Eventos 
        //[HttpGet("eventos")]
        //public async Task<IActionResult> GetEventos()
        //{
        //    var categories = new List<int> { 8 }; // Exemplo de categoria para "comunicados"
        //    var posts = await _postService.GetPostsByCategoryAsync(categories, true);
        //    return Ok(posts);
        //}

        //[HttpGet("eventos/{slug}")]
        //public async Task<IActionResult> GetEventos(string slug)
        //{
        //    var posts = await _postService.GetPostsBySlugAsync(slug);
        //    return Ok(posts);
        //}
        //#endregion


        //#region Outros 
        //[HttpGet("outros")]
        //public async Task<IActionResult> GetOutros()
        //{
        //    var categories = new List<int> { 11 }; // Exemplo de categoria para "comunicados"
        //    var posts = await _postService.GetPostsByCategoryAsync(categories, true);
        //    return Ok(posts);
        //}

        //[HttpGet("outros/{slug}")]
        //public async Task<IActionResult> GetOutros(string slug)
        //{
        //    var posts = await _postService.GetPostsBySlugAsync(slug);
        //    return Ok(posts);
        //}
        //#endregion

        #region Posts 
        [HttpGet("{route}/page/{pageNumber}")]
        public async Task<IActionResult> GetPosts(string route, int pageNumber = 1)
        {
            var categories = new List<int> { 11 }; // Exemplo de categoria para "comunicados"
            var posts = await _postService.GetPostsByRouteAsync(route,true, pageNumber);
            return Ok(posts);
        }

         
        //[HttpGet("teste/{route}/{pageNumber}")]
        //public async Task<IActionResult> GetPostsTeste(string route, int pageNumber = 1)
        //{
        //    var categories = new List<int> { 11 }; // Exemplo de categoria para "comunicados"
        //    var posts = await _postService.GetPostsByRouteAsync(route, true,pageNumber);
        //    return Ok(posts);
        //}


        [HttpGet("{route}/{slug}")]
        public async Task<IActionResult> GetPost(string route,string slug)
        {
            var posts = await _postService.GetPostsBySlugAsync(slug);
            return Ok(posts);
        }
         
        [HttpGet("page/{pageNumber}")]
        public async Task<IActionResult> GetPosts(  int pageNumber = 1)
        {
            var categories = new List<int> { 11 }; // Exemplo de categoria para "comunicados"
            var posts = await _postService.GetAllPostsAsync( true, pageNumber);
            return Ok(posts);
        }

        #endregion
    }
}
