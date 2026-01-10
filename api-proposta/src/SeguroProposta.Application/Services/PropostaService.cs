using SeguroProposta.Application.Dtos;
using SeguroProposta.Application.Interfaces;
using SeguroProposta.Application.Mappers;
using SeguroProposta.Application.VOs;
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

        public async Task<IEnumerable<PropostaVO>> BuscarTodasAsync()
        {
            var propostas = await _propostaRepository.BuscarTodasAsync();

            return propostas.Select(p => p.ToVO());
        }

        public async Task<PropostaVO?> BuscarPorIdAsync(int id)
        {
            var proposta = await _propostaRepository.BuscarPorIdAsync(id);
            return proposta?.ToVO();
        }

        public async Task InserirAsync(PropostaDTO propostaDto)
        {
            ArgumentNullException.ThrowIfNull(propostaDto);

            var proposta = propostaDto.ToDomain();

            await _propostaRepository.InserirAsync(proposta);
        }

        public async Task AlterarStatusAsync(AlteraStatusDTO alteraStatusDto)
        {
            ArgumentNullException.ThrowIfNull(alteraStatusDto);

            var proposta = await _propostaRepository.BuscarPorIdAsync(alteraStatusDto.Id);
            
            if (proposta is null)
            {
                throw new InvalidOperationException($"Proposta com número {alteraStatusDto.Id} não encontrada.");
            }

            proposta.AlterarStatus(alteraStatusDto.Status);

            await _propostaRepository.AtualizaStatusAsync(proposta.Id, proposta.Status);
        }
    }
}
