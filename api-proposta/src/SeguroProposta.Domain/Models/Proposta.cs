using SeguroProposta.Domain.Enums;

namespace SeguroProposta.Domain.Models
{
    public class Proposta
    {
        public Guid Id { get; private set; }
        public int NumeroProposta { get; private set; }
        public StatusProposta Status { get; private set; }
        public DateTime DataInclusao { get; private set; }
        public string Titulo { get; private set; }
        public string Descricao { get; private set; }
        public decimal ValorPremio { get; private set; }
        public decimal ValorCobertura { get; private set; }

        protected Proposta()
        {
        }

        public Proposta(string titulo, string descricao, decimal valorPremio, decimal valorCobertura)
        {
            Id = Guid.NewGuid();
            Status = StatusProposta.EmAnalise;
            DataInclusao = DateTime.UtcNow;
            Titulo = titulo ?? throw new ArgumentNullException(titulo);
            Descricao = descricao ?? string.Empty;
            ValorPremio = valorPremio;
            ValorCobertura = valorCobertura;
        }

        public void AlterarStatus(StatusProposta novoStatus)
        {
            if (novoStatus == Status)
                throw new InvalidOperationException("Status igual ao atual.");

            Status = novoStatus;
        }
    }
}
