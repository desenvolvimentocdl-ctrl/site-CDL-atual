using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SiteCDL.Domain.Entities
{
    
    public class Home
    {
      public List<Post>? Noticias { get; set; }
      public List<Post>? Cursos { get; set; }
      public List<Post>? Eventos { get; set; }
      public List<Post>? Relacionados { get; set; }
      public List<Banner>? Banners { get; set; }
    }

    

}


