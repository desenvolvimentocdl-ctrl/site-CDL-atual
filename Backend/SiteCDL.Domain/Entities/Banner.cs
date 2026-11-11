using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SiteCDL.Domain.Entities
{
    

    public class Banner
    {
        [Key]
         public int Codigo { get; set; }
        public int IdUsuarioPortal { get; set; }
        public int IdUsuarioPortalUpdate { get; set; }
        public int IdUsuarioPortalDelete { get; set; }
        public string? Nome { get; set; }
        public string? CaminhoDesktop { get;set; } 
        public string? CaminhoMobile { get;set; } 
        public string? Link { get;set; }
        public bool Status { get;set; }
        public DateTime? DataInicio { get; set; }
        public DateTime? DataFim { get; set; }
        [NotMapped]
        public string? BannerFile { get; set; }
        [NotMapped]
        public string? Token { get; set; }
    }

   

}


