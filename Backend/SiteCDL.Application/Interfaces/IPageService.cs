using SiteCDL.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteCDL.Application.Interfaces
{
    public interface IPageService
    {

        ValueTask<Paginas> GetPageAsync(string slug);
    }
}
