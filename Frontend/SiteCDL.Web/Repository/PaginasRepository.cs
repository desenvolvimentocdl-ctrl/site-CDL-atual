using Microsoft.EntityFrameworkCore;
using SiteCDL.Controllers;
using SiteCDL.Data;
using SiteCDL.Models;
using SiteCDL.Utilities;
using System.Linq.Expressions;

namespace SiteCDL.Repository
{
	public class PaginasRepository
	{

		private readonly DataContext _context;

		public PaginasRepository(DataContext context)
		{
			_context = context;
		}

		public async ValueTask<Paginas> GetContentRepositoryAsync(string slug)
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
					Slug = pagina.Slug,
                    Seo = pagina.Seo
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
