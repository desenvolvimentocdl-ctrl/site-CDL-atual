using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SiteCDL.Domain.Entities
{
     

    public class DetalhePostGeral
    {
        public Post? Noticia { get; set; }
        public List<Post>? PostsRelacionados { get; set; }
    }

    
}


