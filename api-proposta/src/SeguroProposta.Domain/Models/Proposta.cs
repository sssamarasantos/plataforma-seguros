using SeguroProposta.Domain.Enums;

namespace SeguroProposta.Domain.Models
{
    public class Proposta
    {
        public int Id { get; private set; }
        public string NumeroProposta { get; private set; }
        public StatusProposta Status { get; private set; }
        public string Titulo { get; private set; }
        public string Descricao { get; private set; }
        public DateTime DataHoraInclusao { get; private set; }
        public decimal ValorPremio { get; private set; }
        public decimal ValorCobertura { get; private set; }

        protected Proposta()
        {
        }

        public Proposta(string titulo, string descricao, decimal valorPremio, decimal valorCobertura)
        {
            NumeroProposta = GerarNumeroProposta();
            Status = StatusProposta.EmAnalise;
            DataHoraInclusao = DateTime.UtcNow;
            Titulo = titulo ?? throw new ArgumentNullException(titulo);
            Descricao = descricao ?? string.Empty;
            ValorPremio = valorPremio;
            ValorCobertura = valorCobertura;
        }

        internal static string GerarNumeroProposta()
        {
            return Guid.NewGuid().ToString().Replace("-", "").ToUpper().Substring(0, 10);
        }

        public void AlterarStatus(StatusProposta novoStatus)
        {
            if (novoStatus == Status)
                throw new InvalidOperationException("Status igual ao atual.");

            Status = novoStatus;
        }
    }
}
