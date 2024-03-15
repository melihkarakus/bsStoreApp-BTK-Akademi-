using bsStoreApp.Extensions;
using bsStoreApp.Repositories.EFCore;
using bsStoreApp.Services.Contract;
using Microsoft.EntityFrameworkCore;
using NLog;

var builder = WebApplication.CreateBuilder(args);
//NLog nlog.config tan�mlatt�rd�k.
LogManager.LoadConfiguration(string.Concat(Directory.GetCurrentDirectory(), "/nlog.config"));

// Add services to the container.
//assembly reference vermezsek presentation controller �al��maz.
builder.Services.AddControllers().AddApplicationPart(typeof(bsStoreApp.Presentation.AssemblyReference).Assembly);
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Extensions klas�r� tan�mlama yap�ld� buraya ge�ildi.
builder.Services.ConfigureSqlContext(builder.Configuration);

//Repository birbirlerine anlatmak i�in yaz�lan kod Extensions klas�r�ne bak.
builder.Services.ConfigureRepositoryManager();
builder.Services.ConfigureServiceManager();
builder.Services.ConfigureLoggerService();



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

app.UseAuthorization();

app.MapControllers();

app.Run();
