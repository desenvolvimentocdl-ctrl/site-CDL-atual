using SiteCDL.Application.Interfaces;
using SiteCDL.Domain.Entities;
using SiteCDL.Domain.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteCDL.Application.Services
{
    public class PostService :IPostService
    {
        private readonly IPostRepository _postRepository;
        public PostService(IPostRepository postRepository)
        {
            _postRepository = postRepository;   
        }

        public async ValueTask<List<Post>> GetPostsByCategoryAsync(List<int> categories, bool takeAll = false, int takeAmount = 4)
        {
            return await _postRepository.GetPostsByCategoryAsync(categories,takeAll,takeAmount);

        }
        public async ValueTask<List<Post>> GetPostsByRouteNameAsync(string route, bool takeAll = false, int takeAmount = 4)
        {
            return await _postRepository.GetPostsByRouteNameAsync(route,takeAll,takeAmount);

        }

        public async ValueTask<List<Post>> GetPostsByRouteAsync(string route, bool takeAll = false, int pageNumber = 1, int takeAmount = 4)
        {
            return await _postRepository.GetPostsByRouteAsync(route, takeAll, pageNumber, takeAmount);

        }
        public async ValueTask<DetalhePostGeral> GetPostsBySlugAsync(string slug)
        {
            return await _postRepository.GetPostsBySlugAsync(slug);

        }

        public async ValueTask<List<Post>> GetAllPostsAsync(bool takeAll = false, int pageNumber = 1, int takeAmount = 4)
        {
            return await _postRepository.GetAllPostsAsync(takeAll,pageNumber,takeAmount);

        }

    }
}
