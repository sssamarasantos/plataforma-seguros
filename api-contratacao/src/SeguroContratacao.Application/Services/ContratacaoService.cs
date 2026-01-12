using SeguroContratacao.Application.DTOs;
using SeguroContratacao.Application.Interfaces;
using SeguroContratacao.Domain.Common;
using SeguroContratacao.Domain.Models;

namespace SeguroContratacao.Application.Services
{
    public class ContratacaoService : IContratacaoService
    {
        private readonly IValidacaoPropostaService _validacaoPropostaService;
        private readonly IProcessamentoContratacaoService _processamentoContratacaoService;

        public ContratacaoService(
            IValidacaoPropostaService validacaoPropostaService,
            IProcessamentoContratacaoService processamentoContratacaoService)
        {
            _validacaoPropostaService = validacaoPropostaService;
            _processamentoContratacaoService = processamentoContratacaoService;
        }

        public async Task<ResultadoOperacao> ContratarPropostaAsync(ContratacaoDTO contratacaoDto)
        {
            var resultadoProposta = await _validacaoPropostaService.ValidarPropostaParaContratacaoAsync(contratacaoDto.IdProposta);
            if (resultadoProposta.EhFalha)
            {
                return resultadoProposta.Erro!;
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

            return await _processamentoContratacaoService.ProcessarContratacaoAsync(resultadoContratacao.Valor);
        }
    }
}
