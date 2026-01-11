using SeguroContratacao.Application.DTOs;
using SeguroContratacao.Application.Interfaces;
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

        public async Task ContratarPropostaAsync(ContratacaoDTO contratacaoDto)
        {
            var proposta = await _propostaApi.ObterPropostaPorIdAsync(contratacaoDto.IdProposta);

            Contratacao.ValidarStatus(proposta.Status);

            var novaContratacao = Contratacao.Criar(
                contratacaoDto.IdProposta,
                contratacaoDto.ValorPremioFinal,
                contratacaoDto.ValorCoberturaFinal,
                proposta.EmailContratante
            );

            var sucesso = await _contratacaoRepository.InserirAsync(novaContratacao);
            if (!sucesso)
            {
                throw new Exception("Falha ao registrar a contratação.");
            }
        }
    }
}
