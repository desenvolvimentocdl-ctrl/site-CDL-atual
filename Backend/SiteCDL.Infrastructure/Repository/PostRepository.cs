using Microsoft.EntityFrameworkCore;
using SiteCDL.Application.Utilities;
using SiteCDL.Infrastructure.Data;
using SiteCDL.Domain.Entities;
using System.Linq.Expressions;
using SiteCDL.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using SiteCDL.Utilities;

namespace SiteCDL.Infrastructure.Repository
{
    public class PostRepository : IPostRepository

    {

        private readonly DataContext _context;
        private readonly PaginacaoRepository _paginacaoRepository;

        public PostRepository(DataContext context, PaginacaoRepository paginacaoRepository)
        {
            _context = context;
            _paginacaoRepository = paginacaoRepository;
        }

        public async ValueTask<List<Post>> GetPostsByCategoryAsync(List<int> categories, bool takeAll = false, int takeAmount = 4)
        {


            var query = _context.Post
                .Include(a => a.PostCategoria)
                .Where(x => categories.Contains(x.PostCategoria.Codigo) && x.Status)
                .OrderByDescending(x => x.Codigo)
                .Select(p => new Post
                {
                    Codigo = p.Codigo,
                    Imagem = p.Imagem,
                    Data = p.Data,
                    Titulo = p.Titulo,
                    Slug = p.Slug,
                    Subtitulo = p.Subtitulo,
                    PostCategoria = p.PostCategoria,
                    Conteudo = System.Net.WebUtility.HtmlEncode(p.Conteudo),
                    DataInicio = p.DataInicio,
                    DataFim = p.DataFim,
                    Link = p.Link,
                    LinkFotos = p.LinkFotos
                });

            if (!takeAll)
            {
                query = query.Take(takeAmount);
            }

            return await query.ToListAsync();
        }

        public async ValueTask<List<Post>> GetPostsByRouteNameAsync(string route, bool takeAll = false, int takeAmount = 4)
        {


            var query = _context.Post
                .Include(a => a.PostCategoria)
                .Where(x => x.PostCategoria.Route == route && x.Status)
                .OrderByDescending(x => x.Codigo)
                .Select(p => new Post
                {
                    Codigo = p.Codigo,
                    Imagem = p.Imagem,
                    //ImagemBase64 = p.ImagemBase64,
                    Data = p.Data,
                    Titulo = p.Titulo,
                    Slug = p.Slug,
                    Subtitulo = p.Subtitulo,
                    PostCategoria = p.PostCategoria,
                    Conteudo = System.Net.WebUtility.HtmlEncode(p.Conteudo)
                });

            if (!takeAll)
            {
                query = query.Take(takeAmount);
            }

            return await query.ToListAsync();
        }

        public async ValueTask<List<Post>> GetPostsByRouteAsync(string route, bool takeAll = false, int pageNumber = 1, int takeAmount = 4)
        {


            Expression<Func<Post, bool>> expression = x => x.PostCategoria.Route == route && x.Status;
                Paginacao objPaginacao = new(_paginacaoRepository, pageNumber, 12, expression);
                Paginacao.RetornaDadosPaginacao pageNumberValidado = await objPaginacao.ValidaQuantidadePagina();



            List<Post> listPostsPaginado = await SearchPostByCategoryRepository(pageNumberValidado, expression);

             
            if (!takeAll)
            {
                listPostsPaginado = listPostsPaginado.Take(takeAmount).ToList();
            }

            return objPaginacao.SetValuesPaginacao(listPostsPaginado.ToList());

        
        }

        public async ValueTask<List<Post>> GetAllPostsAsync(  bool takeAll = false, int pageNumber = 1, int takeAmount = 4)
        {


            Expression<Func<Post, bool>> expression = x =>   x.Status;
            Paginacao objPaginacao = new(_paginacaoRepository, pageNumber, 12, expression);
            Paginacao.RetornaDadosPaginacao pageNumberValidado = await objPaginacao.ValidaQuantidadePagina();



            List<Post> listPostsPaginado = await SearchAllPosts(pageNumberValidado, expression);


            if (!takeAll)
            {
                listPostsPaginado = listPostsPaginado.OrderByDescending(x=>x.Data).Take(takeAmount).ToList();
            }

            return objPaginacao.SetValuesPaginacao(listPostsPaginado.ToList());


        }

        public async ValueTask<DetalhePostGeral> GetPostsBySlugAsync(string slug)
        {
            try
            {


                var post = _context.Post
                    .Include(a => a.PostCategoria)
                    .Where(x => x.Slug == slug && x.Status).OrderByDescending(x => x.Codigo)
                    .Select(p => new Post
                    {
                        Codigo = p.Codigo,
                        Imagem = p.Imagem,
                        //ImagemBase64 = p.ImagemBase64,
                        Data = p.Data,
                        Titulo = p.Titulo,
                        Slug = p.Slug,
                        Subtitulo = p.Subtitulo,
                        PostCategoria = p.PostCategoria,
                        Conteudo = System.Net.WebUtility.HtmlEncode(p.Conteudo).Replace("\r\n","<br />"),
                        Link = p.Link,
                        LinkFotos = p.LinkFotos
                    }).FirstOrDefaultAsync();

                

                var postsRelacionados = new List<Post>();

                if (post != null)
                {
                    if (post.Result.PostCategoria != null && post.Result.PostCategoria.Nome != null)
                    {
                        postsRelacionados = await _context.Post
                            .Include(a => a.PostCategoria)
                            .Where(x => x.PostCategoria.Nome == post.Result.PostCategoria.Nome && x.Codigo <= post.Result.Codigo - 3 && x.Codigo != post.Result.Codigo && x.Status == true)
                            .OrderByDescending(x => x.Codigo)
                            .Take(3)
                            .Select(p => new Post
                            {
                                Codigo = p.Codigo,
                                Imagem = p.Imagem,
                                //ImagemBase64 = p.ImagemBase64,
                                Data = p.Data,
                                Titulo = p.Titulo,
                                Slug = p.Slug,
                                Subtitulo = p.Subtitulo,
                                PostCategoria = p.PostCategoria,
                                //Conteudo = System.Net.WebUtility.HtmlEncode(p.Conteudo)
                            })
                            .ToListAsync();
                    }
                }

                DetalhePostGeral model = new()
                {
                    Noticia = post.Result,
                    PostsRelacionados = postsRelacionados
                };

                return model;
            }
            catch
            {
                return new DetalhePostGeral();
            }

         }

