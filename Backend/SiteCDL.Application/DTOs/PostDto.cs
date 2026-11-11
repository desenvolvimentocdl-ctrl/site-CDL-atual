using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SiteCDL.Domain.Entities
{
    public class PostDto
    {
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
        public string? link { get; set; } 
        public string? linkfotos { get; set; }  
        public int IdUsuarioPortalCreate { get; set; }
        public int IdUsuarioPortalUpdate { get; set; }
        public int IdUsuarioPortalDelete { get; set; }
        public bool Status { get;set; }
        
         public string? Token { get; set; }
         public int TotalPages { get; set; }
         public int PageNumber { get; set; }
         public int Previous { get; set; }
         public int Next { get; set; }
         public int QuantidadeLinhas { get; set; }
         public int PageSize { get; set; }
        
        public int PostCategoriaCodigo { get; set; }
        public PostCategoriaDto? PostCategoria { get; set; }
    }

    

}


