using SeguroContratacao.Domain.Common;
using SeguroContratacao.Domain.Enums;

namespace SeguroContratacao.Domain.Models
{
    public class Contratacao
    {
        public int Id { get; private set; }

        public DateTime DataHoraContratacao { get; private set; }

        public string NumeroApolice { get; private set; } = string.Empty;

        public int IdProposta { get; private set; }

        public decimal ValorPremioFinal { get; private set; }

        public decimal ValorCoberturaFinal { get; private set; }

        public string EmailContratante { get; private set; } = string.Empty;

        protected Contratacao() { }

        private Contratacao(
            int idProposta,
            decimal valorPremioFinal,
            decimal valorCoberturaFinal,
            string emailContratante)
        {
            IdProposta = idProposta;
            NumeroApolice = ValueObjects.NumeroApolice.Gerar();
            DataHoraContratacao = DateTime.UtcNow;
            ValorPremioFinal = valorPremioFinal;
            ValorCoberturaFinal = valorCoberturaFinal;
            EmailContratante = emailContratante;
        }

        public static ResultadoOperacao<Contratacao> Criar(
            int idProposta,
            decimal valorPremioFinal,
            decimal valorCoberturaFinal,
            string emailContratante)
        {
            var validacaoIdProposta = ValidarIdProposta(idProposta);
            if (validacaoIdProposta.EhFalha)
                return validacaoIdProposta.Erro!;

            var validacaoValorPremio = ValidarValorPremioFinal(valorPremioFinal);
            if (validacaoValorPremio.EhFalha)
                return validacaoValorPremio.Erro!;

            var validacaoValorCobertura = ValidarValorCoberturaFinal(valorPremioFinal, valorCoberturaFinal);
            if (validacaoValorCobertura.EhFalha)
                return validacaoValorCobertura.Erro!;

            return new Contratacao(idProposta, valorPremioFinal, valorCoberturaFinal, emailContratante);
        }

        public static ResultadoOperacao ValidarStatus(StatusProposta statusProposta)
        {
            if (statusProposta != StatusProposta.Aprovada)
            {
                return ErrosDomain.TituloObrigatorio;
            }

            return ResultadoOperacao.Sucesso();
        }

        private static ResultadoOperacao ValidarIdProposta(int idProposta)
        {
            if (idProposta == 0)
                return ErrosDomain.IdPropostaInvalido;

            return ResultadoOperacao.Sucesso();
        }

        private static ResultadoOperacao ValidarValorPremioFinal(decimal valor)
        {
            if (valor <= 0)
                return ErrosDomain.ValorPremioInvalido;

            return ResultadoOperacao.Sucesso();
        }

        private static ResultadoOperacao ValidarValorCoberturaFinal(decimal valorPremioFinal, decimal valorCoberturaFinal)
        {
            if (valorCoberturaFinal <= valorPremioFinal)
                return ErrosDomain.ValorCoberturaInvalido;

            return ResultadoOperacao.Sucesso();
        }
    }
}
