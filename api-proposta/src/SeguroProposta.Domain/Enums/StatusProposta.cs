using System.Text.Json.Serialization;

namespace SeguroProposta.Domain.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum StatusProposta
    {
        EmAnalise = 1,
        Aprovada = 2,
        Rejeitada = 3
    }
}
