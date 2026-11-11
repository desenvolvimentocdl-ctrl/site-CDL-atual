using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using SiteCDL.Services.Api;
using System.Text.Json;
using SiteCDL.Models;

namespace SiteCDL.Controllers.Api
{
    [Route("kommo/[controller]")]
    [ApiController]
    public class LeadsController : ControllerBase
    {
        private const string TOKEN = "e6aae946-a3fc-4a25-b3c2-d46100d38498";

        private readonly LeadsService _leadsService;

        public LeadsController(LeadsService leadsService)
        {
            _leadsService = leadsService;
        }

        [EnableCors("AllowSpecificOrigins")]
        [HttpPost]
        public async Task<IActionResult> Post([FromQuery] string token)
        {
            if (token == TOKEN)
            {
                object conteudoFinal;

                if (Request.HasFormContentType)
                {
                    var objFinal = new Dictionary<string, object>();

                    foreach (var chave in Request.Form.Keys)
                    {
                        var valor = Request.Form[chave].ToString();

                        if (IsJson(valor))
                        {
                            try
                            {
                                var jsonObj = JsonSerializer.Deserialize<object>(valor);

                                objFinal[chave] = jsonObj!;
                            }
                            catch
                            {
                                objFinal[chave] = valor;
                            }
                        }
                        else
                        {
                            objFinal[chave] = valor;
                        }
                    }

                    conteudoFinal = objFinal;
                }
                else
                {
                    using var reader = new StreamReader(Request.Body);

                    string rawBody = await reader.ReadToEndAsync();

                    if (IsJson(rawBody))
                        conteudoFinal = JsonSerializer.Deserialize<object>(rawBody)!;
                    else
                        conteudoFinal = rawBody;
                }

                string jsonFinal = JsonSerializer.Serialize(conteudoFinal, new JsonSerializerOptions { WriteIndented = false });

                await _leadsService.CreateLeadAsync(token, jsonFinal);
            }

            return Ok("OK");
        }

        private bool IsJson(string input)
        {
            input = input.Trim();

            if ((input.StartsWith("{") && input.EndsWith("}")) || (input.StartsWith("[") && input.EndsWith("]")))
            {
                try
                {
                    JsonDocument.Parse(input);

                    return true;
                }
                catch 
                {
                    return false;                
                }
            }

            return false;
        }

        [EnableCors("AllowSpecificOrigins")]
        [HttpGet("GetLeads/{limit}/{token}")]

        public async Task<List<Lead>> Get(int limit, string token)
        {
            if (token == TOKEN)
            {
                try
                {
                    return  await _leadsService.GetLeadsAsync(limit, token);
                }
                catch
                {
                    return new List<Lead>();
                }
            }
            else
            {
                return new List<Lead>();
            }            
        }
    }
}

