using SeguroProposta.Domain.Enums;
using System.Text.Json.Serialization;

namespace SeguroProposta.Application.Dtos
{
    public class AlteraStatusDTO
    {
        public int Id { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public StatusProposta Status { get; set; }
    }
}
