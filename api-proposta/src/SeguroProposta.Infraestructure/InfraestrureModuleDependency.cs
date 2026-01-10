using Microsoft.Extensions.DependencyInjection;
using SeguroProposta.Domain.Interfaces;
using SeguroProposta.Infraestructure.AWS;
using SeguroProposta.Infraestructure.Contexts;
using SeguroProposta.Infraestructure.Interfaces;
using SeguroProposta.Infraestructure.Repositories;
using System.Data.Common;

namespace SeguroProposta.Infraestructure
{
    public static class InfraestrureModuleDependency
    {
        public static void AddInfraestrureModuleDependency(this IServiceCollection services)
        {
            services.AddSingleton(s => new DbConnectionStringBuilder
            {
                ConnectionString = SecretsManager.ObterAsync("API-PROPOSTA-CONEXAO").Result
            });

            services.AddScoped<IDbContextFactory, SqlServerContext>();
            services.AddScoped<IPropostaRepository, PropostaRepository>();
        }
    }
}
