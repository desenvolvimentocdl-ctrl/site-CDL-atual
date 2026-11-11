using Microsoft.EntityFrameworkCore;
using SiteCDL.Data;
using SiteCDL.Models;
using SiteCDL.Utilities;
using System.Linq.Expressions;

namespace SiteCDL.Repository
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
