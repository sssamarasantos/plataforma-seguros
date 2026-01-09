using Microsoft.OpenApi.Models;
using SeguroProposta.Application.Interfaces;
using SeguroProposta.Application.Services;
using SeguroProposta.Domain.Interfaces;
using SeguroProposta.Infraestructure.AWS;
using SeguroProposta.Infraestructure.Contexts;
using SeguroProposta.Infraestructure.Interfaces;
using SeguroProposta.Infraestructure.Repositories;
using System.Data.Common;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddSingleton(s => new DbConnectionStringBuilder
{
    ConnectionString = SecretsManager.ObterAsync("API-PROPOSTA-CONEXAO").Result
});

builder.Services.AddScoped<IDbContextFactory, SqlServerContext>();

builder.Services.AddScoped<IPropostaRepository, PropostaRepository>();

builder.Services.AddScoped<IPropostaService, PropostaService>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
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

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
