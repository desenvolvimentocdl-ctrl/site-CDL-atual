
using Microsoft.EntityFrameworkCore;
using SiteCDL.Application.Interfaces;
using SiteCDL.Application.Mappings;
using SiteCDL.Application.Services;
using SiteCDL.Domain.Interfaces.Repositories;
using SiteCDL.Infrastructure.Data;
using SiteCDL.Infrastructure.Repository;
using SiteCDL.Utilities;
using System.Globalization;
using System;
using System.Text.Json;
using TimeZoneConverter;
using System.Net;

var builder = WebApplication.CreateBuilder(args);

// Configurar o timezone padrão
TimeZoneInfo timeZone = TZConvert.GetTimeZoneInfo("America/Sao_Paulo"); // Substitua pelo timezone desejado
DateTime now = TimeZoneInfo.ConvertTime(DateTime.UtcNow, timeZone);
CultureInfo.DefaultThreadCurrentCulture = new CultureInfo("pt-BR");
CultureInfo.DefaultThreadCurrentUICulture = new CultureInfo("pt-BR");

builder.Services.AddScoped<IPageService, PageService>();
builder.Services.AddScoped<IPageRepository, PageRepository>();

builder.Services.AddScoped<IHomeService,HomeService>();
builder.Services.AddScoped<IHomeRepository,HomeRepository>();

builder.Services.AddScoped<IPostService, PostService>();
builder.Services.AddScoped<IPostRepository, PostRepository>();

builder.Services.AddScoped<PaginacaoRepository>();

builder.Services.AddAutoMapper(typeof(MappingProfile));



// Obtém o IP da máquina
string localIp = GetLocalIpAddress();
var env = builder.Environment.EnvironmentName;
builder.Configuration
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
    .AddJsonFile($"appsettings.{env}.json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariables();

var connectionStrings = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<DataContext>(options => options.UseMySql(connectionStrings, ServerVersion.AutoDetect(connectionStrings)));
builder.Services.AddDbContext<BannerDbContext>(options => options.UseMySql(connectionStrings, ServerVersion.AutoDetect(connectionStrings)));



builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.DictionaryKeyPolicy = null; // Para manter as chaves como estão

        // Ignora a sensibilidade a maiúsculas e minúsculas durante a desserialização
        options.JsonSerializerOptions.PropertyNamingPolicy = null; // Desabilita a política de nomenclatura padrão
    });


var app = builder.Build();
//var allowedIp = "127.0.0.1:5151";

//// Middleware para verificar o IP de origem
//app.Use(async (context, next) =>
//{
//    // Obtém o IP remoto da solicitação
//    var remoteIp = context.Connection.RemoteIpAddress?.ToString();

//    if (remoteIp != allowedIp)
//    {
//        // Se o IP não for permitido, retorna status 403 - Forbidden
//        context.Response.StatusCode = StatusCodes.Status403Forbidden;
//        await context.Response.WriteAsync("Acesso negado. Seu IP não está autorizado a acessar esta API.");
//        return;
//    }

//    // Se o IP for permitido, continua para o próximo middleware
//    await next();
//});
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
 
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();


app.Run();


// Função para obter o IP da máquina
static string GetLocalIpAddress()
{
    var host = Dns.GetHostEntry(Dns.GetHostName());
    foreach (var ip in host.AddressList)
    {
        if (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
        {
            return ip.ToString();
        }
    }
    return "127.0.0.1"; // Se não encontrar, assume localhost
}
