using Microsoft.EntityFrameworkCore;
using SiteCDL.Data;
using System.Globalization;
using System;
using TimeZoneConverter;
using System.Net;

var builder = WebApplication.CreateBuilder(args);

// Configurar o timezone padrão
TimeZoneInfo timeZone = TZConvert.GetTimeZoneInfo("America/Sao_Paulo"); // Substitua pelo timezone desejado
DateTime now = TimeZoneInfo.ConvertTime(DateTime.UtcNow, timeZone);
CultureInfo.DefaultThreadCurrentCulture = new CultureInfo("pt-BR");
CultureInfo.DefaultThreadCurrentUICulture = new CultureInfo("pt-BR");
 
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigins", builder =>
    {
        //builder.WithOrigins("http://localhost:59412")
        //builder.WithOrigins("https://portal.cdludi.org.br") 
        builder.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

builder.Services.AddHttpClient("ApiHttpClient", client =>
{

    //client.BaseAddress = new Uri("https://localhost:7110/api/");
    client.BaseAddress = new Uri("http://127.0.0.1:7110/api/");
    client.Timeout = TimeSpan.FromMinutes(10);  
});
 
 builder.Services.AddControllersWithViews().AddJsonOptions(options =>
{ 
    options.JsonSerializerOptions.PropertyNamingPolicy = null;  
});

 

builder.Services.AddScoped<SiteCDL.Services.Api.PostService>();
builder.Services.AddScoped<SiteCDL.Repository.Api.PostRepository>();


builder.Services.AddScoped<SiteCDL.Services.Api.PaginasService>();
builder.Services.AddScoped<SiteCDL.Repository.Api.PaginasRepository>();

builder.Services.AddScoped<SiteCDL.Services.Api.BannerService>();
builder.Services.AddScoped<SiteCDL.Repository.Api.BannerRepository>();

builder.Services.AddScoped<SiteCDL.Repository.Api.CategoryRepository>();

builder.Services.AddScoped<SiteCDL.Services.SearchService>();
builder.Services.AddScoped<SiteCDL.Repository.PaginasRepository>();
builder.Services.AddScoped<SiteCDL.Repository.PostRepository>();

builder.Services.AddScoped<SiteCDL.Services.Api.LeadsService>();
builder.Services.AddScoped<SiteCDL.Repository.Api.LeadsRepository>();

builder.Services.AddScoped<DataContext>();
builder.Services.AddScoped<BannerDbContext>();

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





var app = builder.Build();
app.UseExceptionHandler("/Home/Error");

if (!app.Environment.IsDevelopment())
{
     app.UseHsts();
}


app.UseHttpsRedirection();
app.UseStaticFiles();
//app.UseStaticFiles(new StaticFileOptions
//{
//    FileProvider = new PhysicalFileProvider(
//           Path.Combine(builder.Environment.ContentRootPath, @"")),
//    RequestPath = "/wwwroot"
//});

app.UseRouting(); 
 
app.UseCors("AllowSpecificOrigins");
app.UseAuthorization();

 
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");


app.MapControllerRoute(
    name: "Pages",
    pattern: "{slug}",
    defaults: new { controller = "Page", action = "Index" }
);

 

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
