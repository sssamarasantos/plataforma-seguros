using SeguroProposta.Domain.Enums;

namespace SeguroProposta.Application.VOs
{
    public class PropostaVO
    {
        public int Id { get; set; }
        public string NumeroProposta { get; set; }
        public StatusProposta Status { get; set; }
        public string Titulo { get; set; }
        public string Descricao { get; set; }
        public DateTime DataHoraInclusao { get; set; }
        public decimal ValorPremio { get; set; }
        public decimal ValorCobertura { get; set; }
    }
}
