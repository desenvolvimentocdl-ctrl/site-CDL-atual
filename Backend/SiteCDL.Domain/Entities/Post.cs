using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SiteCDL.Domain.Entities
{
    public class Post
    {
        [Key]
        public int Codigo { get; set; }
        public string? Conteudo { get; set; }
        public string? Titulo { get; set; }
        public string? Slug { get; set; }
        public string? Subtitulo { get; set; }
        public string? Imagem { get; set; } 
        public string? ImagemBase64 { get; set; } 
        public DateTime? Data { get; set; } 
        public DateTime? DataInicio { get; set; } 
        public DateTime? DataFim { get; set; } 
        public string? Link { get; set; } 
        public string? LinkFotos { get; set; }  
        public int IdUsuarioPortalCreate { get; set; }
        public int IdUsuarioPortalUpdate { get; set; }
        public int IdUsuarioPortalDelete { get; set; }
        public bool Status { get;set; }
        
        [NotMapped]
        public string? Token { get; set; }
        [NotMapped]
        public int TotalPages { get; set; }
        [NotMapped]
        public int PageNumber { get; set; }
        [NotMapped]
        public int Previous { get; set; }
        [NotMapped]
        public int Next { get; set; }
        [NotMapped]
        public int QuantidadeLinhas { get; set; }
        [NotMapped]
        public int PageSize { get; set; }
        
        public int PostCategoriaCodigo { get; set; }
        public PostCategoria? PostCategoria { get; set; }
    }

    

}


