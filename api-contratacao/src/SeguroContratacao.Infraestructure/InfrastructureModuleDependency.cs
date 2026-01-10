using Microsoft.Extensions.DependencyInjection;
using SeguroContratacao.Domain.Interfaces;
using SeguroContratacao.Infrastructure.AWS;
using SeguroContratacao.Infrastructure.Contexts;
using SeguroContratacao.Infrastructure.Interfaces;
using SeguroContratacao.Infrastructure.Repositories;
using SeguroContratacao.Infrastructure.Rest;
using System.Data.Common;

namespace SeguroContratacao.Infrastructure
{
    public static class InfrastructureModuleDependency
    {
        public static async Task AddInfrastructureModuleDependency(this IServiceCollection services)
        {
            string connectionString;

            using (var secretsManager = new SecretsManager())
            {
                connectionString = await secretsManager.ObterAsync("API-CONTRATACAO-CONEXAO");
            }

            services.AddSingleton(new DbConnectionStringBuilder
            {
                ConnectionString = connectionString
            });

            services.AddScoped<IDbContextFactory, SqlServerContext>();
            services.AddScoped<IContratacaoRepository, ContratacaoRepository>();
            services.AddScoped<IPropostaApi, PropostaApi>();

            services.AddHttpClient("api-proposta", client =>
            {
                client.BaseAddress = new Uri("https://localhost:7269");
            });
        }
    }
}
