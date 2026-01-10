namespace SeguroProposta.Application.DTOs
{
    public class CriaPropostaDTO
    {
        public required string Titulo { get; set; }
        public required string Descricao { get; set; }
        public decimal ValorPremio { get; set; }
        public decimal ValorCobertura { get; set; }
    }
}