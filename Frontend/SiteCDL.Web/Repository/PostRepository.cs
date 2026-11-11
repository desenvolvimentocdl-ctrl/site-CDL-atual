using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using SiteCDL.Data;
using SiteCDL.Models;
using SiteCDL.Utilities;
using System.Linq.Expressions;

namespace SiteCDL.Repository
{
    public class PostRepository
    {

        private readonly DataContext _context;

        public PostRepository(DataContext context)
        {
            _context = context;
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
                    ImagemBase64 = pagina.ImagemBase64,
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
                                    .OrderByDescending(x => x.Codigo)
                                    .Where(expression)
                                    .Skip((dadosPaginacao.pageNumber - 1) * dadosPaginacao.pageSize)
                                    .Take(dadosPaginacao.pageSize)
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
