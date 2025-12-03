using EcotrackPlatform.API.Iam.Application.Internal.CommandServices;
using EcotrackPlatform.API.Iam.Domain.Repositories;
using EcotrackPlatform.API.Iam.Infrastructure.Repositories;
using EcotrackPlatform.API.Monitoringandcontrol.Application.Internal.CommandServices;
using EcotrackPlatform.API.Monitoringandcontrol.Application.Internal.QueryServices;
using EcotrackPlatform.API.Monitoringandcontrol.Domain.Repositories;
using EcotrackPlatform.API.Monitoringandcontrol.Infraestructure.Persistence.EFC.Respositories;
using EcotrackPlatform.API.Profile.Application.Internal.CommandServices;
using EcotrackPlatform.API.Profile.Application.Internal.QueryServices;
using EcotrackPlatform.API.Profile.Domain.Repositories;
using EcotrackPlatform.API.Profile.Infrastructure.Repositories;
using EcotrackPlatform.API.Shared.Domain.Repositories;
using EcotrackPlatform.API.Shared.Infrastructure.Interfaces.ASP.Configuration;
using EcotrackPlatform.API.Shared.Infrastructure.Persistence.EFC.Configuration;
using EcotrackPlatform.API.Shared.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Add Configuration for Routing
builder.Services.AddRouting(options => options.LowercaseUrls = true);
builder.Services.AddControllers(options =>
{
    options.Conventions.Add(new KebabCaseRouteNamingConvention());
    options.RespectBrowserAcceptHeader = true;
})
.AddXmlSerializerFormatters()
.ConfigureApiBehaviorOptions(options =>
{
    options.SuppressConsumesConstraintForFormFileParameters = true;
});

// Add Database Connection

var host = Environment.GetEnvironmentVariable("ECOTRACK_DB_HOST");
var port = Environment.GetEnvironmentVariable("ECOTRACK_DB_PORT");
var db_ecotrack = Environment.GetEnvironmentVariable("ECOTRACK_DB_NAME");
var user = Environment.GetEnvironmentVariable("ECOTRACK_DB_USER");
var pass = Environment.GetEnvironmentVariable("ECOTRACK_DB_PASSWORD");

var connectionString =
    $"Server={host};Port={port};Database={db_ecotrack};User Id={user};Password={pass};SslMode=Preferred;AllowPublicKeyRetrieval=True;Pooling=True;";

// Add CORS Policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllPolicy", policy =>
        policy.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader());
});

// Configure Database Context and Logging Levels
builder.Services.AddDbContext<AppDbContext>(options =>
{
    if (builder.Environment.IsDevelopment())
    {
        options.UseMySQL(connectionString)
            .LogTo(Console.WriteLine, LogLevel.Information)
            .EnableDetailedErrors()
            .EnableSensitiveDataLogging();
    }
    else if (builder.Environment.IsProduction())
    {
        options.UseMySQL(connectionString)
            .LogTo(Console.WriteLine, LogLevel.Error)
            .EnableDetailedErrors();
    }
});

// Configure Swagger / OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.EnableAnnotations();
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Agromind Ecotrack Platform API",
        Version = "v1",
        Description = "Backend RESTful API for Agromind Ecotrack"
    });
});

//Dependency Injection

//Shared Bounded Context
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

//Monitoring And Control Bounded Context
builder.Services.AddScoped<ITaskRepository, TaskRepository>();
builder.Services.AddScoped<IChecklistRepository, ChecklistRepository>();
builder.Services.AddScoped<ILogbookRepository,  LogbookRepository>();
builder.Services.AddScoped<CreateTaskCommandService>();
builder.Services.AddScoped<CreateChecklistCommandService>();
builder.Services.AddScoped<UpdateTaskStatusCommandService>();
builder.Services.AddScoped<GetTasksQueryService>();
builder.Services.AddScoped<GetChecklistByTaskIdQueryService>();
builder.Services.AddScoped<CreateLogbookCommandService>();
builder.Services.AddScoped<GetLogbookQueryService>();

//Report Bounded Context
builder.Services.AddScoped<EcotrackPlatform.API.Report.Application.Internal.QueryServices.ReportQueryService>();
builder.Services.AddScoped<EcotrackPlatform.API.Report.Infrastructure.Services.PdfReportGeneratorService>();

builder.Services.AddScoped<EcotrackPlatform.API.Organization.Domain.Repositories.IOrganizationRepository, 
    EcotrackPlatform.API.Organization.Infrastructure.Repositories.OrganizationRepository>();
builder.Services.AddScoped<EcotrackPlatform.API.Organization.Domain.Repositories.ICropRepository, 
    EcotrackPlatform.API.Organization.Infrastructure.Repositories.CropRepository>();

// Organization Context - Services
builder.Services.AddScoped<EcotrackPlatform.API.Organization.Aplication.Services.IOrganizationCommandService, 
    EcotrackPlatform.API.Organization.Aplication.Internal.CommandServices.OrganizationCommandService>();
builder.Services.AddScoped<EcotrackPlatform.API.Organization.Aplication.Services.IOrganizationQueryService, 
    EcotrackPlatform.API.Organization.Aplication.Internal.QueryServices.OrganizationQueryService>();
builder.Services.AddScoped<EcotrackPlatform.API.Organization.Aplication.Services.ICropCommandService, 
    EcotrackPlatform.API.Organization.Aplication.Internal.CommandServices.CropCommandService>();
builder.Services.AddScoped<EcotrackPlatform.API.Organization.Aplication.Services.ICropQueryService, 
    EcotrackPlatform.API.Organization.Aplication.Internal.QueryServices.CropQueryService>();

// IAM Bounded Context
builder.Services.AddScoped<AuthCommandService>();
builder.Services.AddScoped<IAuthSessionRepository, AuthSessionRepository>();
builder.Services.AddScoped<IProfileRepository, ProfileRepository>();  // Si aún no está registrado


builder.Services.AddScoped<ProfileQueryService>();
builder.Services.AddScoped<ProfileCommandService>();
builder.Services.AddScoped<IProfileSettingsRepository, ProfileSettingsRepository>(); 
builder.Services.AddScoped<SettingsQueryService>();  // Agrega esta línea
builder.Services.AddScoped<SettingsCommandService>();  // Registra el servicio SettingsCommandService


var app = builder.Build();

// Verify Database Objects are Created
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<AppDbContext>();
    context.Database.EnsureCreated();
}

// Enable Swagger
app.UseSwagger();
app.UseSwaggerUI();

// Enable CORS
app.UseCors("AllowAllPolicy");

// HTTPS Redirection
if (app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}
// Map controllers
app.MapControllers();

app.Run();

