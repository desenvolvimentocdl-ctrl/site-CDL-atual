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
    public class HomeService : IHomeService
    {

        private readonly IHomeRepository _homeRepository;
 
        public HomeService(IHomeRepository homeRepository)
        {
            _homeRepository = homeRepository;
        }
        public async ValueTask<Home> GetHomeDataAsync()
        { 
            return await _homeRepository.GetHomeDataAsync();
        }
    }
}
