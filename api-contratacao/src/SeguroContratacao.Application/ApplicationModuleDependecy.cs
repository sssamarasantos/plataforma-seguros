using Microsoft.Extensions.DependencyInjection;
using SeguroContratacao.Application.Interfaces;
using SeguroContratacao.Application.Services;

namespace SeguroContratacao.Application
{
    public static class ApplicationModuleDependecy
    {
        public static void AddApplicationModuleDependecy(this IServiceCollection services)
        {
            services.AddScoped<IContratacaoService, ContratacaoService>();
            services.AddScoped<IValidacaoPropostaService, ValidacaoPropostaService>();
            services.AddScoped<IProcessamentoContratacaoService, ProcessamentoContratacaoService>();
        }
    }
}
