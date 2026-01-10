using Microsoft.Extensions.DependencyInjection;
using SeguroContratacao.Domain.Interfaces;
using SeguroContratacao.Infraestructure.AWS;
using SeguroContratacao.Infraestructure.Contexts;
using SeguroContratacao.Infraestructure.Interfaces;
using SeguroContratacao.Infraestructure.Repositories;
using SeguroContratacao.Infraestructure.Rest;
using System.Data.Common;

namespace SeguroContratacao.Infraestructure
{
    public static class InfraestructureModuleDependency
    {
        public static void AddInfraestructureModuleDependency(this IServiceCollection services)
        {
            services.AddSingleton(s => new DbConnectionStringBuilder
            {
                ConnectionString = SecretsManager.ObterAsync("API-CONTRATACAO-CONEXAO").Result
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
