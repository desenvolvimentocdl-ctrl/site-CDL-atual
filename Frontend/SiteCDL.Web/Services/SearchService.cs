using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.Hosting;
using SiteCDL.Models;
using SiteCDL.Repository;
using SiteCDL.Utilities;

namespace SiteCDL.Services
{
    public class SearchService
    {

        private readonly PaginasRepository _repositoryPages;
        private readonly PostRepository _repositoryPosts;

        public SearchService(PaginasRepository repositoryPages, PostRepository repositoryPosts)
        {
            _repositoryPages = repositoryPages;
            _repositoryPosts = repositoryPosts;
        }

        public async ValueTask<List<Search>> GetSearchPostServiceAsync(string id)
        {


            var resultPostTitle = await _repositoryPosts.GetSearchPostsTitleRepositoryAsync(id);
            var resultPostContent = await _repositoryPosts.GetSearchPostsContentRepositoryAsync(id);
            var resultPageTitle = await _repositoryPages.GetSearchPagesTitleRepositoryAsync(id);
            var resultPageContent = await _repositoryPages.GetSearchPagesContentRepositoryAsync(id);

            List<Search> search = new List<Search>();


            SearchPostContentByTitle(search, resultPostTitle);
            SearchPostContentByContent(search, resultPostContent, resultPostTitle);
            SearchPageContentByTitle(search, resultPageTitle);
            SearchPageContentByContent(search, resultPageContent, resultPageTitle);

			return search
	        .OrderByDescending(s => s.Data)
	        .ToList();
		}




        private void SearchPostContentByTitle(List<Search> search, List<Post> resultPostTitle)
        {
            if (resultPostTitle != null && resultPostTitle.Count > 0)
            {
                foreach (var postTitle in resultPostTitle)
                {
                    search.Add(new Search
                    {
                        Codigo = postTitle.Codigo,
                        Imagem = postTitle.Imagem,
                        ImagemBase64 = postTitle.ImagemBase64,
                        Data = postTitle.Data,
                        Titulo = postTitle.Titulo,
                        Slug = postTitle.Slug,
                        Subtitulo = postTitle.Subtitulo,
                        PostCategoria = postTitle.PostCategoria

                    });
                }
            }
        }

        private void SearchPostContentByContent(List<Search> search, List<Post> resultPostContent, List<Post> resultPostTitle)
        {
            if (resultPostContent != null && resultPostContent.Count > 0)
            {
				var codigosJaAdicionados = resultPostTitle?.Select(p => p.Codigo).ToHashSet() ?? new HashSet<int>();

				foreach (var postContent in resultPostContent)
                {
                    if (!codigosJaAdicionados.Contains(postContent.Codigo))
                    {
                        search.Add(new Search
                        {
                            Codigo = postContent.Codigo,
                            Imagem = postContent.Imagem,
                            ImagemBase64 = postContent.ImagemBase64,
                            Data = postContent.Data,
                            Titulo = postContent.Titulo,
                            Slug = postContent.Slug,
                            Subtitulo = postContent.Subtitulo,
                            PostCategoria = postContent.PostCategoria

                        });
                    }
                }
            }
        }

        private void SearchPageContentByTitle(List<Search> search, List<Paginas> resultPageTitle)
        {
            if (resultPageTitle != null && resultPageTitle.Count > 0)
            {
                foreach (var pageTitle in resultPageTitle)
                {
                    search.Add(new Search
                    {
                        Codigo = pageTitle.Codigo,
                        Titulo = pageTitle.Titulo,
                        Controller = pageTitle.Controller,
                         Slug= pageTitle.Slug,
                         Seo = pageTitle.Seo

                    });
                }
            }
        }

        private void SearchPageContentByContent(List<Search> search, List<Paginas> resultPageContent, List<Paginas> resultPageTitle)
        {
            if (resultPageContent != null && resultPageContent.Count > 0 && resultPageTitle.Count == 0)
            {
                foreach (var pageContent in resultPageContent)
                {
                    search.Add(new Search
                    {
                        Codigo = pageContent.Codigo,
                        Titulo = pageContent.Titulo,
                        Controller = pageContent.Controller,
                        Slug = pageContent.Slug,
                        Seo = pageContent.Seo

                    });
                }
            }
        }
    }
}
