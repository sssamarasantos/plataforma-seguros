using Microsoft.OpenApi.Models;
using SeguroProposta.Api.Middlewares;
using SeguroProposta.Application;
using SeguroProposta.Infrastructure;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddApplicationModuleDependency();
builder.Services.AddInfrastructureModuleDependency();

// Método auxiliar para configurar opções JSON compartilhadas
static void ConfigureJsonOptions(JsonSerializerOptions options)
{
    options.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
    options.WriteIndented = false;
    options.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
}

builder.Services.ConfigureHttpJsonOptions(options =>
{
    ConfigureJsonOptions(options.SerializerOptions);
});

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        ConfigureJsonOptions(options.JsonSerializerOptions);
    });
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "Proposta API",
        Description = "API para controle de propostas"
    });

    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
