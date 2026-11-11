using Microsoft.EntityFrameworkCore;
 using SiteCDL.Domain.Entities;
using SiteCDL.Application;
using SiteCDL.Application.Utilities;
using SiteCDL.Infrastructure.Data;
using SiteCDL.Domain.Interfaces.Repositories;

namespace SiteCDL.Infrastructure.Repository
{
    public class PageRepository : IPageRepository
    {

		private readonly DataContext _context;

		public PageRepository(DataContext context)
		{
			_context = context;
		}

		public async ValueTask<Paginas> GetPageAsync(string slug)
		{
            try
            {
                return await _context.Paginas.Where(a => a.Slug == slug && a.Status).FirstOrDefaultAsync() ?? new Paginas();
            }
            catch
            {
                return new Paginas();
            }
        }


		public async ValueTask<List<Paginas>> GetSearchPagesTitleRepositoryAsync(string id)
		{
			try
			{
				return await _context.Paginas.Where(a => EF.Functions.Like(a.Titulo, $"%{id}%") && a.Status).ToListAsync();
			}
			catch
			{
				return new List<Paginas>();
			}
		}


		public async ValueTask<List<Paginas>> GetSearchPagesContentRepositoryAsync(string id)
		{
			try
			{
				var paginas = await _context.Paginas.Where(a => a.Status).ToListAsync();

				var paginasSemTags = paginas.Select(pagina => new Paginas
				{
					Codigo = pagina.Codigo,
					Titulo = pagina.Titulo,
					Conteudo = Remover.HtmlTags(pagina.Conteudo),
					Controller = pagina.Controller,
					Seo = pagina.Seo,
					Slug = pagina.Slug
                }).ToList();

				return paginasSemTags
					 .Where(pagina => pagina.Conteudo.IndexOf(id, StringComparison.OrdinalIgnoreCase) >= 0)
					 .ToList();

			}
			catch (Exception ex)
			{
				return new List<Paginas>();
			}
		}

	}
}
