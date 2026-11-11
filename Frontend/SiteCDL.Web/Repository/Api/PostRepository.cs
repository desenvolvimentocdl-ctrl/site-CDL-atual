using Microsoft.EntityFrameworkCore;
using SiteCDL.Data;
using SiteCDL.Models;
using System.Web;

namespace SiteCDL.Repository.Api
{
    public class PostRepository
    {

        private DataContext _context;

        public PostRepository(DataContext context)
        {
            _context = context;
        }


        public async Task<Post> FindPostsRepositoryAsync(Post post, string token)
        {
            if (token == "e6aae946-a3fc-4a25-b3c2-d46100d38498")
            {
                try
                {
                    return await _context.Post.FindAsync(post.Codigo);
                }
                catch
                {
                    return new Post();
                }
            }
            else
            {
                return new Post();
            }
        }


        public async Task<List<Post>> GetAllPostsRepositoryAsync(string token)
        {
            if (token == "e6aae946-a3fc-4a25-b3c2-d46100d38498")
            {
                try
                {
                    return await _context.Post.Include(r => r.PostCategoria).Where(a => a.Status == true).ToListAsync();
                }
                catch
                {
                    return new List<Post>();
                }
            }
            else
            {
                return new List<Post>();
            }

        }


        public async Task<Post> GetByIdPostsRepositoryAsync(int id, string token)
        {

            if (token == "e6aae946-a3fc-4a25-b3c2-d46100d38498")
            {
                try
                {
                    return await _context.Post.Include(r => r.PostCategoria).Where(a => a.Status == true && a.Codigo == id).SingleOrDefaultAsync();
                }
                catch
                {
                    return new Post();
                }
            }
            else
            {
                return new Post();
            }

        }


        public async Task<int> CreatePostRepositoryAsync(Post post, string token)
        {
            if (token == "e6aae946-a3fc-4a25-b3c2-d46100d38498")
            {
                try
                {
                    _context.Post.Add(post);
                    return await _context.SaveChangesAsync();
                }
                catch
                {
                    return 0;
                }
            }
            else
            {
                return 0;
            }

        }


        public async Task<int> UpdatePostsRepositoryAsync(Post post, string token)
        {

            if (token == "e6aae946-a3fc-4a25-b3c2-d46100d38498")
            {
                try
                {
                    _context.Post.Update(post);
                    return await _context.SaveChangesAsync();
                }
                catch
                {
                    return 0;
                }
            }
            else
            {
                return 0;
            }

        }


    }
}
