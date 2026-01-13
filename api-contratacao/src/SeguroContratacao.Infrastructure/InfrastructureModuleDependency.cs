using Microsoft.Extensions.Configuration;
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
        public static void AddInfrastructureModuleDependency(this IServiceCollection services)
        {
            ConfigurarAWSServices(services, services.BuildServiceProvider().GetRequiredService<IConfiguration>());


            services.AddScoped<IDbContextFactory, SqlServerContext>();
            services.AddScoped<IContratacaoRepository, ContratacaoRepository>();
            services.AddScoped<IPropostaApi, PropostaApi>();

            services.AddHttpClient("api-proposta", client =>
            {
                client.BaseAddress = new Uri("https://localhost:7269");
            });
        }

        private static void ConfigurarAWSServices(IServiceCollection services, IConfiguration configuration)
        {
            string? connectionString = null;
            string topicoArn;

            try
            {
                using (var secretsManager = new SecretsManager())
                {
                    connectionString = secretsManager.ObterAsync("API-CONTRATACAO-CONEXAO").GetAwaiter().GetResult();
                    topicoArn = secretsManager.ObterAsync("API-CONTRATACAO-NOTIFICACAO-TOPICO-SNS").GetAwaiter().GetResult();
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

            services.AddSingleton<INotificacaoService>(new SnsNotificacaoService(topicoArn));
        }
    }
}
