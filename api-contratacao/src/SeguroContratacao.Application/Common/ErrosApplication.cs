using SeguroContratacao.Domain.Common;

namespace SeguroContratacao.Application.Common
{
    public static class ErrosApplication
    {
        public static Erro ResultadoContratacaoFalhou => new(TipoErro.ErroOperacional, "Falha ao registrar a contratação.");
    }
}
