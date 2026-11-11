using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SiteCDL.Domain.Entities
{
  

    public class PostCategoria
    {
        [Key]
        public int Codigo { get; set; }
        public string? Nome { get; set; }     
        public string? Controller { get; set; }     
        public string? Route { get; set; }     
        public ICollection<Post>? Posts { get; set; }
    }
 

}


