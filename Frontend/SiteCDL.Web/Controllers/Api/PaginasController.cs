using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using SiteCDL.Models;
using SiteCDL.Services.Api;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SiteCDL.Controllers.Api
{
    [Route("portal/[controller]")]
    [ApiController]
    public class PaginasController : ControllerBase
    {

        private PaginasService _service;
        public PaginasController(PaginasService service)
        {
            _service = service;
        }

        //// GET: api/<BannersController>
        [EnableCors("AllowSpecificOrigins")]
        [HttpGet("{token}")]
        public async Task<List<Paginas>> Get(string token)
        {

            if (token == "e6aae946-a3fc-4a25-b3c2-d46100d38498")
            {
                try
                {
                    return await _service.GetAllServiceAsync(token);

                }
                catch
                {
                    return new List<Paginas>();
                }
            }
            else
            {
                return new List<Paginas>();
            }

        }


        [EnableCors("AllowSpecificOrigins")]
        [HttpGet("GetListControllers/{token}")]
        public List<string> GetListControllers(string token)
        {

            if (token == "e6aae946-a3fc-4a25-b3c2-d46100d38498")
            {
                try
                {
                    return _service.ListControllersService(token);
                }
                catch
                {
                    return new List<string>();
                }
            }
            else
            {
                return new List<string>();
            }


        }

        //// GET api/<BannersController>/5
        [EnableCors("AllowSpecificOrigins")]
        [HttpGet("{id}/{token}")]
        public async Task<Paginas> Get(int id, string token)
        {
            if (token == "e6aae946-a3fc-4a25-b3c2-d46100d38498")
            {

                try
                {
                    return await _service.GetByIdServiceAsync(id, token);

                }
                catch
                {
                    return new Paginas();
                }
            }
            else
            {
                return new Paginas();
            }
        }

        //// POST api/<BannersController>
        [EnableCors("AllowSpecificOrigins")]
        [HttpPost]
        public async Task<HttpResponseMessage> Post([FromBody] Paginas model)
        {

            try
            {

                if (model.Token == "e6aae946-a3fc-4a25-b3c2-d46100d38498")
                {

                    if (await _service.CreateServiceAsync(model, model.Token) != 0)
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
        public async Task<HttpResponseMessage> Put([FromBody] Paginas model)
        {
            try
            {

                if (model.Token == "e6aae946-a3fc-4a25-b3c2-d46100d38498")
                {
                    if (await _service.UpdateServiceAsync(model, model.Token) != 0)
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
        public async Task<HttpResponseMessage> Delete([FromBody] Paginas model)
        {
            try
            {

                if (model.Token == "e6aae946-a3fc-4a25-b3c2-d46100d38498")
                {

                    if (await _service.DeleteServiceAsync(model, model.Token) != 0)
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
