using SeguroContratacao.Application.Interfaces;
using SeguroContratacao.Domain.Common;
using SeguroContratacao.Domain.Interfaces;
using SeguroContratacao.Infrastructure.DTOs;

namespace SeguroContratacao.Application.Services
{
    public class ValidacaoPropostaService : IValidacaoPropostaService
    {
        private readonly IPropostaApi _propostaApi;

        public ValidacaoPropostaService(IPropostaApi propostaApi)
        {
            _propostaApi = propostaApi;
        }

        public async Task<ResultadoOperacao<PropostaDTO>> ValidarPropostaParaContratacaoAsync(int idProposta)
        {
            var resultadoProposta = await _propostaApi.ObterPropostaPorIdAsync(idProposta);
            if (resultadoProposta.EhFalha)
            {
                return resultadoProposta.Erro!;
            }

            var validacaoStatus = Domain.Models.Contratacao.ValidarStatus(resultadoProposta.Valor.Status);
            if (validacaoStatus.EhFalha)
            {
                return validacaoStatus.Erro!;
            }

            var propostaDto = new PropostaDTO
            {
                Id = resultadoProposta.Valor.Id,
                Status = resultadoProposta.Valor.Status,
                EmailContratante = resultadoProposta.Valor.EmailContratante
            };

            return ResultadoOperacao.Sucesso(propostaDto);
        }
    }
}