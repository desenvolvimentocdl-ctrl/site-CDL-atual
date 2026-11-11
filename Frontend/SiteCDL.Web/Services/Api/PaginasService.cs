using Microsoft.Extensions.Hosting;
using SiteCDL.Models;
using SiteCDL.Repository.Api;
using SiteCDL.Utilities;
using System.Web;

namespace SiteCDL.Services.Api
{
    public class PaginasService
    {

        private readonly PaginasRepository _repository;
        public PaginasService(PaginasRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<Paginas>> GetAllServiceAsync(string token)
        {
            if (token == "e6aae946-a3fc-4a25-b3c2-d46100d38498")
            {
                try
                {
                    return await _repository.GetAllRepositoryAsync(token);
                }
                catch
                {
                    return new List<Paginas>();
                }
            }
            else
            {
                return new List<Paginas>();
            }

        }

        public List<string> ListControllersService(string token)
        {
            if (token == "e6aae946-a3fc-4a25-b3c2-d46100d38498")
            {
                try
                {
                    return _repository.ListControllers(token);
                }
                catch
                {
                    return new List<string>();
                }
            }
            else
            {
                return new List<string>();
            }
        }


        public async Task<Paginas> GetByIdServiceAsync(int id, string token)
        {


            if (token == "e6aae946-a3fc-4a25-b3c2-d46100d38498")
            {
                try
                {
                    return await _repository.GetByIdRepositoryAsync(id, token);

                }
                catch
                {
                    return new Paginas();
                }
            }
            else
            {
                return new Paginas();
            }

        }

        public async Task<int> CreateServiceAsync(Paginas model, string token)
        {
            if (token == "e6aae946-a3fc-4a25-b3c2-d46100d38498")
            {
                try
                {
                    string slug = Slug.StringToSlug(model.Titulo);
                    if (!string.IsNullOrWhiteSpace(slug))
                    {
                        int qtde = await _repository.GetQuantityBySlugRepositoryAsync(slug, token);
                        if (qtde > 0)
                        {
                            slug = slug + "-" + qtde.ToString();
                        }
                    }

                    Paginas obj = new();

                    obj.Titulo = model.Titulo;
                    obj.Conteudo = HttpUtility.UrlDecode(model.Conteudo);
                    obj.Controller = model.Controller;
                    obj.Seo = model.Seo;
                    obj.Slug = slug;
                    obj.Status = true;
                    obj.IdUsuarioPortalCreate = model.IdUsuarioPortalCreate;

                    return await _repository.CreateRepositoryAsync(obj, token);
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

        public async Task<int> UpdateServiceAsync(Paginas model, string token)
        {
            if (token == "e6aae946-a3fc-4a25-b3c2-d46100d38498")
            {
                try
                {
                    var existingPost = await _repository.FindRepositoryAsync(model, token);

                    if (existingPost != null)
                    {
                        existingPost.Titulo = model.Titulo;
                        existingPost.Seo = model.Seo;
                        existingPost.Conteudo = HttpUtility.UrlDecode(model.Conteudo);
                        existingPost.Controller = model.Controller;

                        return await _repository.UpdateRepositoryAsync(existingPost, token);

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

        public async Task<int> DeleteServiceAsync(Paginas model, string token)
        {
            if (token == "e6aae946-a3fc-4a25-b3c2-d46100d38498")
            {
                try
                {
                    var existingPost = await _repository.FindRepositoryAsync(model, token);

                    if (existingPost != null)
                    {
                        existingPost.Status = false;
                        //existingPost.IdUsuarioPortalDelete = post.IdUsuarioPortalDelete;

                        return await _repository.UpdateRepositoryAsync(existingPost, token);

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
