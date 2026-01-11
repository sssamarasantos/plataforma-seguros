using SeguroProposta.Domain.Enums;

namespace SeguroProposta.Application.DTOs
{
    public class PropostaDTO
    {
        public int Id { get; set; }
        public required string NumeroProposta { get; set; }
        public StatusProposta Status { get; set; }
        public required string Titulo { get; set; }
        public required string Descricao { get; set; }
        public DateTime DataHoraInclusao { get; set; }
        public decimal ValorPremio { get; set; }
        public decimal ValorCobertura { get; set; }
        public required string EmailContratante { get; set; }
    }
}
