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

        public Contratacao()
        {

        }

        public Contratacao(int idProposta, decimal valorPremioFinal, decimal valorCoberturaFinal)
        {
            if (idProposta == 0)
            {
                throw new ArgumentException("O ID da proposta não pode ser zero.", nameof(idProposta));
            }

            if (valorPremioFinal <= 0)
            {
                throw new ArgumentException("O valor do prêmio final deve ser maior que zero.", nameof(valorPremioFinal));
            }

            if (valorCoberturaFinal <= 0)
            {
                throw new ArgumentException("O valor da cobertura final deve ser maior que zero.", nameof(valorCoberturaFinal));
            }

            IdProposta = idProposta;
            ValorPremioFinal = valorPremioFinal;
            ValorCoberturaFinal = valorCoberturaFinal;
        }

        internal static string GerarNumeroApolice()
        {
            return Guid.NewGuid().ToString().Replace("-", "").ToUpper().Substring(0, 10);
        }

        public void ValidarStatus(StatusProposta statusProposta)
        {
            if (statusProposta != StatusProposta.Aprovada)
            {
                throw new InvalidOperationException("A proposta deve estar aprovada para finalizar a contratação.");
            }
        }

        public void FinalizarContratacao()
        {
            DataHoraContratacao = DateTime.UtcNow;
            NumeroApolice = GerarNumeroApolice();
        }
    }
}
