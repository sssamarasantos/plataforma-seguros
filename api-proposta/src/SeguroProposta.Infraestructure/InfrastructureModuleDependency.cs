using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using SeguroProposta.Domain.Interfaces;
using System.Data.Common;
using SeguroProposta.Infrastructure.Contexts;
using SeguroProposta.Infrastructure.Repositories;
using SeguroProposta.Infrastructure.AWS;
using SeguroProposta.Infrastructure.Interfaces;

namespace SeguroProposta.Infrastructure
{
    public static class InfrastructureModuleDependency
    {
        public static void AddInfraestrureModuleDependency(this IServiceCollection services)
        {
            services.AddSingleton(s => new DbConnectionStringBuilder
            {
                ConnectionString = SecretsManager.Instance
                    .ObterAsync("API-PROPOSTA-CONEXAO")
                    .GetAwaiter()
                    .GetResult()
            });

            services.AddScoped<IDbContextFactory, SqlServerContext>();
            services.AddScoped<IPropostaRepository, PropostaRepository>();
        }
    }
}