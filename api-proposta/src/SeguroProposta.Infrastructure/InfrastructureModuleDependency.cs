using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SeguroProposta.Domain.Interfaces;
using SeguroProposta.Infrastructure.AWS;
using SeguroProposta.Infrastructure.Contexts;
using SeguroProposta.Infrastructure.Interfaces;
using SeguroProposta.Infrastructure.Repositories;
using System.Data.Common;

namespace SeguroProposta.Infrastructure
{
    public static class InfrastructureModuleDependency
    {
        public static void AddInfrastructureModuleDependency(this IServiceCollection services, IConfiguration configuration)
        {
            ConfigurarConexaoBancoDeDados(services, configuration); 

            services.AddScoped<IDbContextFactory, SqlServerContext>();
            services.AddScoped<IPropostaRepository, PropostaRepository>();
        }

        private static void ConfigurarConexaoBancoDeDados(IServiceCollection services, IConfiguration configuration)
        {
            string? connectionString = null;

            try
            {
                using (var secretsManager = new SecretsManager())
                {
                    connectionString = secretsManager.ObterAsync("API-PROPOSTA-CONEXAO").GetAwaiter().GetResult();
                }
            }
            catch (Exception)
            {
                connectionString = null;
            }

            if (string.IsNullOrEmpty(connectionString))
            {
                connectionString = configuration.GetConnectionString("DefaultConnection");

                if (string.IsNullOrEmpty(connectionString))
                {
                    throw new InvalidOperationException(
                        "Connection string não encontrada. Verifique se o AWS Secrets Manager está acessível, " +
                        "ou se 'ConnectionStrings:DefaultConnection' está definida no appsettings.json");
                }
            }

            services.AddSingleton(s => new DbConnectionStringBuilder
            {
                ConnectionString = connectionString
            });
        }
    }
}