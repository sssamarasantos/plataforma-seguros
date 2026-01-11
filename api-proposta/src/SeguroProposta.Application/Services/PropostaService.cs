using SeguroProposta.Application.Common;
using SeguroProposta.Application.Dtos;
using SeguroProposta.Application.DTOs;
using SeguroProposta.Application.Interfaces;
using SeguroProposta.Application.Mappers;
using SeguroProposta.Domain.Common;
using SeguroProposta.Domain.Interfaces;

namespace SeguroProposta.Application.Services
{
    public class PropostaService : IPropostaService
    {
        private readonly IPropostaRepository _propostaRepository;

        public PropostaService(IPropostaRepository propostaRepository)
        {
            _propostaRepository = propostaRepository
                ?? throw new ArgumentNullException(nameof(propostaRepository));
        }

        public async Task<IEnumerable<PropostaDTO>> BuscarTodasAsync()
        {
            var propostas = await _propostaRepository.BuscarTodasAsync();
            return propostas.Select(p => p.ToDTO());
        }

        public async Task<PropostaDTO?> BuscarPorIdAsync(int id)
        {
            var proposta = await _propostaRepository.BuscarPorIdAsync(id);
            return proposta?.ToDTO();
        }

        public async Task<ResultadoOperacao> InserirAsync(CriaPropostaDTO propostaDto)
        {
            ArgumentNullException.ThrowIfNull(propostaDto);

            var resultadoCriacao = propostaDto.ToDomain();
            if (resultadoCriacao.EhFalha)
            {
                return resultadoCriacao.Erro!;
            }

            var proposta = resultadoCriacao.Valor;

            var sucesso = await _propostaRepository.InserirAsync(proposta);

            return !sucesso
                ? ErrosApplication.InserirPropostaFalhou 
                : ResultadoOperacao.Sucesso();
        }

        public async Task<ResultadoOperacao> AlterarStatusAsync(AlteraStatusDTO alteraStatusDto)
        {
            ArgumentNullException.ThrowIfNull(alteraStatusDto);

            var proposta = await _propostaRepository.BuscarPorIdAsync(alteraStatusDto.Id);
            if (proposta is null)
            {
                return ErrosApplication.PropostaNaoEncontrada(alteraStatusDto.Id);
            }

            var alterarStatus = proposta.AlterarStatus(alteraStatusDto.Status);
            if (alterarStatus.EhFalha)
            {
                return alterarStatus.Erro!;
            }

            var sucesso = await _propostaRepository.AtualizaStatusAsync(proposta.Id, proposta.Status);

            return !sucesso
                ? ErrosApplication.AtualizarPropostaFalhou(alteraStatusDto.Id)
                : ResultadoOperacao.Sucesso();
        }
    }
}
