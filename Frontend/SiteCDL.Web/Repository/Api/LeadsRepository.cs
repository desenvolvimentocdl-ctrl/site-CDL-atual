using Microsoft.EntityFrameworkCore;
using SiteCDL.Data;
using SiteCDL.Models;
using System.Web;

namespace SiteCDL.Repository.Api
{
    public class LeadsRepository
    {
        private const string TOKEN = "e6aae946-a3fc-4a25-b3c2-d46100d38498";

        private DataContext _context;

        public LeadsRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<int> CreateLeadAsync(string token, Lead lead)
        {
            if (token == TOKEN)
            {
                _context.Lead.Add(lead);

                return await _context.SaveChangesAsync();
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
                    return await _context.Lead.OrderByDescending(x => x.Codigo).Take(limit).ToListAsync();
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
