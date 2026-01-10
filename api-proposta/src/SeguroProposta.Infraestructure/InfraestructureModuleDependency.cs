using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using SeguroProposta.Domain.Interfaces;
using SeguroProposta.Infraestructure.AWS;
using SeguroProposta.Infraestructure.Contexts;
using SeguroProposta.Infraestructure.Interfaces;
using SeguroProposta.Infraestructure.Repositories;
using System.Data.Common;

namespace SeguroProposta.Infraestructure
{
    public static class InfraestructureModuleDependency
    {
        public static void AddInfraestrureModuleDependency(this IServiceCollection services)
        {
            services.AddSingleton(s =>
            {
                var configuration = s.GetRequiredService<IConfiguration>();
                
                string connectionString;
                
                // Tenta pegar do Secrets Manager, se falhar usa appsettings
                if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development")
                {
                    connectionString = configuration.GetConnectionString("DefaultConnection")
                        ?? throw new InvalidOperationException("Connection string não configurada.");
                }
                else
                {
                    connectionString = SecretsManager.Instance
                        .ObterAsync("API-PROPOSTA-CONEXAO")
                        .GetAwaiter()
                        .GetResult();
                }

                return new DbConnectionStringBuilder { ConnectionString = connectionString };
            });

            services.AddScoped<IDbContextFactory, SqlServerContext>();
            services.AddScoped<IPropostaRepository, PropostaRepository>();
        }
    }
}