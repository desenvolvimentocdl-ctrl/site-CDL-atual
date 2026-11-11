using SiteCDL.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteCDL.Domain.Interfaces.Repositories
{
    public interface IPageRepository
    {
        ValueTask<Paginas> GetPageAsync(string slug);

     }
}
