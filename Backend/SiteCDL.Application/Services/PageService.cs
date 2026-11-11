using SiteCDL.Application.Interfaces;
using SiteCDL.Domain.Entities;
using SiteCDL.Domain.Interfaces.Repositories;

namespace SiteCDL.Application.Services
{
    public class PageService :IPageService
    {

        private readonly IPageRepository _repository;

        public PageService(IPageRepository repository)
        {
            _repository = repository;
        }

        public async ValueTask<Paginas> GetPageAsync(string slug)
        {
            return await _repository.GetPageAsync(slug);
        }



    }
}
