using EcotrackPlatform.API.Monitoringandcontrol.Application.Internal.CommandServices;
using EcotrackPlatform.API.Monitoringandcontrol.Application.Internal.QueryServices;
using EcotrackPlatform.API.Monitoringandcontrol.Domain.Repositories;
using EcotrackPlatform.API.Monitoringandcontrol.Infraestructure.Persistence.EFC.Respositories;
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
});

// Add Database Connection
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
if (connectionString is null)
    throw new InvalidOperationException("Connection string is null");

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
app.UseHttpsRedirection();

// Map controllers
app.MapControllers();

app.Run();


