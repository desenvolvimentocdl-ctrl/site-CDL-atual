using System.Globalization;
using System.Net;
using System.Text;
using System.Text.Json;

namespace SiteCDL.Utilities
{
    public static class Response
    {
        public static HttpResponseMessage Api(string typeResponse)
        {

            try
            {
                if (typeResponse == "OK")
                {

                    var resposta = new HttpResponseMessage(HttpStatusCode.OK);

                    var dados = new
                    {
                        Mensagem = "Requisição bem-sucedida"
                    };
                    var json = JsonSerializer.Serialize(dados);

                    resposta.Content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
                    return resposta;
                }
                else
                {
                    var resposta = new HttpResponseMessage(HttpStatusCode.BadRequest);
                    var dados = new
                    {
                        Mensagem = "Erro: não permitido"
                    };
                    var json = JsonSerializer.Serialize(dados);

                    resposta.Content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
                    return resposta;
                }



            }
            catch (Exception ex)
            {
                var resposta = new HttpResponseMessage(HttpStatusCode.BadRequest);
                var dados = new
                {
                    Mensagem = "Erro: não permitido"
                };
                var json = JsonSerializer.Serialize(dados);

                resposta.Content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
                return resposta;
            }
        }
    }
}
