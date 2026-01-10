using SeguroContratacao.Domain.Interfaces;
using SeguroContratacao.Domain.Models;
using SeguroContratacao.Infrastructure.DTOs;
using System.Text.Json;

namespace SeguroContratacao.Infrastructure.Rest
{
    public class PropostaApi : IPropostaApi
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public PropostaApi(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory
                ?? throw new ArgumentNullException(nameof(httpClientFactory));
        }

        public async Task<Proposta> ObterPropostaPorIdAsync(int idProposta)
        {
            var httpClient = _httpClientFactory.CreateClient("api-proposta");
            var responseHttp = await httpClient.GetAsync($"api/v1/proposta/{idProposta}");
            responseHttp.EnsureSuccessStatusCode();

            var content = await responseHttp.Content.ReadAsStringAsync();
            var dto = JsonSerializer.Deserialize<PropostaDTO>(content);
            if (dto == null)
            {
                throw new InvalidOperationException("Falha ao desserializar a proposta: conteúdo inválido ou vazio.");
            }

            return new Proposta(dto.Id, dto.Status);
        }
    }
}
