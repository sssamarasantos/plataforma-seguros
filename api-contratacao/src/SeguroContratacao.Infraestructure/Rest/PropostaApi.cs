using SeguroContratacao.Domain.Interfaces;
using SeguroContratacao.Domain.DTOs;
using System.Text.Json;

namespace SeguroContratacao.Infraestructure.Rest
{
    public class PropostaApi : IPropostaApi
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public PropostaApi(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory
                ?? throw new ArgumentNullException(nameof(httpClientFactory));
        }

        public async Task<PropostaDTO> ObterPropostaPorIdAsync(int idProposta)
        {
            var httpClient = _httpClientFactory.CreateClient("api-proposta");
            var responseHttp = await httpClient.GetAsync($"api/v1/proposta/{idProposta}");
            responseHttp.EnsureSuccessStatusCode();
            var content = await responseHttp.Content.ReadAsStringAsync();

            var response = JsonSerializer.Deserialize<PropostaDTO>(content);
            return response!;
        }
    }
}
