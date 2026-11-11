using SiteCDL.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteCDL.Application.Interfaces
{
    public interface IPostService
    {
        ValueTask<List<Post>> GetPostsByCategoryAsync(List<int> categories, bool takeAll = false, int takeAmount = 4);
        ValueTask<List<Post>> GetPostsByRouteNameAsync(string route, bool takeAll = false, int takeAmount = 4);
        ValueTask<List<Post>> GetPostsByRouteAsync(string route, bool takeAll = false, int pageNumber = 1, int takeAmount = 4);
        ValueTask<DetalhePostGeral> GetPostsBySlugAsync(string slug);
        ValueTask<List<Post>> GetAllPostsAsync(  bool takeAll = false, int pageNumber = 1, int takeAmount = 4);
    }
}
