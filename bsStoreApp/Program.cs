using bsStoreApp.Extensions;
using bsStoreApp.Presentation.ActionFilters;
using bsStoreApp.Repositories.EFCore;
using bsStoreApp.Services.Contract;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NLog;

var builder = WebApplication.CreateBuilder(args);
//NLog nlog.config tan�mlatt�rd�k.
LogManager.LoadConfiguration(string.Concat(Directory.GetCurrentDirectory(), "/nlog.config"));

// Add services to the container.
//assembly reference vermezsek presentation controller �al��maz.
builder.Services.AddControllers(config =>
{
    //��erik Pazarl��� Konfigrasyon
    config.RespectBrowserAcceptHeader = true; //��erik pazarl���n� yapmam�z i�in gereken konfigrasyon
    config.ReturnHttpNotAcceptable = true; //Api g�nderilen iste�in bu i�erikle pazarlama yap�lmad���n� belirtir.
})
    .AddXmlDataContractSerializerFormatters() //XML Format�nda d�nmesi i�in
    .AddApplicationPart(typeof(bsStoreApp.Presentation.AssemblyReference).Assembly);



// Hata davran���n� devre d��� b�rakmak i�in
builder.Services.Configure<ApiBehaviorOptions>(opt =>
{
    opt.SuppressModelStateInvalidFilter = true;
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Extensions klas�r� tan�mlama yap�ld� buraya ge�ildi.
builder.Services.ConfigureSqlContext(builder.Configuration);

//Repository birbirlerine anlatmak i�in yaz�lan kod Extensions klas�r�ne bak.
builder.Services.ConfigureRepositoryManager();
builder.Services.ConfigureServiceManager();
builder.Services.ConfigureLoggerService();

//Automapper Konfigrasyonu
builder.Services.AddAutoMapper(typeof(Program));

//Action Filters Konfigrasyonu Extentions Klas�r�ne tan�mland�.
builder.Services.ConfigureActionFilters();

//Page Konfigrasyon i�lemi
builder.Services.ConfigureCors();

//DataShaper Konfigrasyon i�lemi
builder.Services.ConfigureDataShapper();

var app = builder.Build();

//ExceptionsMiddlewareExtensions s�n�f�na tan�mlanan configure burada bu �ekilde ge�melisin ��nk� mimari olu�tuktan sonra bu kod �al��acakt�r.
var logger = app.Services.GetRequiredService<ILoggerService>();
app.ConfigureExceptionHandler(logger);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//Productions i�in gerekli kod eklendi.
if (app.Environment.IsProduction())
{
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseCors("corsPolicy");//Policy Page Konfigrasyonun i�inde oras�da Api->Extensions

app.UseAuthorization();

app.MapControllers();

app.Run();
