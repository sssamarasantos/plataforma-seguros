using SeguroContratacao.Domain.Common;
using SeguroContratacao.Domain.Interfaces;
using SeguroContratacao.Domain.Models;
using SeguroContratacao.Infrastructure.Common;
using SeguroContratacao.Infrastructure.DTOs;
using System.Net;
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

        public async Task<ResultadoOperacao<Proposta>> ObterPropostaPorIdAsync(int idProposta)
        {
            try
            {
                var httpClient = _httpClientFactory.CreateClient("api-proposta");
                var responseHttp = await httpClient.GetAsync($"api/v1/proposta/{idProposta}");

                if (responseHttp.StatusCode == HttpStatusCode.NotFound || responseHttp.StatusCode == HttpStatusCode.NoContent)
                {
                    return ErrosInfrastructure.PropostaNaoEncontrada;
                }

                if (!responseHttp.IsSuccessStatusCode)
                {
                    return ErrosInfrastructure.ErroAoComunicarComApiProposta;
                }

                var content = await responseHttp.Content.ReadAsStringAsync();
                var dto = JsonSerializer.Deserialize<PropostaDTO>(content);

                if (dto == null)
                {
                    return ErrosInfrastructure.PropostaNaoEncontrada;
                }

                return ResultadoOperacao.Sucesso(new Proposta(dto.Id, dto.Status, dto.EmailContratante));
            }
            catch (HttpRequestException)
            {
                return ErrosInfrastructure.ErroAoComunicarComApiProposta;
            }
            catch (TaskCanceledException)
            {
                return ErrosInfrastructure.TimeoutApiProposta;
            }
            catch (JsonException)
            {
                return ErrosInfrastructure.ErroAoComunicarComApiProposta;
            }
        }
    }
}
