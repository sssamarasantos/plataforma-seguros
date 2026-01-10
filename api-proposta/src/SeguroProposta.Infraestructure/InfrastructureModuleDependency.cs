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
        public static void AddInfrastructureModuleDependency(this IServiceCollection services)
        {
            string connectionString;
            using (var secretsManager = new SecretsManager())
            {
                connectionString = secretsManager.ObterAsync("API-PROPOSTA-CONEXAO").GetAwaiter().GetResult();
            }

            services.AddSingleton(s => new DbConnectionStringBuilder
            {
                ConnectionString = connectionString
            });

            services.AddScoped<IDbContextFactory, SqlServerContext>();
            services.AddScoped<IPropostaRepository, PropostaRepository>();
        }
    }
}