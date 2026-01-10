using SeguroContratacao.Domain.Enums;
using SeguroContratacao.Domain.Exceptions;

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

        protected Contratacao() {}

        private Contratacao(int idProposta, decimal valorPremioFinal, decimal valorCoberturaFinal)
        {
            IdProposta = idProposta;
            NumeroApolice = ValueObjects.NumeroApolice.Gerar();
            DataHoraContratacao = DateTime.UtcNow;

            ValorPremioFinal = valorPremioFinal;
            ValorCoberturaFinal = valorCoberturaFinal;
        }

        public static Contratacao Criar(int idProposta, decimal valorPremioFinal, decimal valorCoberturaFinal)
        {
            ValidarIdProposta(idProposta);
            ValidarValorPremioFinal(valorPremioFinal);
            ValidarValorCoberturaFinal(valorPremioFinal, valorCoberturaFinal);

            return new Contratacao(idProposta, valorPremioFinal, valorCoberturaFinal);
        }

        public static void ValidarStatus(StatusProposta statusProposta)
        {
            if (statusProposta != StatusProposta.Aprovada)
            {
                throw new RegraDeNegocioException("A proposta deve estar aprovada para finalizar a contratação.");
            }
        }

        private static void ValidarIdProposta(int idProposta)
        {
            if (idProposta == 0)
                throw new ContratacaoInvalidaException("O ID da proposta não pode ser zero.");
        }

        private static void ValidarValorPremioFinal(decimal valor)
        {
            if (valor <= 0)
                throw new ContratacaoInvalidaException("Valor do prêmio deve ser maior que zero.");
        }

        private static void ValidarValorCoberturaFinal(decimal valorPremioFinal, decimal valorCoberturaFinal)
        {
            if (valorCoberturaFinal < valorPremioFinal)
                throw new ContratacaoInvalidaException("Valor da cobertura deve ser maior que o prêmio.");
        }
    }
}
