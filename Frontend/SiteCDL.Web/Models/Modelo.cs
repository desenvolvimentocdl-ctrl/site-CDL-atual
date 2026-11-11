using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace SiteCDL.Models
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

    public class Home
    {
      public List<Post>? Noticias { get; set; }
      public List<Post>? Cursos { get; set; }
      public List<Post>? Eventos { get; set; }
      public List<Post>? Relacionados { get; set; }
         public List<Banner>? Banners { get; set; }
    }

    public class DetalhePostGeral
    {
        public Post? Noticia { get; set; }
        public List<Post>? PostsRelacionados { get; set; }
    }

    public class PostCategoria
    {
        [Key]
        public int Codigo { get; set; }
        public string? Nome { get; set; }     
        public string? Controller { get; set; }     
        public string? Route { get; set; }     
        public ICollection<Post>? Posts { get; set; }
    }

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

        [NotMapped]
        public IFormFile? UploadBannerDesktop { get; set; } // Arquivo do banner desktop
        [NotMapped]
        public IFormFile? UploadBannerMobile { get; set; }  // Arquivo do banner mobile
    }

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

    public class Search
    {
        [Key]
        public int Codigo { get; set; }
        public string? Conteudo { get; set; }
        public string? Controller { get; set; }
        public string? Titulo { get; set; }
        public string? Slug { get; set; }
        public string? Subtitulo { get; set; }
        public string? Imagem { get; set; } 
        public string? ImagemBase64 { get; set; } 
        public string? Seo { get; set; } 
        public DateTime? Data { get; set; } 
        public DateTime? DataInicio { get; set; } 
        public DateTime? DataFim { get; set; } 
        public string? link { get; set; } 
        public string? linkfotos { get; set; }  
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


    public class Lead
    {
        [Key]
        public int Codigo { get; set; }
        public string? Json { get; set; }
        public DateTime? DataCadastro { get; set; }
        public bool Integrado { get; set; }
    }
}