        public async Task<List<Post>> GetPostsRepositoryAsync(Expression<Func<Post, bool>> expression)
        {
            try
            {
                return await _context.Post.Include(a => a.PostCategoria).Where(expression).OrderBy(x => x.DataInicio).Take(4).ToListAsync();
            }
            catch
            {
                return new List<Post>();
            }
        }


        public async ValueTask<List<Post>> GetSearchPostsTitleRepositoryAsync(string id)
        {
            try
            {

                return await _context.Post.Where(a => EF.Functions.Like(a.Titulo, $"%{id}%") && a.Status).Include(r => r.PostCategoria).ToListAsync();


            }
            catch
            {
                return new List<Post>();
            }
        }


        public async ValueTask<List<Post>> GetSearchPostsContentRepositoryAsync(string id)
        {
            try
            {

                var paginas = await _context.Post.ToListAsync();

                var paginasSemTags = paginas.Select(pagina => new Post
                {
                    Codigo = pagina.Codigo,
                    Imagem = pagina.Imagem,
                    //ImagemBase64 = pagina.ImagemBase64,
                    Data = pagina.Data,
                    Titulo = pagina.Titulo,
                    Slug = pagina.Slug,
                    Subtitulo = pagina.Subtitulo,
                    PostCategoria = pagina.PostCategoria,
                    Conteudo = Remover.HtmlTags(pagina.Conteudo)
                }).ToList();

                return paginasSemTags
                     .Where(pagina => pagina.Conteudo.IndexOf(id, StringComparison.OrdinalIgnoreCase) >= 0)
                     .ToList();

            }
            catch
            {
                return new List<Post>();
            }
        }


        public async ValueTask<List<Post>> SearchPostByCategoryRepository(Paginacao.RetornaDadosPaginacao dadosPaginacao, Expression<Func<Post, bool>> expression)
        {
            try
            {
                return await _context.Post
                                    .Include(a => a.PostCategoria)
                                    .OrderByDescending(x => x.Data)
                                    .Where(expression)
                                    .Skip((dadosPaginacao.pageNumber - 1) * dadosPaginacao.pageSize)
                                    .Take(dadosPaginacao.pageSize)
                                    .Select(p => new Post
                                    {
                                        Codigo = p.Codigo,
                                        Imagem = p.Imagem,
                                        //ImagemBase64 = p.ImagemBase64,
                                        Data = p.Data,
                                        Titulo = p.Titulo,
                                        Slug = p.Slug,
                                        Subtitulo = p.Subtitulo,
                                        PostCategoria = p.PostCategoria,
                                        Conteudo = System.Net.WebUtility.HtmlEncode(p.Conteudo)
                                    })
                                    .ToListAsync();
            }
            catch
            {
                return new List<Post>();
            }
        }

        public async ValueTask<List<Post>> SearchAllPosts(Paginacao.RetornaDadosPaginacao dadosPaginacao, Expression<Func<Post, bool>> expression)
        {
            try
            {
                return await _context.Post
                                    .Include(a => a.PostCategoria)
                                    .OrderByDescending(x => x.Data)
                                    .Where(expression)
                                    .Skip((dadosPaginacao.pageNumber - 1) * dadosPaginacao.pageSize)
                                    .Take(dadosPaginacao.pageSize)
                                    
                                    .Select(p => new Post
                                    {
                                        Codigo = p.Codigo,
                                        Imagem = p.Imagem,
                                        //ImagemBase64 = p.ImagemBase64,
                                        Data = p.Data,
                                        Titulo = p.Titulo,
                                        Slug = p.Slug,
                                        Subtitulo = p.Subtitulo,
                                        PostCategoria = p.PostCategoria,
                                        Conteudo = System.Net.WebUtility.HtmlEncode(p.Conteudo)
                                    })
                                    .ToListAsync();
            }
            catch
            {
                return new List<Post>();
            }
        }


        public async Task<DetalhePostGeral> RelatedPostsRepository(string id, Expression<Func<Post, bool>> expression)
        {

            try
            {
                var noticia = await _context.Post
                                            .Include(a => a.PostCategoria)
                                            .OrderByDescending(x => x.Codigo)
                                            .Where(x => x.Slug == id && x.Status)
                                            .Where(expression)
                                            .FirstOrDefaultAsync();

                var postsRelacionados = new List<Post>();

                if (noticia != null)
                {
                    if (noticia.PostCategoria != null && noticia.PostCategoria.Nome != null)
                    {
                        postsRelacionados = await _context.Post.Include(a => a.PostCategoria).Where(x => x.PostCategoria.Nome == noticia.PostCategoria.Nome && x.Slug != id && x.Status == true).OrderByDescending(x => x.Codigo).Take(3).ToListAsync();
                    }
                }

                DetalhePostGeral model = new()
                {
                    Noticia = noticia,
                    PostsRelacionados = postsRelacionados
                };

                return model;
            }
            catch
            {
                return new DetalhePostGeral();
            }

        }

 
    }
}
