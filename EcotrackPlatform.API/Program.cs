using System.Reflection;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.EntityFrameworkCore;
using EcotrackPlatform.API.Shared.Infrastructure.Interfaces.ASP.Configuration;
using EcotrackPlatform.API.Shared.Infrastructure.Persistence.EFC.Configuration;

var builder = WebApplication.CreateBuilder(args);

// --- Database (MySQL: Oracle provider) ---
var host = Environment.GetEnvironmentVariable("ECOTRACK_DB_HOST") ;
var port = Environment.GetEnvironmentVariable("ECOTRACK_DB_PORT") ;
var db_ecotrack = Environment.GetEnvironmentVariable("ECOTRACK_DB_NAME") ;
var user = Environment.GetEnvironmentVariable("ECOTRACK_DB_USER") ;
var pass = Environment.GetEnvironmentVariable("ECOTRACK_DB_PASSWORD") ;

var connectionString =
    $"Server={host};Port={port};Database={db_ecotrack};User Id={user};Password={pass};SslMode=Preferred;AllowPublicKeyRetrieval=True;Pooling=True;";


// OJO: con Oracle se usa UseMySQL (no ServerVersion)
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseMySQL(connectionString);
});

// --- Controllers & kebab-case ---
builder.Services.AddControllers(options =>
{
    options.Conventions.Add(new KebabCaseRouteNamingConvention());
    options.SuppressAsyncSuffixInActionNames = true;
});

// --- Swagger ---
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.EnableAnnotations();
    var xml = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xml);
    if (File.Exists(xmlPath)) c.IncludeXmlComments(xmlPath, includeControllerXmlComments: true);
});

// --- CORS Dev ---
builder.Services.AddCors(o => o.AddDefaultPolicy(p => p.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod()));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors();
app.UseAuthorization();
app.MapControllers();

// EnsureCreated solo en dev (según “estilo del profe”)
if (app.Environment.IsDevelopment())
{
    using var scope = app.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.EnsureCreated();
}

app.Run();