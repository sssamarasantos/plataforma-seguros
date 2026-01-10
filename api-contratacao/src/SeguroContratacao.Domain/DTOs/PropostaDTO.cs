using SeguroContratacao.Domain.Enums;
using System.Text.Json.Serialization;

namespace SeguroContratacao.Domain.DTOs
{
    public class PropostaDTO
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("status")]
        public StatusProposta Status { get; set; }
    }
}
