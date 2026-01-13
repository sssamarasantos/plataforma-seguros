using Microsoft.Extensions.DependencyInjection;
using SeguroProposta.Application.Interfaces;
using SeguroProposta.Application.Services;

namespace SeguroProposta.Application
{
    public static class ApplicationModuleDependency
    {
        public static void AddApplicationModuleDependency(this IServiceCollection services)
        {
            services.AddScoped<IPropostaService, PropostaService>();
        }
    }
}
