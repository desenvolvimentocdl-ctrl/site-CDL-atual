using Microsoft.EntityFrameworkCore;
using SiteCDL.Models;
using SiteCDL.Repository.Api;
using System.Web;

namespace SiteCDL.Services.Api
{
    public class LeadsService
    {
        private const string TOKEN = "e6aae946-a3fc-4a25-b3c2-d46100d38498";

        private readonly LeadsRepository _leadsRepository;

        public LeadsService(LeadsRepository leadsRepository)
        {
            _leadsRepository = leadsRepository;
        }

        public async Task<int> CreateLeadAsync(string token, string json)
        {
            if (token == TOKEN)
            {
                Lead lead = new Lead();

                lead.Json = json;
                lead.DataCadastro = DateTime.Now;

                return await _leadsRepository.CreateLeadAsync(token, lead);
            }
            else
            {
                return 0;
            }
        }

        public async Task<List<Lead>> GetLeadsAsync(int limit, string token)
        {
            if (token == TOKEN)
            {
                try
                {
                    return await _leadsRepository.GetLeadsAsync(limit, token);
                }
                catch
                {
                    return new List<Lead>();
                }
            }
            else
            {
                return new List<Lead>();
            }
        }
    }
}