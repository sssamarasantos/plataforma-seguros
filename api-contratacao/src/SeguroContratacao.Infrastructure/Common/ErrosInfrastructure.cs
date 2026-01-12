using SeguroContratacao.Domain.Common;

namespace SeguroContratacao.Infrastructure.Common
{
    public static class ErrosInfrastructure
    {
        public static Erro PropostaNaoEncontrada => new(TipoErro.NaoEncontrado, "Proposta não encontrada.");
        
        public static Erro ErroAoComunicarComApiProposta => new(TipoErro.ErroOperacional, "Erro ao comunicar com a API de propostas.");
        
        public static Erro TimeoutApiProposta => new(TipoErro.ErroOperacional, "Timeout ao comunicar com a API de propostas.");
    }
}
