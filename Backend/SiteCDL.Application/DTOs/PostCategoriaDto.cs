using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SiteCDL.Domain.Entities
{
  

    public class PostCategoriaDto
    {
         public int Codigo { get; set; }
        public string? Nome { get; set; }     
        public string? Controller { get; set; }     
        public ICollection<PostDto>? Posts { get; set; }
    }
 

}


