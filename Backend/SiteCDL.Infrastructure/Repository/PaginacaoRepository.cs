using SiteCDL.Infrastructure.Data;
using System.Linq.Expressions;
using SiteCDL.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace SiteCDL.Infrastructure.Repository
{
    public class PaginacaoRepository
    {

        private readonly DataContext _context;

        public PaginacaoRepository(DataContext context)
        {
            _context = context;
        }
    
        public async Task<int> QuantidadeDeLinhas(Expression<Func<Post, bool>> expression)
        {            
            return await _context.Post.Include(a => a.PostCategoria).Where(expression).CountAsync();
        }

    }
}
