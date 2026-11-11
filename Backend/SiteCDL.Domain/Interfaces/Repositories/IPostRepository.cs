using SiteCDL.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SiteCDL.Domain.Interfaces.Repositories
{
    public interface IPostRepository
    {
        ValueTask<List<Post>> GetPostsByCategoryAsync(List<int> categories, bool takeAll = false, int takeAmount = 4);
        ValueTask<List<Post>> GetPostsByRouteNameAsync(string route, bool takeAll = false, int takeAmount = 4);
        ValueTask<List<Post>> GetPostsByRouteAsync(string route, bool takeAll = false, int pageNumber = 1, int takeAmount = 4 );
        ValueTask<DetalhePostGeral> GetPostsBySlugAsync(string slug);
        ValueTask<List<Post>> GetAllPostsAsync(bool takeAll = false, int pageNumber = 1, int takeAmount = 4);

        Task<List<Post>> GetPostsRepositoryAsync(Expression<Func<Post, bool>> expression);
        ValueTask<List<Post>> GetSearchPostsTitleRepositoryAsync(string id);
        ValueTask<List<Post>> GetSearchPostsContentRepositoryAsync(string id);
        Task<DetalhePostGeral> RelatedPostsRepository(string id, Expression<Func<Post, bool>> expression);
    }
}
