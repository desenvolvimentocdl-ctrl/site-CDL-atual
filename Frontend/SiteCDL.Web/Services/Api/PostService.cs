using Microsoft.EntityFrameworkCore;
using SiteCDL.Models;
using SiteCDL.Repository.Api;
using System.Web;

namespace SiteCDL.Services.Api
{
    public class PostService
    {
        private readonly PostRepository _postRepository;
        private readonly CategoryRepository _categoryRepository;

        public PostService(PostRepository postRepository, CategoryRepository categoryRepository)
        {
            _postRepository = postRepository;
            _categoryRepository = categoryRepository;
        }

        public async Task<List<object>> GetAllPostsServiceAsync(string token)
        {
            if (token == "e6aae946-a3fc-4a25-b3c2-d46100d38498")
            {
                try
                {
                    List<Post> posts = await _postRepository.GetAllPostsRepositoryAsync(token);
                    var formattedPosts = posts.Select(post => new
                    {
                        post.Codigo,
                        post.Conteudo,
                        post.Titulo,
                        post.Slug,
                        post.Subtitulo,
                        post.ImagemBase64,
                        post.Data,
                        post.DataInicio,
                        post.DataFim,
                        post.Link,
                        post.LinkFotos,
                        Categoria = post.PostCategoria.Nome
                    });
                    return formattedPosts.ToList<object>();
                }
                catch
                {
                    return new List<object>();
                }
            }
            else
            {
                return new List<object>();
            }

        }

        public async Task<List<PostCategoria>> GetAllPostCategoriaServiceAsync(string token)
        {
            if (token == "e6aae946-a3fc-4a25-b3c2-d46100d38498")
            {
                try
                {
                    return await _categoryRepository.GetAllPostCategoriaRepositoryAsync(token);
                }
                catch
                {
                    return new List<PostCategoria>();
                }
            }
            else
            {
                return new List<PostCategoria>();
            }
        }


        public async Task<object> GetByIdPostsServiceAsync(int id, string token)
        {


            if (token == "e6aae946-a3fc-4a25-b3c2-d46100d38498")
            {
                try
                {
                    Post post = await _postRepository.GetByIdPostsRepositoryAsync(id, token);

                    var formattedPosts = new
                    {
                        post.Codigo,
                        post.Conteudo,
                        post.Titulo,
                        post.Slug,
                        post.Subtitulo,
                        post.ImagemBase64,
                        post.Imagem,
                        post.Data,
                        post.DataInicio,
                        post.DataFim,
                        post.Link,
                        post.LinkFotos,
                        Categoria = post.PostCategoria.Codigo
                    };

                    return formattedPosts;

                }
                catch
                {
                    return new List<object>();
                }
            }
            else
            {
                return new List<object>();
            }

        }

        public async Task<int> CreatePostServiceAsync(Post post, string token)
        {
            if (token == "e6aae946-a3fc-4a25-b3c2-d46100d38498")
            {
                try
                {
                    Post obj = new();

                    obj.Titulo = post.Titulo;
                    obj.Subtitulo = post.Subtitulo;
                    obj.Data = post.Data;
                    obj.DataInicio = post.DataInicio;
                    obj.DataFim = post.DataFim;
                    obj.Link = post.Link;
                    obj.LinkFotos = post.LinkFotos;
                    obj.Conteudo = post.Conteudo;
                    obj.ImagemBase64 = post.ImagemBase64;
                    obj.IdUsuarioPortalCreate = post.IdUsuarioPortalCreate;
                    obj.PostCategoriaCodigo = post.PostCategoriaCodigo;
                    obj.Slug = post.Slug;
                    obj.Status = true;
                    obj.Imagem = post.Imagem;

                    return await _postRepository.CreatePostRepositoryAsync(obj, token);
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

        public async Task<int> UpdatePostsServiceAsync(Post post, string token)
        {
            if (token == "e6aae946-a3fc-4a25-b3c2-d46100d38498")
            {
                try
                {
                    var existingPost = await _postRepository.FindPostsRepositoryAsync(post, token);

                    if (existingPost != null)
                    {
                        existingPost.Titulo = post.Titulo;
                        existingPost.Subtitulo = post.Subtitulo;
                        existingPost.Data = post.Data;
                        existingPost.DataInicio = post.DataInicio;
                        existingPost.DataFim = post.DataFim;
                        existingPost.Link = post.Link;
                        existingPost.LinkFotos = post.LinkFotos;
                        existingPost.Conteudo = HttpUtility.UrlDecode(post.Conteudo);
                        existingPost.PostCategoriaCodigo = post.PostCategoriaCodigo;
                        existingPost.Slug = post.Slug;
                        existingPost.IdUsuarioPortalUpdate = post.IdUsuarioPortalUpdate;

                        if (!string.IsNullOrWhiteSpace(post.Imagem))
                        {
                            existingPost.Imagem = post.Imagem;
                        }

                        if (!string.IsNullOrWhiteSpace(post.ImagemBase64))
                        {
                            existingPost.ImagemBase64 = post.ImagemBase64;
                        }

                        return await _postRepository.UpdatePostsRepositoryAsync(existingPost, token);
                    }
                    else
                    {
                        return 0;
                    }
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

        public async Task<int> DeletePostsServiceAsync(Post post, string token)
        {
            if (token == "e6aae946-a3fc-4a25-b3c2-d46100d38498")
            {
                try
                {
                    var existingPost = await _postRepository.FindPostsRepositoryAsync(post, token);

                    if (existingPost != null)
                    {
                        existingPost.Status = false;
                        existingPost.IdUsuarioPortalDelete = post.IdUsuarioPortalDelete;

                        return await _postRepository.UpdatePostsRepositoryAsync(existingPost, token);
                    }
                    else
                    {
                        return 0;
                    }
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