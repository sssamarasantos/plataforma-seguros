using SeguroContratacao.Application.DTOs;
using SeguroContratacao.Application.Interfaces;
using SeguroContratacao.Application.Mappers;
using SeguroContratacao.Domain.Interfaces;

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

        public async Task<int> ContratarPropostaAsync(ContratacaoDTO contratacaoDto)
        {
            var proposta = await _propostaApi.ObterPropostaPorIdAsync(contratacaoDto.IdProposta);

            var novaContratacao = contratacaoDto.ToDomain();
            novaContratacao.ValidarStatus(proposta.Status);
            novaContratacao.FinalizarContratacao();

            var id = await _contratacaoRepository.InserirAsync(novaContratacao);
            return id;
        }
    }
}
