using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SiteCDL.Application.Interfaces;
using SiteCDL.Domain.Entities;
using SiteCDL.Domain.Interfaces.Repositories;
using SiteCDL.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteCDL.Infrastructure.Repository
{
    public class HomeRepository : IHomeRepository
    {

        private readonly DataContext _dataContext;
        private readonly BannerDbContext _bannerDbContext;
        private readonly IPostRepository _postRepository;

        public HomeRepository(DataContext dataContext, BannerDbContext bannerDbContext, IPostRepository postRepository, DataContext _dataContext)
        {
        
            _bannerDbContext = bannerDbContext;
            _postRepository = postRepository;   
            _dataContext= dataContext;
        }
        public async ValueTask<Home> GetHomeDataAsync()
        {
          

            Home modelo = new()
            {

                Noticias = await _postRepository.GetAllPostsAsync( false,1, 4),
                Cursos = await _postRepository.GetPostsByCategoryAsync(new List<int> { 7 }, false, 3),
                Eventos = await _postRepository.GetPostsByCategoryAsync(new List<int> { 8 }, false, 3)

            };



            modelo.Banners = await _bannerDbContext.Banners.Where(a => a.Status == true && a.DataInicio <= DateTime.Now.Date && (!a.DataFim.HasValue || a.DataFim > DateTime.Now.Date)).OrderByDescending(x =>x.Codigo).ToListAsync();

            return  modelo;
        }


    }
}
