using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SiteCDL.Domain.Entities
{
   
    public class Paginas
    {
        [Key]
        public int Codigo { get; set; }
        public string? Titulo { get; set; }
        public string? Conteudo { get; set; }
        public string? Controller { get; set; }
        public bool Status { get; set; }
        public int IdUsuarioPortalCreate { get; set; }
        public int IdUsuarioPortalUpdate { get; set; }
        public int IdUsuarioPortalDelete { get; set; }
        [NotMapped]
        public string? Token { get; set; }

        [Column(TypeName = "text")]
        public string? Seo { get; set; }

        [Column(TypeName = "text")]
        public string? Slug { get; set; }

    }
 
}


