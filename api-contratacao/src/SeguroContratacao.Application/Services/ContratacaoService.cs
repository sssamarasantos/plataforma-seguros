using SeguroContratacao.Application.Common;
using SeguroContratacao.Application.DTOs;
using SeguroContratacao.Application.Interfaces;
using SeguroContratacao.Domain.Common;
using SeguroContratacao.Domain.Interfaces;
using SeguroContratacao.Domain.Models;

namespace SeguroContratacao.Application.Services
{
    public class ContratacaoService : IContratacaoService
    {
        private readonly IContratacaoRepository _contratacaoRepository;
        private readonly IPropostaApi _propostaApi;

        public ContratacaoService(IContratacaoRepository contratacaoRepository, IPropostaApi propostaApi)
        {
            _contratacaoRepository = contratacaoRepository;
            _propostaApi = propostaApi;
        }

        public async Task<ResultadoOperacao> ContratarPropostaAsync(ContratacaoDTO contratacaoDto)
        {
            var resultadoProposta = await _propostaApi.ObterPropostaPorIdAsync(contratacaoDto.IdProposta);
            if (resultadoProposta.EhFalha)
            {
                return resultadoProposta.Erro!;
            }

            var validacaoStatus = Contratacao.ValidarStatus(resultadoProposta.Valor.Status);
            if (validacaoStatus.EhFalha)
            {
                return validacaoStatus;
            }

            var resultadoContratacao = Contratacao.Criar(
                contratacaoDto.IdProposta,
                contratacaoDto.ValorPremioFinal,
                contratacaoDto.ValorCoberturaFinal,
                resultadoProposta.Valor.EmailContratante
            );

            if (resultadoContratacao.EhFalha)
            {
                return resultadoContratacao.Erro!;
            }

            var sucesso = await _contratacaoRepository.InserirAsync(resultadoContratacao.Valor);
            if (!sucesso)
            {
                return ErrosApplication.ResultadoContratacaoFalhou;
            }

            return ResultadoOperacao.Sucesso();
        }
    }
}
