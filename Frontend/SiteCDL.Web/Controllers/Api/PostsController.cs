using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using SiteCDL.Models;
using SiteCDL.Services.Api;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SiteCDL.Controllers.Api
{
    [Route("portal/[controller]")]
    [ApiController]
    public class PostsController : ControllerBase
    {

        private PostService _postService;
        public PostsController(PostService postService)
        {
            _postService = postService;
        }

        //// GET: api/<BannersController>
        [EnableCors("AllowSpecificOrigins")]
        [HttpGet("{token}")]
        public async Task<List<object>> Get(string token)
        {

            if (token == "e6aae946-a3fc-4a25-b3c2-d46100d38498")
            {
                try
                {
                    return await _postService.GetAllPostsServiceAsync(token);

                }
                catch
                {
                    return new List<object>();
                }
            }
            else
            {
                return new List<object>();
            }

        }


        [EnableCors("AllowSpecificOrigins")]
        [HttpGet("GetCategories/{token}")]
        public async Task<List<PostCategoria>> GetCategories(string token)
        {

            if (token == "e6aae946-a3fc-4a25-b3c2-d46100d38498")
            {
                try
                {
                    return await _postService.GetAllPostCategoriaServiceAsync(token);
                }
                catch
                {
                    return new List<PostCategoria>();
                }
            }
            else
            {
                return new List<PostCategoria>();
            }


        }

        //// GET api/<BannersController>/5
        [EnableCors("AllowSpecificOrigins")]
        [HttpGet("{id}/{token}")]
        public async Task<object> Get(int id, string token)
        {
            if (token == "e6aae946-a3fc-4a25-b3c2-d46100d38498")
            {

                try
                {
                    return await _postService.GetByIdPostsServiceAsync(id, token);

                }
                catch
                {
                    return new object();
                }
            }
            else
            {
                return new object();
            }
        }

        //// POST api/<BannersController>
        [EnableCors("AllowSpecificOrigins")]
        [HttpPost]
        public async Task<HttpResponseMessage> Post([FromForm] Post post)
        {
            try
            {
                if (post.Token == "e6aae946-a3fc-4a25-b3c2-d46100d38498")
                {
                    // Diretório de upload
                    var uploadPath = Path.Combine(Directory.GetCurrentDirectory(), $"wwwroot/wp-content/uploads/posts");
                    if (!Directory.Exists(uploadPath))
                    {
                        Directory.CreateDirectory(uploadPath);
                    }

                    // Salvar arquivos e gerar caminhos
                    var files = Request.Form.Files;
                    string postFilePath = string.Empty;
                    string postFileName = string.Empty;

                    foreach (var file in files)
                    {
                        postFileName = $"{Guid.NewGuid()}_{file.FileName}";
                        postFilePath = Path.Combine(uploadPath, postFileName);

                        string fileNameWebP = $"{Guid.NewGuid()}_{file.FileName}.webp";
                        string uploadPathWebP = Path.Combine(uploadPath, fileNameWebP);


                        using (var stream = new FileStream(postFilePath, FileMode.Create))
                        {
                            await file.CopyToAsync(stream);
                        }

                        Utilities.ConvertImage.ConvertToWebP(postFilePath, uploadPathWebP);

                        post.Imagem = "posts/" + fileNameWebP;

                       

                    }



                    if (await _postService.CreatePostServiceAsync(post, post.Token) > 0)
                        return Utilities.Response.Api("OK");
                    else
                        return Utilities.Response.Api("BadRequest");
                }
                else
                {
                    return Utilities.Response.Api("BadRequest");
                }
            }
            catch
            {
                return Utilities.Response.Api("BadRequest");
            }
        }

        //// PUT api/<BannersController>/5
        [EnableCors("AllowSpecificOrigins")]
        [HttpPut]
        public async Task<HttpResponseMessage> Put([FromForm] Post post)
        {
            try
            {
                if (post.Token == "e6aae946-a3fc-4a25-b3c2-d46100d38498")
                {
                    // Diretório de upload
                    var uploadPath = Path.Combine(Directory.GetCurrentDirectory(), $"wwwroot/wp-content/uploads/posts");
                    if (!Directory.Exists(uploadPath))
                    {
                        Directory.CreateDirectory(uploadPath);
                    }

                    // Salvar arquivos e gerar caminhos
                    var files = Request.Form.Files;
                    string postFilePath = string.Empty;
                    string postFileName = string.Empty;

                    if (files.Count > 0)
                    {
                        foreach (var file in files)
                        {
                            postFileName = $"{Guid.NewGuid()}_{file.FileName}";
                            postFilePath = Path.Combine(uploadPath, postFileName);

                            string fileNameWebP =  $"{Guid.NewGuid()}_{file.FileName}.webp";
                            string uploadPathWebP = Path.Combine(uploadPath, fileNameWebP);

                            using (var stream = new FileStream(postFilePath, FileMode.Create))
                            {
                                await file.CopyToAsync(stream);
                            }
                            Utilities.ConvertImage.ConvertToWebP(postFilePath, uploadPathWebP);

                            post.Imagem = "posts/" + fileNameWebP;
                        }
                    }

                    if (await _postService.UpdatePostsServiceAsync(post, post.Token) > 0)
                        return Utilities.Response.Api("OK");
                    else
                        return Utilities.Response.Api("BadRequest");
                }
                else
                {
                    return Utilities.Response.Api("BadRequest");
                }
            }
            catch
            {
                return Utilities.Response.Api("BadRequest");
            }
        }

        //// DELETE api/<BannersController>/5
        [EnableCors("AllowSpecificOrigins")]
        [HttpDelete]
        public async Task<HttpResponseMessage> Delete([FromBody] Post post)
        {
            try
            {

                if (post.Token == "e6aae946-a3fc-4a25-b3c2-d46100d38498")
                {

                    if (await _postService.DeletePostsServiceAsync(post, post.Token) != 0)
                        return Utilities.Response.Api("OK");
                    else
                        return Utilities.Response.Api("BadRequest");

                }
                else
                {
                    return Utilities.Response.Api("BadRequest");
                }

            }
            catch (Exception ex)
            {
                return Utilities.Response.Api("BadRequest");
            }
        }
    }
}
